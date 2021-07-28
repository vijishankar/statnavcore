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
    public class SitesController:Controller
    {
        private readonly ISitesRepository _sitesRepository;
        private readonly ILogger<SitesController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "Sites";

        public SitesController(ISitesRepository sitesRepository, IMapper mapper, ILogger<SitesController> logger)
        {
            _sitesRepository = sitesRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
               // ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ModelSortParm = sortOrder == "model" ? "model_desc" : "model";
                ViewBag.LanguageSortParm = sortOrder == "language" ? "language_desc" : "language";

                List<Sites> Sites = _sitesRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "Sites";
                ViewBag.SearchString = searchString;
                var SitesList = _mapper.Map<List<SitesVM>>(Sites);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(SitesList);
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
                Sites thisSites = _sitesRepository.Load(id);

                if (thisSites == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var sites = _mapper.Map<SitesVM>(thisSites);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(sites);
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
                SitesVM newGoal = new SitesVM();
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(SitesVM newSite)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var site = _mapper.Map<Sites>(newSite);
                    _sitesRepository.Add(site);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = site.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                return View("Edit", newSite);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", newSite);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                Sites thisGoal = _sitesRepository.Load(id);
                if (thisGoal == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var site = _mapper.Map<SitesVM>(thisGoal);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", site);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(SitesVM editedSite)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var site = _mapper.Map<Sites>(editedSite);
                    _sitesRepository.Edit(site);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedSite.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedSite.Id);
                return View(pageAction, editedSite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(pageAction, editedSite);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Sites delGoal = _sitesRepository.Load(id);
                if (delGoal == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var site = _mapper.Map<SitesVM>(delGoal);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(site);
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
                _sitesRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                Sites thisGoal = _sitesRepository.Load(id);
                var site = _mapper.Map<SitesVM>(thisGoal);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(site);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Models = _sitesRepository.GetModels();
                ViewBag.Parents = _sitesRepository.GetParents();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
