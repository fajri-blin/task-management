﻿using Task_Management.Model.Data;

namespace Task_Management.DTOs.AccountDto;

public class AccountDto
{
    public Guid Guid { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int OTP { get; set; }
    public bool IsUsedOTP { get; set; }
    public string? Password { get; set; }
    public string? ImageProfile { get; set; }


    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            OTP = account.OTP,
            IsUsedOTP = account.IsUsedOTP,
            Password = account.Password,
            ImageProfile = account.ImageProfile,
        };
    }

    public static explicit operator Account(AccountDto account)
    {
        return new Account
        {
            Guid = account.Guid,
            Username= account.Username,
            Email = account.Email,
            OTP = account.OTP,
            IsUsedOTP = account.IsUsedOTP,
            Password = account.Password,
            ImageProfile = account.ImageProfile,
            ModifiedAt = DateTime.MinValue,
            CreatedAt = DateTime.MinValue,
        };
    }
}
