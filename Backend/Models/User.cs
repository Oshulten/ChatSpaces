
namespace Backend.Models;

public class User(string id, string username)
{
    public string Id { get; set; } = id;
    public string Username { get; set; } = username;
    public List<Space> Spaces { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
    public bool Admin { get; set; } = false;

    public static readonly User Null = new("Null", "Null");
    public User() : this(Null.Id, Null.Username) { }
}