namespace Entities;

public class Post
{
    public Post(string title, string body, int postId, int userId)
    {
        Title = title;
        Body = body;
        PostId = postId;
        UserId = userId;
        Likes = 0;
    }

    public string Title { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }

}