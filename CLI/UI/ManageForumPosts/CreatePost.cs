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
        Console.WriteLine("[Enter post title:]");
        var title = Console.ReadLine();
        
        if(title == null)
        {
            Console.WriteLine("[Title cannot be empty.]");
            return;
        }
        
        if (title.Length > 50)
        {
            Console.WriteLine("[Title cannot be more than 50 characters.]");
            return;
        }

        var existingpost = _postRepository.GetMany().FirstOrDefault(p => p.Title == title);
        if (existingpost != null)
        {
            Console.WriteLine($"[A post with the title '{title}' already exists.]");
            return;
        }
        
        Console.WriteLine("[Enter post content:]");
        var body = Console.ReadLine();
        
        var newPost = new Post (title, body, "",1 );
        await _postRepository.AddAsync(newPost);
        Console.WriteLine("+ Post created successfully! +");
        Console.WriteLine($"+ Title: {newPost.Title} +");
        Console.WriteLine($"+ Content: {newPost.Body} +");
    }
}