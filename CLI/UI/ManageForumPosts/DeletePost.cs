using RepositoryContracts;

namespace CLI.UI.ManageForumPosts;

public class DeletePost
{
    private readonly IPostRepository _postRepository;
    
    
    public DeletePost(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task DeleteForumPost()
    {
        var posts = _postRepository.GetMany().ToList();
        
        if (!posts.Any())
        {
            Console.WriteLine("No posts available to delete.");
            return;
        }
        
        Console.WriteLine("Available posts:");
        foreach (var post in posts)
        {
            Console.WriteLine($"Post ID: {post.PostId}, Title: {post.Title}");
        }
        
        Console.WriteLine("Enter the Post ID to delete:");
        var postIdInput = Console.ReadLine();
        
        var postToDelete = posts.FirstOrDefault(p => p.PostId == postIdInput);
        if (postToDelete == null)
        {
            Console.WriteLine($"Post with ID {postIdInput} not found.");
            return;
        }
        
        Console.WriteLine($"Are you sure you want to delete the post titled '{postToDelete.Title}'? (yes/no)");
        var confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            await _postRepository.DeleteAsync(postIdInput);
            Console.WriteLine($"Post with ID {postIdInput} deleted successfully!");
        }
        else
        {
            Console.WriteLine("Post deletion canceled.");
        }
        
    }
}