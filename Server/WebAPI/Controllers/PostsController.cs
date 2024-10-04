// using DTOs;
// using Microsoft.AspNetCore.Mvc;
// using RepositoryContracts;
//
// namespace WebAPI.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class PostsController
// {
//    private readonly IPostRepository _postRepository;
//    
//    public PostsController(IPostRepository postRepository)
//    {
//       _postRepository = postRepository;
//    }
//
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<PostDTO>>> GetUsers([FromQuery])
//    {
//       
//    }
// }