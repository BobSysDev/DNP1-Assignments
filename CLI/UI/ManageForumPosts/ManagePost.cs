using RepositoryContracts;

namespace CLI.UI.ManageForumPosts;

public class ManagePost
{
    private readonly IPostRepository _postRepository;

    public ManagePost(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task ManagePosts()
    {
        Console.WriteLine("1. Create a new post");
        Console.WriteLine("2. Delete a post");
        Console.WriteLine("3. List all posts");
        var choice = Console.ReadLine();
    
        switch (choice)
        {
            case "1":
                var createPost = new CreatePost(_postRepository);
                await createPost.CreateForumPost();
                break;
            case "2":
                var deletePost = new DeletePost(_postRepository);
                await deletePost.DeleteForumPost();
                break;
            case "3":
                var listPosts = new ListPosts(_postRepository);
                listPosts.DisplayPosts();
                break;
            case "4":
                Console.WriteLine("Enter the Post ID to view:");
                var postId = Console.ReadLine();
                var listSinglePost = new ListPosts(_postRepository);
                listSinglePost.DisplayPostById(postId);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

}