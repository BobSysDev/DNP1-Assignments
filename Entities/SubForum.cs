namespace Entities;

public class SubForum
{
    public SubForum(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}