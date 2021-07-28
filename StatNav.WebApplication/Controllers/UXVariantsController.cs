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
    public class UXVariantsController: Controller
    {
        private readonly IUXVariantsRepository _uxVariantsRepository;
        private readonly ILogger<UXVariantsController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "UXVariants";

        public UXVariantsController(IUXVariantsRepository uxVariantsRepository, IMapper mapper, ILogger<UXVariantsController> logger)
        {
            _uxVariantsRepository = uxVariantsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.ExperimentSortParm = sortOrder == "experiment" ? "experiment_desc" : "experiment";

                List<UXVariants> uxVariants = _uxVariantsRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "UXVariants";
                ViewBag.SearchString = searchString;
                var uxVariantsModelList = _mapper.Map<List<UXVariantsVM>>(uxVariants);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(uxVariantsModelList);
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
                UXVariants thisUXVariants = _uxVariantsRepository.Load(id);

                if (thisUXVariants == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXVariantsVM>(thisUXVariants);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(uxeModel);
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
                UXVariantsVM newUXE = new UXVariantsVM();
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newUXE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(UXVariantsVM newUXE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXVariants>(newUXE);
                    _uxVariantsRepository.Add(uxeModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = uxeModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                return View("Edit", newUXE);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", newUXE);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                UXVariants thisUXE = _uxVariantsRepository.Load(id);
                if (thisUXE == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXVariantsVM>(thisUXE);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", uxeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(UXVariantsVM editedUXE)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXVariants>(editedUXE);
                    _uxVariantsRepository.Edit(uxeModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedUXE.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedUXE.Id);
                return View(pageAction, editedUXE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(pageAction, editedUXE);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                UXVariants delUxe = _uxVariantsRepository.Load(id);
                if (delUxe == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXVariantsVM>(delUxe);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(uxeModel);
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
                _uxVariantsRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                UXVariants thisUXE = _uxVariantsRepository.Load(id);
                var uxeModel = _mapper.Map<UXVariantsVM>(thisUXE);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(uxeModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Experiments = _uxVariantsRepository.GetExperiments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
