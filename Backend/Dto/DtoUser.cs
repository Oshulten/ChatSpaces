using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Dto;

public class DtoUser(string id, string username)
{
    public string Id { get; set; } = id;
    public string Username { get; set; } = username;

    public static explicit operator DtoUser(User user) =>
        new(user.Id, user.Username);
}