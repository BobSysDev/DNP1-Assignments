namespace Entities;

public class Comment
{
    public Comment(string id, int userId, int parentId, string body)
    {
        Id = id;
        UserId = userId;
        ParentId = parentId;
        Body = body;
        Likes = 0;
    }

    public string Id { get; set; }
    public int UserId { get; set; }
    public int ParentId { get; set; }
    public string Body { get; set; }
    public int Likes { get; set; }
}