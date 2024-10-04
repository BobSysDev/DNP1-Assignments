namespace DTOs;

public class CommentDTO
{
    public string Id { get; set; }
    public string Body { get; set; }
    public string UserId { get; set; }
    public string ParentId { get; set; }
    public List<CommentDTO> Children { get; set; }
}