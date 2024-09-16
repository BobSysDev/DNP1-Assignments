using RepositoryContracts;

namespace CLI.UI.ManageForumPosts;

public class ListPosts
{
    private readonly IPostRepository _postRepository;
        
    public ListPosts(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public void DisplayPosts()
    {
      
        var posts = _postRepository.GetMany().ToList();
        
        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }
        
        Console.WriteLine("List of posts:");
        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Content: {post.Body}, Likes: {post.Likes}");
        }
    }
    
    public void DisplayPostById(string postId)
    {
        var post = _postRepository.GetMany().FirstOrDefault(p => p.PostId == postId);

        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        Console.WriteLine($"Post ID: {post.PostId}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Content: {post.Body}");
        Console.WriteLine($"Likes: {post.Likes}");
    }
}
