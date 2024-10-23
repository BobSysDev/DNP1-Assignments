namespace DTOs;

public class SimpleCommentDTO
{
    public string Id { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public string ParentId { get; set; }
    public int Likes { get; set; }
}