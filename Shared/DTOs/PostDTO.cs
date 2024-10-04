namespace DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string UserId { get; set; }
    public List<CommentDTO> Comments { get; set; }
    
} 