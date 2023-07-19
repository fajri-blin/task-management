﻿namespace Task_Management.DTOs.AccountDto;

public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string Role { get; set; }
    public string? ImageProfile { get;set; }
}
