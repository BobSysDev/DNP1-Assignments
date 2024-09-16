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
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}, Likes: {post.Likes}");
        }
    }
}
