using DTOs;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateNewPost([FromBody] CreatePostDTO postDto)
    {
        if (string.IsNullOrEmpty(postDto.Title))
        {
            return Results.BadRequest("Post title is required");
        }

        Post newPost = new Post()
        {
            Title = postDto.Title,
            Body = postDto.Body,
            UserId = postDto.UserId,
            Likes = 0
        };

        Post createdPost = await _postRepository.AddAsync(newPost);

        return Results.Created($"posts/{createdPost.PostId}", ConvertToDto(createdPost));
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetSinglePost([FromRoute] string id)
    {
        try
        {
            Post post = await _postRepository.GetSingleAsync(id);
            return Results.Ok(ConvertToDto(post));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IResult> GetMany([FromQuery] int? userId, [FromQuery] string? title)
    {
        IQueryable<Post> posts = _postRepository.GetMany();

        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        if (title is not null)
        {
            posts = posts.Where(p => p.Title.Contains(title));
        }

        List<PostDTO> postDtos = new();
        foreach (var post in posts)
        {
            postDtos.Add(ConvertToDto(post));
        }

        return Results.Ok(postDtos);
    }

    [HttpPost("{id}/like")]
    public async Task<IResult> LikePost([FromRoute] string id)
    {
        await _postRepository.LikePostAsync(id);
        return Results.Ok();
    }

    [HttpDelete("{id}/like")]
    public async Task<IResult> RemoveLikePost([FromRoute] string id)
    {
        await _postRepository.RemoveLikePostAsync(id);
        return Results.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeletePost([FromRoute] string id)
    {
        await _postRepository.DeleteAsync(id);
        return Results.Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IResult> UpdatePost([FromRoute] string id, [FromBody] CreatePostDTO request)
    {
        Post postToUpdate = await _postRepository.GetSingleAsync(id);
        postToUpdate.UserId = request.UserId;
        postToUpdate.Body = request.Body;
        postToUpdate.Title = request.Title;
        await _postRepository.UpdateAsync(postToUpdate);
        return Results.Ok(ConvertToDto(postToUpdate));
    }

    private static PostDTO ConvertToDto(Post post)
    {
        return new PostDTO
        {
            Body = post.Body,
            Id = post.PostId,
            Likes = post.Likes,
            Title = post.Title,
            UserId = post.UserId
        };
    }
}