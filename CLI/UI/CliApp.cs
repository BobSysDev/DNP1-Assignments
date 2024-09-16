using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    // private readonly IUserRepository _userRepository;
    // private readonly ICommentRepository _commentRepository;
    // private readonly IPostRepository _postRepository;
    //
    // public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    // {
    //     _userRepository = userRepository;
    //     _commentRepository = commentRepository;
    //     _postRepository = postRepository;
    // }
    //
    // public async Task ShowMenuAsync()
    // {
    //     while (true)
    //     {
    //         Console.WriteLine("1. Register");
    //         Console.WriteLine("2. Login");
    //         Console.WriteLine("3. Exit");
    //         Console.Write("Choose an option: ");
    //         string option = Console.ReadLine();
    //         
    //         switch (option)
    //         {
    //             case "1":
    //                 await CreatePostAsync();
    //                 break;
    //             case "2":
    //                 ViewPosts();
    //                 break;
    //             case "3":
    //                 return;
    //             default:
    //                 Console.WriteLine("Invalid option. Please try again.");
    //                 break;
    //         }
    //     }
    // }
    //
    // private async Task CreatePostAsync()
    // {
    //     Console.Write("Enter post title: ");
    //     string title = Console.ReadLine();
    //     Console.Write("Enter post content: ");
    //     string content = Console.ReadLine();
    //     var post = new Post(title, content, _postRepository.GetSingleAsync(1).Result.PostId + 1, 1);
    //     await _postRepository.AddAsync(post);
    //     Console.WriteLine($"Post Created Successfully");
    // }
    //
    // private void ViewPosts()
    // {
    //     var posts = _postRepository.GetMany();
    //     foreach (var post in posts)
    //     {
    //         Console.WriteLine($"ID: {post.PostId}, Title: {post.Title}, Body: {post.Body}");
    //     }
    // }
    //
}

