using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePostComments;

public class ReadComment
{
    private ICommentRepository _commentRepository;
    private IUserRepository _userRepository;
    private int _meAuthor;

    public ReadComment(ICommentRepository commentRepository, IUserRepository userRepository, int meAuthor)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _meAuthor = meAuthor;
    }

    private async Task ViewComment(string commentId)
    {
        var comment = await _commentRepository.GetSingleAsync(commentId);
        var author = await _userRepository.GetSingleAsync(comment.UserId);

        Console.WriteLine(
            $"==== [ {comment.Id} ] ========== [ Comment by {author.Username} ] ========== [ {comment.Likes} ] ====");
        Console.WriteLine(comment.Body);
    }

    public async Task ViewForumComments(string parentId)
    {
        while (true)
        {
            var comments = _commentRepository.GetManyAsync();

            var commentsAvailable = comments.Any();

            if (commentsAvailable)
            {
                foreach (var comment in comments)
                {
                    if (comment.ParentId == parentId)
                    {
                        await ViewComment(comment.Id);
                    }
                }

                Console.WriteLine($"All comments under [ {parentId} ] have been displayed. \n" +
                                  $"More actions available: ");
            }
            else
            {
                Console.WriteLine($"No comments available under [ {parentId} ] \n" +
                                  $"More actions available: ");
            }


            Console.WriteLine("C: create a new comment under this entity \n" +
                              (commentsAvailable
                                  ? "CC: create a new comment under another comment\n" +
                                    "R: read child comments\n" +
                                    "U: update an existing comment\n" +
                                    "D: delete an existing comment\n" +
                                    "L: like a comment\n" +
                                    "DL: dislike a comment\n"
                                  : "") +
                              "E: go back");
            var input = Console.ReadLine();
            string? temporaryId = null;
            switch (input)
            {
                case "C":
                    var createComment = new CreateComment(_commentRepository, _meAuthor);
                    await createComment.CreateForumComment(parentId);
                    break;
                case "CC":
                    while (true)
                    {
                        Console.WriteLine("Which comment do you want to comment upon? ");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    var commentCreator = new CreateComment(_commentRepository, _meAuthor);
                    await commentCreator.CreateForumComment(temporaryId);
                    break;
                case "R":
                    while (true)
                    {
                        Console.WriteLine("Which comment's children would you like to view? ");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    await ViewForumComments(temporaryId);
                    break;
                case "U":
                    while (true)
                    {
                        Console.WriteLine("Which comment would you like to update? ");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    var commentUpdater = new UpdateComment(_commentRepository);
                    await commentUpdater.UpdateForumComment(temporaryId);
                    break;
                case "D":
                    while (true)
                    {
                        Console.WriteLine("Which comment would you like to delete? ");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    var commentDeleter = new DeleteComment(_commentRepository);
                    await commentDeleter.DeleteForumComment(temporaryId);

                    break;
                case "L":
                    while (true)
                    {
                        Console.WriteLine("Which comment would you like to like?");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    await _commentRepository.LikeCommentAsync(temporaryId);
                    break;
                case "DL":
                    while (true)
                    {
                        Console.WriteLine("Which comment would you like to like?");
                        Console.Write("Provide comment ID (in first square brackets): ");
                        temporaryId = Console.ReadLine();

                        if (temporaryId is null)
                        {
                            throw new InvalidOperationException(
                                "No idea how did you manage to input null from keyboard");
                        }

                        try
                        {
                            await _commentRepository.GetSingleAsync(temporaryId);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Please, try again...");
                        }
                    }

                    await _commentRepository.RemoveLikeCommentAsync(temporaryId);
                    break;
                case "E":
                    return;
                default:
                    Console.WriteLine("Command not recognized, please, try again...");
                    break;
            }
        }
    }
}