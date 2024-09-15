using Backend.Models;

namespace Backend.Dto;

public class DtoMessage(Guid guid, string senderId, Guid spaceGuid, string content, DateTime postedAt)
{
    public Guid Guid { get; set; } = guid;
    public string SenderId { get; } = senderId;
    public Guid SpaceGuid { get; } = spaceGuid;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;

    public static explicit operator DtoMessage(Message message) =>
        new(message.Guid, message.Sender.Id, message.Space.Guid, message.Content, message.PostedAt);
}