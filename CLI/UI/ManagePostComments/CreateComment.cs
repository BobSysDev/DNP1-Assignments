
using Entities;
using RepositoryContracts;

public class CreateComment
{
    private ICommentRepository _commentRepository;
    private int _meAuthor;
    
    public CreateComment(ICommentRepository commentRepository, int meAuthor)
    {
        _commentRepository = commentRepository;
        _meAuthor = meAuthor;
    }
    
    public async Task CreateForumComment(string parentId)
    {
        while (true)
        {
            Console.WriteLine($"Adding new comment to [ {parentId} ] as [ {_meAuthor} ]. ");
            Console.WriteLine("Write your comment here: ");
            var content = Console.ReadLine();
            if (content is not null)
            {
                var newComment = new Comment("Temporary_ID", _meAuthor, parentId, content);
                await _commentRepository.AddAsync(newComment);
                break;
            }
            Console.WriteLine("Try again...");
        }
    }
}