using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    
    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }
    
    public async Task StartAsync()
    {
        Console.WriteLine("CLI App Started");
        await ShowMenuAsync();
    }
    
    private async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. View Posts");
            Console.WriteLine("3. Delete Post");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    ViewPosts();
                    break;
                case "3":
                    await DeletePostAsync();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    
    private async Task CreatePostAsync()
    {
        var createPost = new ManageForumPosts.CreatePost(_postRepository);
        await createPost.CreateForumPost();
    }

    private void ViewPosts()
    {
        var listPosts = new ManageForumPosts.ListPosts(_postRepository);
        listPosts.DisplayPosts();
    }

    private async Task DeletePostAsync()
    {
        var deletePost = new ManageForumPosts.DeletePost(_postRepository);
        await deletePost.DeleteForumPost();
    }
}
