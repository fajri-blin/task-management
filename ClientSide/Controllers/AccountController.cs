﻿using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Management.Dtos.AccountDto;

namespace ClientSide.Controllers;

[Authorize]
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
        var accounts = await _accountRepository.Get();
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
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

    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateVM updateVM)
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
                return View("Index");
            }
            return RedirectToAction("Index");
        }
        return View(updateVM);
    }*/

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotPass(ForgotPasswordVM forgotPasswordVM)
    {
        var result = await _accountRepository.ForgotPassword(forgotPasswordVM);
        if (result == null || result.Code != 200)
        {
            return RedirectToAction("Error", "Index");
        }

        // If the request is successful, redirect to the CheckAccountOTP action
        return RedirectToAction("CheckAccountOTP", new { email = forgotPasswordVM.Email });
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPass()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CheckAccountOTP(CheckOTPVM checkOTPVM)
    {
        var result = await _accountRepository.CheckAccountOTP(checkOTPVM);
        if(result.Code == 404)
        {
            return RedirectToAction("Error", "Index");
        }else if (result is null)
        {
            return RedirectToAction("Error", "Index");
        }
        return View();
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult CheckAccountOTP(string email)
    {
        var viewModel = new CheckOTPVM
        {
            Email = email
        };

        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ChangeAccountPassword(ChangePasswordVM changePasswordVM)
    {
        var result = await _accountRepository.ChangeAccountPassword(changePasswordVM);
        if (result == null || result.Code != 200)
        {
            return RedirectToAction("Error", "Index");
        }

        // If the password change is successful, redirect to the login page or another appropriate page.
        return RedirectToAction("SignIn");
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult ChangeAccountPassword()
    {
        return View();
    }

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
            return View("Index");
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



    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    /*    [HttpGet]
        public async Task<IActionResult> Update(Guid guid)
        {

            var account = await _accountRepository.Get(guid);
            if (account == null)
            {
                return RedirectToAction("Error", "Index");
            }

            var updateVM = new UpdateVM
            {
                Guid = account.Guid,
                Username = account.Username,
                Email = account.Email,
                Name = account.Name,
                Role = account.Role,
                ImageProfile = account.ImageProfile
            };
            return View(updateVM);
        }*/
}
