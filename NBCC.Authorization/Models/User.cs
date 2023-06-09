﻿namespace NBCC.Authorization.Models;

public sealed record User : IUser
{
    public int UserId { get; }
    public string UserName { get; } = string.Empty;
    public string Email { get; } = string.Empty;
    public List<Role> Roles { get; } = new();

    public User() { }
    public User(int userId, string userName, string email, List<Role> roles)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        Roles = roles;
    }
}