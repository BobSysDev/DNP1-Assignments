using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using DTOs;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController
{
    private readonly ICommentRepository commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateNewComment([FromBody] CreateCommentDTO request)
    {
        if (string.IsNullOrEmpty(request.Body))
        {
            return Results.BadRequest("Body of the comment required.");
        }
        if (int.IsNegative(request.UserId))
        {
            return Results.BadRequest("Comment author required.");
        }

        Comment newComment = new Comment("newly-created", request.UserId, request.ParentId, request.Body);
        Comment newlyCreatedComment = await commentRepository.AddAsync(newComment);

        CommentDTO commentToReturn = ConvertToDto(newlyCreatedComment);
        return Results.Created($"/Comment/{commentToReturn.Id}", commentToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetSingleComment([FromRoute] string id)
    {
        if(string.IsNullOrEmpty(id))
        {
            return Results.BadRequest("Id required.");
        }

        try
        {
            var dto = ConvertToDto(await commentRepository.GetSingleAsync(id));
            return Results.Accepted($"/Comment/{dto.Id}", dto);
        }
        catch (InvalidDataException e)
        {
            return Results.Problem(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IResult> GetAllComments()
    {
        var dtos = new List<SimpleCommentDTO>();
        foreach (var comment in commentRepository.GetManyAsync())
        {
            var dto = new SimpleCommentDTO()
            {
                Id = comment.Id,
                Body = comment.Body,
                ParentId = comment.ParentId,
                UserId = comment.UserId,
                Likes = comment.Likes
            };
            
            dtos.Add(dto);
        }
        
        return Results.Accepted("/Comment/", dtos);
    }

    [HttpPatch("{id}")]
    public async Task<IResult> UpdateComment([FromRoute] string id, [FromBody] UpdateCommentDTO request)
    {
        try
        {
            var updatedComment = await commentRepository.GetSingleAsync(id);
            
            var commentToUpdate = new Comment(id, request.UserId, updatedComment.ParentId, request.Body)
            {
                Likes = updatedComment.Likes
            };
            
            await commentRepository.UpdateAsync(commentToUpdate);
            return Results.Accepted($"/Comment/{id}",
                ConvertToDto(await commentRepository.GetSingleAsync(id)));
        }
        catch (InvalidDataException e)
        {
            return Results.Problem(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteComment([FromRoute] string id)
    {
        try
        {
            await commentRepository.GetSingleAsync(id);
        }
        catch (InvalidDataException e)
        {
            return Results.Problem(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.NotFound(e.Message);
        }

        await commentRepository.DeleteCascadeAsync(id);
        return Results.Ok();
    }

    [HttpPost("{id}/like")]
    public async Task<IResult> LikeComment([FromRoute] string id)
    {
        Console.WriteLine("hello");
        try
        {
            await commentRepository.LikeCommentAsync(id);
            return Results.Ok();
        }
        catch (InvalidDataException e)
        {
            return Results.Problem(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.NotFound(e.Message);
        }
    }
    
    [HttpPost("{id}/dislike")]
    public async Task<IResult> DislikeComment([FromRoute] string id)
    {
        try
        {
            await commentRepository.RemoveLikeCommentAsync(id);
            return Results.Ok();
        }
        catch (InvalidDataException e)
        {
            return Results.Problem(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.NotFound(e.Message);
        }
    }
    
    [HttpGet("/User/{userId:int}/Comments")]
    public async Task<IResult> GetCommentsByUser([FromRoute] int userId)
    {
        var dtos = new List<SimpleCommentDTO>();
        foreach (var comment in commentRepository.GetManyAsync())
        {
            if (comment.UserId != userId) continue;
            
            var dto = new SimpleCommentDTO()
            {
                Id = comment.Id,
                Body = comment.Body,
                ParentId = comment.ParentId,
                UserId = comment.UserId,
                Likes = comment.Likes
            };
            
            dtos.Add(dto);
        }
        
        return Results.Accepted($"/User/{userId}/Comments", dtos);
    }
    
    [HttpGet("/Post/{postId}/Comments")]
    public async Task<IResult> GetCommentsByPost([FromRoute] string postId)
    {
        var dtos = new List<CommentDTO>();
        foreach (var comment in commentRepository.GetManyAsync())
        {
            if (comment.ParentId != postId) continue;
            
            dtos.Add(ConvertToDto(comment));
        }
        
        return Results.Accepted($"/Post/{postId}/Comments/", dtos);
    }
    
    private CommentDTO ConvertToDto(Comment comment)
    {
        var dto = new CommentDTO()
        {
            UserId = comment.UserId,
            Body = comment.Body,
            Id = comment.Id,
            ParentId = comment.ParentId,
            Likes = comment.Likes
        };
        
        var children = new List<CommentDTO>();
        foreach (var c in commentRepository.GetManyAsync())
        {
            if (c.ParentId == comment.Id)
            {
                children.Add(ConvertToDto(c));
            }
        }
        dto.Children = children;

        return dto;
    }
}