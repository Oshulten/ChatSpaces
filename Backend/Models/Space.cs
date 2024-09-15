using Backend.Database;

namespace Backend.Models;

public class Space(string alias)
{
    public int Id { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public List<User> Members { get; } = [];
    public List<Message> Messages { get; } = [];

    public static readonly Space Null = new("Null");
    public Space() : this(Null.Alias) { }
}