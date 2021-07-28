using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.Controllers
{
    public class HomeController : Controller
    {       
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "Home";
        public HomeController(IMapper mapper, ILogger<HomeController> logger)
        {
            _logger = logger;
            _mapper = mapper;
        }
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

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
