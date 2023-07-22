﻿using Microsoft.AspNetCore.Mvc;
using Task_Management.Service;
using Task_Management.DTOs.AccountProgressDto;
using Microsoft.AspNetCore.Authorization;
using Task_Management.Utilities.Enum;
using System.Net;
using Task_Management.DTOs.AccountDto;
using Task_Management.Utilities.Handler;
using Task_Management.DTOs.AccountRoleDto;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class AccountProgressController : ControllerBase
{
    private readonly AccountProgressService _accountRoleSevices;

    public AccountProgressController(AccountProgressService accountSevices)
    {
        _accountRoleSevices = accountSevices;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountRoleSevices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AccountProgressDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = (AccountProgressDto)entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _accountRoleSevices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AccountRoleDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    public IActionResult Create(NewAccountProgressDto entity)
    {
        var created = _accountRoleSevices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Failed to created"
        });
        
        return Ok(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Successfully created",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(AccountProgressDto entity) 
    {
        var updated = _accountRoleSevices.Update(entity);
        if(updated is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountRoleSevices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data failed to deleted"
        });
    }
    //==========
}
