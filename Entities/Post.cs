namespace Entities;

public class Post
{
    // public Post(string title, string body, string postId, int userId)
    // {
    //     Title = title;
    //     Body = body;
    //     PostId = postId;
    //     UserId = userId;
    //     Likes = 0;
    // }
    
    public Post() {}

    public string Title { get; set; }
    public string Body { get; set; }
    public string PostId { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
    
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    

}