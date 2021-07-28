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
    public class UXExperimentsController : Controller
    {
        private readonly IUXExperimentsRepository _uxExperimentsRepository;
        private readonly ILogger<UXExperimentsController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "UXExperiments";

        public UXExperimentsController(IUXExperimentsRepository uxExperimentsRepository, IMapper mapper, ILogger<UXExperimentsController> logger)
        {
            _uxExperimentsRepository = uxExperimentsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.ElementSortParm = sortOrder == "element" ? "element_desc" : "element";

                List<UXExperiments> uxExperiments = _uxExperimentsRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "UXExperiments";
                ViewBag.SearchString = searchString;
                var uxExperimentsModelList = _mapper.Map<List<UXExperimentsVM>>(uxExperiments);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(uxExperimentsModelList);
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
                UXExperiments thisUXExperiments = _uxExperimentsRepository.Load(id);

                if (thisUXExperiments == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXExperimentsVM>(thisUXExperiments);
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
                UXExperimentsVM newUXE = new UXExperimentsVM();
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
        public IActionResult Create(UXExperimentsVM newUXE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXExperiments>(newUXE);
                    _uxExperimentsRepository.Add(uxeModel);
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
                SetDDLs();
                return View("Edit", newUXE);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                UXExperiments thisUXE = _uxExperimentsRepository.Load(id);
                if (thisUXE == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXExperimentsVM>(thisUXE);
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
        public IActionResult Edit(UXExperimentsVM editedUXE)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXExperiments>(editedUXE);
                    _uxExperimentsRepository.Edit(uxeModel);
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
                UXExperiments delUxe = _uxExperimentsRepository.Load(id);
                if (delUxe == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXExperimentsVM>(delUxe);
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
                _uxExperimentsRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                UXExperiments thisUXE = _uxExperimentsRepository.Load(id);
                var uxeModel = _mapper.Map<UXExperimentsVM>(thisUXE);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(uxeModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Elements = _uxExperimentsRepository.GetElements();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
