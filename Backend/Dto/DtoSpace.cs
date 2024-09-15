using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Dto;

public class DtoSpace(Guid spaceGuid, string alias, List<string> memberIds, List<Guid> messageGuids)
{
    public Guid Guid { get; init; } = spaceGuid;
    public string Alias { get; set; } = alias;
    public List<string> MemberIds { get; } = memberIds;
    public List<Guid> MessageGuids { get; } = messageGuids;

    public static explicit operator DtoSpace(Space space) =>
        new(
            space.Guid,
            space.Alias,
            space.Members
                .Select(member => member.Id)
                .ToList(),
            space.Messages.
                Select(message => message.Guid)
                .ToList());
}