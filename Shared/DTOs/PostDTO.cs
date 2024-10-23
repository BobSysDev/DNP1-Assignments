namespace DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public List<CommentDTO> Comments { get; set; }
} 