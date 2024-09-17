using CLI.UI.ManagePostComments;
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
            Console.WriteLine("====== Select an option: ======");
            Console.WriteLine("== 1. Create Post ==");
            Console.WriteLine("== 2. View Posts ==");
            Console.WriteLine("== 3. Delete Post ==");
            Console.WriteLine("== 4. Manage Posts ==");
            // Console.WriteLine("5. Like Post");
            // Console.WriteLine("6. Dislike Post");
            Console.WriteLine("== 5. Add a user ==");
            Console.WriteLine("== 6. Delete a User ==");
            Console.WriteLine("== 7. Manage a User ==");
            Console.WriteLine("== 8. Read Comments ==");
            Console.WriteLine("== 9. Exit ==");
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
                case "5":
                    await CreateUserAsync();
                    break;
                case "6":
                    await DeleteUserAsync();
                    break;
                case "7":
                    await ManageUserAsync();
                    break;  
                case "8":
                    await ReadCommentsAsync();
                    break;
                case "9":
                    Console.WriteLine("- Exiting CLI App - ");
                    return;
                default:
                    Console.WriteLine("{Invalid option. Please try again.}");
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
    
    private async Task ReadCommentsAsync()
    { 
        Console.WriteLine("[Enter the Post ID to view comments:]");
        var postId = Console.ReadLine();    
        if (postId == null)
        {
            Console.WriteLine("[Post ID cannot be empty.]");
            return;
        }

        var post = await _postRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine($"[Post with ID {postId} not found.]");
            return;
        }

        var commentReader = new ReadComment(_commentRepository, _userRepository, 1);
        await commentReader.ViewForumComments(postId);
    }
    
    private async Task CreateUserAsync()
    {
        var createUser = new ManageForumUsers.CreateUser(_userRepository);
        await createUser.CreateForumUser();
    }
    
    private async Task DeleteUserAsync()
    {
        var deleteUser = new ManageForumUsers.DeleteUser(_userRepository);
        await deleteUser.DeleteForumUser();
    }
    
    private async Task ManageUserAsync()
    {
        var manageUser = new ManageForumUsers.ManageUser(_userRepository);
        await manageUser.ManageForumUser();
}
    }




