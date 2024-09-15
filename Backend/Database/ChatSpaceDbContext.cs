using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class ChatSpaceDbContext(DbContextOptions options) : DbContext(options)
{
    private DbSet<User> Users { get; set; }
    private DbSet<Message> Messages { get; set; }
    private DbSet<Space> Spaces { get; set; }

    public Space? SpaceByGuid(Guid guid) =>
        Spaces
            .Include(space => space.Members)
            .Include(space => space.Messages)
            .FirstOrDefault(s => s.Guid == guid);

    public Message? MessageByGuid(Guid guid) =>
        Messages
            .Include(message => message.Sender)
            .Include(message => message.Space)
            .FirstOrDefault(m => m.Guid == guid);

    public User? UserById(string id) =>
        Users
            .Include(user => user.Messages)
            .Include(user => user.Spaces)
            .FirstOrDefault(user => user.Id == id);

    public void CreateUser(User user)
    {
        if (!Users.Contains(user))
        {
            Users.Add(user);
            SaveChanges();
        }
    }

    public void CreateSpace(Space space)
    {
        if (!Spaces.Contains(space))
        {
            Spaces.Add(space);
            SaveChanges();
        }
    }

    public void AddUserToSpace(string userId, Guid spaceGuid)
    {
        var user = UserById(userId);
        var space = SpaceByGuid(spaceGuid);

        if (user == null || space == null)
            return;

        space.Members.Add(user);
        user.Spaces.Add(space);
        SaveChanges();
    }

    public void AddUserToSpace(User user, Space space) =>
        AddUserToSpace(user.Id, space.Guid);

    public Message? CreateMessage(Message message)
    {
        var user = UserById(message.Sender.Id);
        var space = SpaceByGuid(message.Space.Guid);

        if (user == null || space == null)
        {
            Console.WriteLine("Non-existant user or space!");
            return null;
        }

        Messages.Add(message);
        space.Messages.Add(message);
        user.Messages.Add(message);

        SaveChanges();

        return message;
    }

    public void Clear()
    {
        Users.RemoveRange(Users);
        Spaces.RemoveRange(Spaces);
        Messages.RemoveRange(Messages);

        SaveChanges();
    }

    public List<Space>? SpaceByUserId(string userId)
    {
        if (UserById(userId) == null)
            return null;

        return Spaces
            .Where(space => space.Members.Select(m => m.Id).Contains(userId))
            .Include(space => space.Members)
            .Include(space => space.Messages)
            .ToList();
    }

    public List<Message>? MessagesBySpaceGuid(Guid spaceGuid, DateTime? before, int? numberOfMessages)
    {
        var space = SpaceByGuid(spaceGuid);

        if (space == null)
            return null;

        var messages = Messages
            .Where(m => m.Space.Guid == spaceGuid)
            .Include(message => message.Sender)
            .Include(message => message.Space)
            .OrderByDescending(m => m.PostedAt).ToList();

        if (before != null)
            messages = messages.Where(m => m.PostedAt < before).ToList();

        if (numberOfMessages != null)
            messages = messages.Take(numberOfMessages ?? 0).ToList();

        return messages;
    }
}