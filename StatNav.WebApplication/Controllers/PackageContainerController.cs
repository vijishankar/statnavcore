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
    public class PackageContainerController : Controller
    {
        private readonly IPackageContainerRepository _pcRepository;
        private readonly ILogger<PackageContainerController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "PackageContainer";

        public PackageContainerController(IPackageContainerRepository containerRepository, IMapper mapper, ILogger<PackageContainerController> logger)
        {
            _pcRepository = containerRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
                ViewBag.StageSortParm = sortOrder == "stage" ? "stage_desc" : "stage";
                List<PackageContainer> containers = _pcRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "PackageContainer";
                ViewBag.SearchString = searchString;
                var containersModelList = _mapper.Map<List<PackageContainerVM>>(containers);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(containersModelList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                PackageContainer thisContainer = _pcRepository.Load(id);

                if (thisContainer == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var containerModel = _mapper.Map<PackageContainerVM>(thisContainer);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(containerModel);
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
                SetDDLs();
                PackageContainerVM newContainer = new PackageContainerVM();
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newContainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(PackageContainerVM newContainer)
        {
            string pageAction = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var containerModel = _mapper.Map<PackageContainer>(newContainer);
                    _pcRepository.Add(containerModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = containerModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                returnModelToEdit(pageAction, ref newContainer);
                return View("Edit", newContainer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref newContainer);
                return View("Edit", newContainer);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                PackageContainer thisContainer = _pcRepository.Load(id);
                if (thisContainer == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName),id);
                    return NotFound();
                }
                var containerModel = _mapper.Map<PackageContainerVM>(thisContainer);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", containerModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(PackageContainerVM editedContainer)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var containerModel = _mapper.Map<PackageContainer>(editedContainer);
                    _pcRepository.Edit(containerModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedContainer.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName),editedContainer.Id);
                returnModelToEdit(pageAction, ref editedContainer);
                return View(pageAction, editedContainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref editedContainer);
                return View(pageAction, editedContainer);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                PackageContainer delContainer = _pcRepository.Load(id);
                if (delContainer == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var containerModel = _mapper.Map<PackageContainerVM>(delContainer);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(containerModel);
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
                _pcRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                PackageContainer thisContainer = _pcRepository.Load(id);
                var containerModel = _mapper.Map<PackageContainerVM>(thisContainer);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(containerModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Stages = _pcRepository.GetStages();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void returnModelToEdit(string action, ref PackageContainerVM pc)
        {
            try
            {
                ViewBag.Action = action;
                SetDDLs();
                if (action == "Edit")
                {
                    var marketingAssetPackage = _pcRepository.GetMAPs(pc.Id);
                    pc.MarketingAssetPackage = _mapper.Map<List<MarketingAssetPackageVM>>(marketingAssetPackage);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
