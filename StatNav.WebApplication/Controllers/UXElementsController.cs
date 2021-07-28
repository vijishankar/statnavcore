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
    public class UXElementsController: Controller
    {
        private readonly IUXElementsRepository _uxElementsRepository;
        private readonly ILogger<UXElementsController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "UXElements";

        public UXElementsController(IUXElementsRepository uxElementsRepository, IMapper mapper, ILogger<UXElementsController> logger)
        {
            _uxElementsRepository = uxElementsRepository;
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

                List<UXElements> uxElements = _uxElementsRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "UXElements";
                ViewBag.SearchString = searchString;
                var uxElementsModelList = _mapper.Map<List<UXElementsVM>>(uxElements);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(uxElementsModelList);
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
                UXElements thisUXElements = _uxElementsRepository.Load(id);

                if (thisUXElements == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXElementsVM>(thisUXElements);
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
                UXElementsVM newUXE = new UXElementsVM();
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
        public IActionResult Create(UXElementsVM newUXE)
        {
            string pageAction = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXElements>(newUXE);
                    _uxElementsRepository.Add(uxeModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = uxeModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                returnModelToEdit(pageAction, ref newUXE);
                return View("Edit", newUXE);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref newUXE);
                return View("Edit", newUXE);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                UXElements thisUXE = _uxElementsRepository.Load(id);
                if (thisUXE == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXElementsVM>(thisUXE);
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
        public IActionResult Edit(UXElementsVM editedUXE)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var uxeModel = _mapper.Map<UXElements>(editedUXE);
                    _uxElementsRepository.Edit(uxeModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedUXE.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedUXE.Id);
                returnModelToEdit(pageAction, ref editedUXE);
                return View(pageAction, editedUXE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref editedUXE);
                return View(pageAction, editedUXE);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                UXElements delUxe = _uxElementsRepository.Load(id);
                if (delUxe == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var uxeModel = _mapper.Map<UXElementsVM>(delUxe);
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
                _uxElementsRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                UXElements thisUXE = _uxElementsRepository.Load(id);
                var uxeModel = _mapper.Map<UXElementsVM>(thisUXE);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(uxeModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Parents = _uxElementsRepository.GetParents();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void returnModelToEdit(string action, ref UXElementsVM uxe)
        {
            try
            {
                ViewBag.Action = action;
               // SetDDLs();
                if (action == "Edit")
                {
                    var uxExperiment = _uxElementsRepository.GetUXEles(uxe.Id);
                    uxe.UXExperiments = _mapper.Map<List<UXExperimentsVM>>(uxExperiment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
