using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatNav.WebApplication.BLL;
using System;

namespace StatNav.WebApplication.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName ="Dashboard";

        public DashboardController(IMapper mapper, ILogger<DashboardController> logger)
        {
            _logger = logger;
            _mapper = mapper;
        }

        //empty placeholder controller for the moment - for demo in Feb 2020
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return View();
            }
        
        }
    }
}