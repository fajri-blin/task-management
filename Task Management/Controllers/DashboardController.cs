﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Dtos.Dashboard;
using Task_Management.Service;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {

        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("Manager/CountMonth/{guid}")]
        public IActionResult CountMonthManager(Guid guid)
        {
            var entity = _dashboardService.CountMonthManager(guid);
            if (entity == null) return NotFound(new ResponseHandlers<AssignmentRateDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });

            return Ok(new ResponseHandlers<AssignmentRateDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.Found.ToString(),
                Message = "Data Found",
                Data = entity
            });
        }


        [HttpGet("Manager/CountCategory/{guid}")]
        public IActionResult CountCategory(Guid guid)
        {
            var entity = _dashboardService.CountCategory(guid);
            if (entity == null) return NotFound(new ResponseHandlers<CountTop3CategoryDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });

            return Ok(new ResponseHandlers<CountTop3CategoryDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = entity
            });
        }
    }
}
