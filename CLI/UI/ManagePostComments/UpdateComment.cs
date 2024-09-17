using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePostComments;

public class UpdateComment
{
    private ICommentRepository _commentRepository;
    
    public UpdateComment(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    public async Task UpdateForumComment(string commentId)
    {
        var theCommentInQuestion = await _commentRepository.GetSingleAsync(commentId);
        while (true)
        {
            Console.WriteLine($"Updating comment [ {commentId} ] by [ {theCommentInQuestion.UserId} ]. ");
            Console.WriteLine("Write the new body here, or write \"<C>\" to cancel: ");
            var content = Console.ReadLine();
            if (content is not null)
            {
                if (content == "<C>")
                {
                    return;
                }

                theCommentInQuestion.Body = content;
                await _commentRepository.UpdateAsync(theCommentInQuestion);
                break;
            }
            Console.WriteLine("Try again...");
        }
    }
}