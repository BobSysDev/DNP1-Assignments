namespace DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public string ParentId { get; set; }
    public List<CommentDTO> Children { get; set; }
}