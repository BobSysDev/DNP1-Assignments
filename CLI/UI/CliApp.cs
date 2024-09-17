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
            Console.WriteLine("Select an option: ");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. View Posts");
            Console.WriteLine("3. Delete Post");
            // Console.WriteLine("4. Manage Posts");
            // Console.WriteLine("5. Like Post");
            // Console.WriteLine("6. Dislike Post");
            Console.WriteLine("7. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2": 
                    await ViewPosts();
                    break;
                case "3":
                    await DeletePostAsync();
                    break;
                case "4":
                    await ManagePostsAsync();
                    break;
                // case "5":
                //     await LikePostAsync();
                //     break;
                // case "6":
                //     await RemoveLikePostAsync();
                //     break;
                case "7":
                    Console.WriteLine("Exiting CLI App");
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

    private async Task ViewPosts()
    {
        var listPosts = new ManageForumPosts.ListPosts(_postRepository);
        listPosts.DisplayPosts();
    }

    private async Task DeletePostAsync()
    {
        var deletePost = new ManageForumPosts.DeletePost(_postRepository);
        await deletePost.DeleteForumPost();
    }

    private async Task ManagePostsAsync()
    {
        var managePost = new ManageForumPosts.ManagePost(_postRepository);
        await managePost.ManagePosts();
    }
    
    // private async Task LikePostAsync()
    // {
    //     var likePost = new ManageForumPosts.LikePost(_postRepository);
    //     await likePost.LikePost();
    // }
    //
    // private async Task RemoveLikePostAsync()
    // {
    //     var removeLikePost = new ManageForumPosts.RemoveLikePost(_postRepository);
    //     await removeLikePost.RemoveLikePost();
    // }
}


