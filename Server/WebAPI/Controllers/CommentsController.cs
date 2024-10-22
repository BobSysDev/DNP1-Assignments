using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using DTOs;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
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
        return Results.Created($"/Comments/{commentToReturn.Id}", commentToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetSingleComment([FromRoute] string id)
    {
        var dto = ConvertToDto(await commentRepository.GetSingleAsync(id));
        return Results.Accepted($"/Comments/{dto.Id}", dto);
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
                UserId = comment.UserId
            };
            
            dtos.Add(dto);
        }
        
        return Results.Accepted("/Comments/", dtos);
    }
    

    private CommentDTO ConvertToDto(Comment comment)
    {
        var dto = new CommentDTO();
        dto.UserId = comment.UserId;
        dto.Body = comment.Body;
        dto.Id = comment.Id;
        dto.ParentId = comment.ParentId;
        
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