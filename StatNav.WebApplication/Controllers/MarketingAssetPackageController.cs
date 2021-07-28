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
    public class MarketingAssetPackageController : Controller
    {
        private readonly IMAPRepository _mapRepository;

        private readonly ILogger<MarketingAssetPackageController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "PackageContainer";
        public MarketingAssetPackageController(IMAPRepository mapRepository, IMapper mapper, ILogger<MarketingAssetPackageController> logger)
        {
            _mapRepository = mapRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                //ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status"; //2069 - Status field removed from UI
                //ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id"; //2069 - ID field removed from UI
                List<MarketingAssetPackage> maps = _mapRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "MarketingAssetPackage";
                ViewBag.SearchString = searchString;
                var mapsModelList = _mapper.Map<List<MarketingAssetPackageVM>>(maps);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(mapsModelList);
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
                MarketingAssetPackage thisMap = _mapRepository.Load(id);

                if (thisMap == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var mapsModel = _mapper.Map<MarketingAssetPackageVM>(thisMap);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(mapsModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
}

        public IActionResult Create(int? packageContainerId = 0)
        {
            try
            {
                ViewBag.Action = "Create";
                SetDDLs();
                MarketingAssetPackageVM newMAP = new MarketingAssetPackageVM();
                if (packageContainerId != 0)
                { newMAP.PackageContainerId = packageContainerId.GetValueOrDefault(); }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newMAP);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
}

        [HttpPost]
        public IActionResult Create(MarketingAssetPackageVM newMAP)
        {
            string pageAction = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var mapsModel = _mapper.Map<MarketingAssetPackage>(newMAP);
                    _mapRepository.Add(mapsModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = mapsModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                returnModelToEdit(pageAction, ref newMAP);
                return View("Edit", newMAP);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref newMAP);
                return View("Edit", newMAP);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                MarketingAssetPackage thisMAP = _mapRepository.Load(id);
                if (thisMAP == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var mapsModel = _mapper.Map<MarketingAssetPackageVM>(thisMAP);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", mapsModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
}

        [HttpPost]
        public IActionResult Edit(MarketingAssetPackageVM editedMAP)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var mapsModel = _mapper.Map<MarketingAssetPackage>(editedMAP);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    _mapRepository.Edit(mapsModel);
                    return RedirectToAction(pageAction, new { id = mapsModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedMAP.Id);
                returnModelToEdit(pageAction, ref editedMAP);
                return View(pageAction, editedMAP);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref editedMAP);
                return View(pageAction, editedMAP);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                MarketingAssetPackage delMap = _mapRepository.Load(id);
                if (delMap == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var mapsModel = _mapper.Map<MarketingAssetPackageVM>(delMap);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(mapsModel);
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
                _mapRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                MarketingAssetPackage thisMap = _mapRepository.Load(id);
                var mapsModel = _mapper.Map<MarketingAssetPackageVM>(thisMap);
                ModelState.AddModelError("", ex.Message);
                return View(mapsModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.MetricModels = _mapRepository.GetMetricModels();
                ViewBag.ExperimentStatuses = _mapRepository.GetStatuses();
                ViewBag.Methods = _mapRepository.GetMethods();
                ViewBag.PackageContainers = _mapRepository.GetPCs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private void returnModelToEdit(string action, ref MarketingAssetPackageVM ep)
        {
            try
            {
                ViewBag.Action = action;
                SetDDLs();
                if (action == "Edit")
                {
                    var experiments = _mapRepository.GetExperiments(ep.Id);
                    ep.Experiment = _mapper.Map<List<ExperimentVM>>(experiments);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
}
    }
}
