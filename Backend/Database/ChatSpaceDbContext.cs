using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Clerk.Net.Client;

namespace Backend.Database;

public class ChatSpaceDbContext(DbContextOptions options) : DbContext(options)
{
    private DbSet<User> Users { get; set; }
    private DbSet<Message> Messages { get; set; }
    private DbSet<Space> Spaces { get; set; }

    public Space SpaceByGuid(Guid guid) =>
        Spaces
            .Include(space => space.Members)
            .Include(space => space.Messages)
            .FirstOrDefault(s => s.Guid == guid)
        ?? throw new Exception($"A space with guid {guid} doesn't exist");

    public Message MessageByGuid(Guid guid) =>
        Messages
            .Include(message => message.Sender)
            .Include(message => message.Space)
            .FirstOrDefault(m => m.Guid == guid)
        ?? throw new Exception($"A message with guid {guid} doesn't exist");

    public User UserById(string id) =>
        Users
            .Include(user => user.Messages)
            .Include(user => user.Spaces)
            .FirstOrDefault(user => user.Id == id)
        ?? throw new Exception($"A user with id {id} doesn't exist");

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

        space.Members.Add(user);
        user.Spaces.Add(space);

        SaveChanges();
    }

    public void AddUserToSpace(User user, Space space) =>
        AddUserToSpace(user.Id, space.Guid);

    public Message CreateMessage(Message message, Guid spaceGuid, string senderId)
    {
        var user = UserById(senderId);
        var space = SpaceByGuid(spaceGuid);

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

    public List<Space> SpaceByUserId(string userId) =>
        Spaces
            .Where(space => space.Members.Select(m => m.Id).Contains(userId))
            .Include(space => space.Members)
            .Include(space => space.Messages)
            .ToList();

    public List<Message> MessagesBySpaceGuid(Guid spaceGuid, DateTime? before, int? numberOfMessages)
    {
        var space = SpaceByGuid(spaceGuid);

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

    public void Seed(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        Clear();

        var spaces = new Faker<Space>()
            .RuleFor(o => o.Alias, f => f.Internet.DomainWord())
            .Generate(numberOfSpaces);

        foreach (var space in spaces)
        {
            CreateSpace(space);
        }

        var users = new Faker<User>()
            .RuleFor(o => o.Username, f => f.Person.UserName + "_" + Guid.NewGuid().ToString())
            .RuleFor(o => o.Id, f => Guid.NewGuid().ToString())
            .Generate(numberOfUsers)
            .ToList();

        foreach (var user in users)
        {
            Console.WriteLine($"userId: {user.Id}");
            CreateUser(user);
        }

        foreach (var space in spaces)
        {
            foreach (var user in users)
            {
                AddUserToSpace(user.Id, space.Guid);
            }
        }

        var messages = new Faker<Message>()
            .RuleFor(o => o.PostedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Content, f => f.Lorem.Paragraph())
            .Generate(numberOfMessages);

        foreach (var message in messages)
        {
            Console.WriteLine($"userId: {message.Sender.Id}");
        }

        foreach (var message in messages)
        {
            var userId = users[Random.Shared.Next(0, users.Count)].Id;
            var spaceGuid = spaces[Random.Shared.Next(0, spaces.Count)].Guid;
            CreateMessage(message, spaceGuid, userId);
        }
    }
}