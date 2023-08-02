﻿using ClientSide.Contract;
using ClientSide.Utilities.Enum;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientSide.Controllers;

[Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
[Controller]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _accountRepository.Get();
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };

        var accounts = result.Data.Select(entity => new AccountVM
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Role = entity.Role,
        }).ToList();
        ViewBag.Components = components;
        return View("Index", accounts);
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var account = await _accountRepository.Get(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
        var profile = new GetProfileVM
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            Name = account.Name,
        };
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;
        return View("Profile", profile);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Profile([FromForm] GetProfileVM updateProfileVM)
    {
        var result = await _accountRepository.UpdateProfile(updateProfileVM);
        return RedirectToAction("Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountVM updateVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _accountRepository.Update(updateVM);
            if (result == null)
            {
                return RedirectToAction("Error", "Index");
            }
            else if (result.Code == 404)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        return View(updateVM);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotPass(ForgotPasswordVM forgotPasswordVM)
    {
        var result = await _accountRepository.ForgotPassword(forgotPasswordVM);
        if (result == null)
        {
            return RedirectToAction("Error", "Index");
        }
        else if (result.Code == 404)
        {
            return View("Index");
        }
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterVM registerDto)
    {
        var result = await _accountRepository.Register(registerDto);
        if (result == null)
        {
            return RedirectToAction("Error", "Index");
        }
        else if (result.Code == 200)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInVM signInDto)
    {
        var result = await _accountRepository.Login(signInDto);
        if (result == null)
        {
            return RedirectToAction("Error", "Home");
        }
        else if (result.Code == 404)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        else if (result.Code == 200)
        {
            HttpContext.Session.SetString("JWToken", result.Data);
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult SignIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPass()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid guid)
    {

        var account = await _accountRepository.Get(guid);
        var updateVM = new GetAccountVM
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            Name = account.Name,
            Role = account.Role,
        };

        return View("Edit", updateVM);
    }
}
