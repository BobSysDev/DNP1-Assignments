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
        Console.WriteLine("====== Choose an option: ======");
        Console.WriteLine("== 1. Create a new post ==");
        Console.WriteLine("== 2. Delete a post ==");
        Console.WriteLine("== 3. List all posts ==");
        Console.WriteLine("== 4. View a single post ==");
        Console.WriteLine("== 5. Like a post ==");
        Console.WriteLine("== 6. Dislike a post ==");
        Console.WriteLine("== 7. Update existing post ==");
        
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
                Console.WriteLine("|Enter the Post ID to view:|");
                var postId = Console.ReadLine();
                var listSinglePost = new ListPosts(_postRepository);
                listSinglePost.DisplayPostById(postId);
                break;
            case "5":
                Console.WriteLine("|Enter the Post ID to like:|");
                var postToLikeId = Console.ReadLine();
                await _postRepository.LikePostAsync(postToLikeId);
                Console.WriteLine($"|Post {postToLikeId} has been liked.|");
                break;
            case "6":
                Console.WriteLine("|Enter the Post ID to dislike:|");
                var postToDislikeId = Console.ReadLine();
                await _postRepository.RemoveLikePostAsync(postToDislikeId);
                Console.WriteLine($"|Post {postToDislikeId} has been disliked.|");
                break;
            case "7":
                Console.WriteLine("|Enter the Post ID to update:|");
                var postToUpdateId = Console.ReadLine();
                Console.WriteLine($"Change Title: ");
                var changeTitle = Console.ReadLine();
                Console.WriteLine($"Change Body: ");
                var changeBody = Console.ReadLine();
                Console.WriteLine($"New Title: --{changeTitle}--: New Body: --{changeBody}--");
                
                var postToUpdate = await _postRepository.GetSingleAsync(postToUpdateId);
                if (postToUpdate != null)
                {
                    postToUpdate.Title = changeTitle;
                    postToUpdate.Body = changeBody;
                    await _postRepository.UpdateAsync(postToUpdate);
                    Console.WriteLine($"Post {postToUpdateId} has been updated.");
                }
                else
                {
                    Console.WriteLine($"Post with ID {postToUpdateId} not found.");
                }
                break
                    ;
            default:
                Console.WriteLine("|Invalid choice.|");
                break;
        }
    }

}