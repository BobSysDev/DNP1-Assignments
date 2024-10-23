namespace DTOs;

public class CreateCommentDTO
{
    public string Body { get; set; }
    public int UserId { get; set; }
    public string ParentId { get; set; } 
}