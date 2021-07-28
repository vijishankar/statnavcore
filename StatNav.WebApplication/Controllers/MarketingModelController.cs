using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.Controllers
{
    [Authorize]
    public class MarketingModelController:Controller
    {
        private readonly IMarketingModelRepository _marketingModelRepository;
        private readonly ILogger<MarketingModelController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "MarketingModel";

        public MarketingModelController(IMarketingModelRepository marketingModelRepository, IMapper mapper, ILogger<MarketingModelController> logger)
        {
            _marketingModelRepository = marketingModelRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                List<MarketingModel> marketingModel = _marketingModelRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "MarketingModel";
                ViewBag.SearchString = searchString;
                var marketinModelList = _mapper.Map<List<MarketingModelVM>>(marketingModel);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(marketinModelList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                MarketingModel thisMarketingModel = _marketingModelRepository.Load(id);

                if (thisMarketingModel == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var marketingModel = _mapper.Map<MarketingModelVM>(thisMarketingModel);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(marketingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            try
            {
                ViewBag.Action = "Create";
                MarketingModelVM newMarketing = new MarketingModelVM();
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newMarketing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(MarketingModelVM newMarketingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var marketingModel = _mapper.Map<MarketingModel>(newMarketingModel);
                    _marketingModelRepository.Add(marketingModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = marketingModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                return View("Edit", newMarketingModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", newMarketingModel);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                MarketingModel thisMarketing = _marketingModelRepository.Load(id);
                if (thisMarketing == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var marketingModel = _mapper.Map<MarketingModelVM>(thisMarketing);
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", marketingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(MarketingModelVM editedMarketingModel)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var marketingModel = _mapper.Map<MarketingModel>(editedMarketingModel);
                    _marketingModelRepository.Edit(marketingModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedMarketingModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedMarketingModel.Id);
                return View(pageAction, editedMarketingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(pageAction, editedMarketingModel);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                MarketingModel delMarketingModel = _marketingModelRepository.Load(id);
                if (delMarketingModel == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var marketingModel = _mapper.Map<MarketingModelVM>(delMarketingModel);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(marketingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _marketingModelRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                MarketingModel thisMarketingModel = _marketingModelRepository.Load(id);
                var marketingModel = _mapper.Map<MarketingModelVM>(thisMarketingModel);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(marketingModel);
            }
        }
    }
}
