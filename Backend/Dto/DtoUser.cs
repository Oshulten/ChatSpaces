using Backend.Models;

namespace Backend.Dto;

public class DtoUser(string id, string username, List<Guid> spaceGuids, List<Guid> messageGuids)
{
    public string Id { get; set; } = id;
    public string Username { get; set; } = username;
    public List<Guid> SpaceGuids { get; set; } = spaceGuids;

    public List<Guid> MessageGuids { get; set; } = messageGuids;


    public static explicit operator DtoUser(User user) =>
        new(
            user.Id,
            user.Username,
            user.Messages
                .Select(message => message.Guid)
                .ToList(),
            user.Spaces
                .Select(space => space.Guid)
                .ToList());
}