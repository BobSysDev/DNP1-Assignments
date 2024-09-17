using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePostComments;

public class DeleteComment
{
    private ICommentRepository _commentRepository;
    
    public DeleteComment(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task DeleteForumComment(string id)
    {
        while (true)
        {
            Console.Write("Are you absolutely sure you want to delete this comment and all of its children??? \n" +
                          "Type its ID to confirm, or \"N\" to cancel: ");
            var commentId = Console.ReadLine();
            if (commentId is not null && id == commentId)
            {
                await DeleteCascade(id);
                Console.WriteLine("Comment successfully deleted. ");
                break;
            }
            
            if(commentId == "N")
            {
                return;
            }
            
            Console.WriteLine("Command not recognized. Please, try again...");
        }
    }

    private async Task DeleteCascade(string parentId)
    {
        var comments = _commentRepository.GetManyAsync();
        List<Comment> children = [];
        foreach (var comment in comments)
        {
            if (comment.ParentId == parentId)
            {
                children.Add(comment);
            }
        }

        if (children.Count > 0)
        {
            foreach (var child in children)
            {
                await DeleteCascade(child.Id);
            }
        }

        await _commentRepository.DeleteAsync(parentId);
    }
}