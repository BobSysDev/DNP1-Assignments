using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumPosts;

public class CreatePost
{
    private readonly IPostRepository _postRepository;
    
    public CreatePost(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task CreateForumPost()
    {
        Console.WriteLine("Enter post title:");
        var title = Console.ReadLine();
        
        Console.WriteLine("Enter post content:");
        var body = Console.ReadLine();
        
        var newPost = new Post (title, body, "",1 );
        await _postRepository.AddAsync(newPost);
        Console.WriteLine("Post created successfully!");
    }
}