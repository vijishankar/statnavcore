using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatNav.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Interfaces;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using StatNav.WebApplication.BLL;

namespace StatNav.WebApplication.Controllers
{
    [Authorize]
    public class ExperimentController : Controller
    {
        private readonly IExperimentRepository _eRepository;
        private readonly ILogger<ExperimentController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "Experiment";
        public ExperimentController(IExperimentRepository experimentRepository, IMapper mapper, ILogger<ExperimentController> logger)
        {
            _eRepository = experimentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
                ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";
                List<Experiment> experiments = _eRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "Experiment";
                ViewBag.Sortable = true;
                ViewBag.SearchString = searchString;


                var experimentsModelList = _mapper.Map<List<ExperimentVM>>(experiments);
                ViewData.Model = experimentsModelList;
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(experimentsModelList);
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
                Experiment experiment = _eRepository.Load(id);
                if (experiment == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName),id);
                    return NotFound();
                }
                var experimentModel = _mapper.Map<ExperimentVM>(experiment);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(experimentModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create(int? mapId = 0)
        {
            try
            {
                ViewBag.Action = "Create";
                ExperimentVM newExperiment = new ExperimentVM
                {
                    StartDateTime = DateTime.Today,
                    EndDateTime = DateTime.Today
                };
                if (mapId != null) { newExperiment.MarketingAssetPackageId = mapId.GetValueOrDefault(); }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                SetDDLs();
                return View("Edit", newExperiment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(ExperimentVM newExperiment)
        {
            string pageAction = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var experiment = _mapper.Map<Experiment>(newExperiment);
                    _eRepository.Add(experiment);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = experiment.Id, fromSave = "Saved" });
                }
                returnModelToEdit(pageAction, ref newExperiment);
                return View("Edit", newExperiment);                
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref newExperiment);
                return View("Edit", newExperiment);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                Experiment experiment = _eRepository.Load(id);
                var experimentModel = _mapper.Map<ExperimentVM>(experiment);
                if (experimentModel == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName),id);
                    return NotFound();
                }

                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                return View("Edit", experimentModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(ExperimentVM editedExperiment)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var experiment = _mapper.Map<Experiment>(editedExperiment);
                    _eRepository.Edit(experiment);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedExperiment.Id, fromSave = "Saved" });
                }
                _logger.LogWarning(ConstantMessages.UpdateFailure.Replace("{page}", pageName),editedExperiment.Id);
                returnModelToEdit(pageAction, ref editedExperiment);
                return View(pageAction, editedExperiment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction, ref editedExperiment);
                return View(pageAction, editedExperiment);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Experiment delExperiment = _eRepository.Load(id);
                if (delExperiment == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var experiment = _mapper.Map<ExperimentVM>(delExperiment);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(experiment);
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
                _eRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                Experiment thisExperiment = _eRepository.Load(id);
                var experiment = _mapper.Map<ExperimentVM>(thisExperiment);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(experiment);
            }
        }

        private void SetDDLs()
        {
            var  marketingAssetPackages = _eRepository.GetMAPs();
            ViewBag.MarketingAssetPackages = _mapper.Map<List<MarketingAssetPackageVM>>(marketingAssetPackages);
        }

        private void returnModelToEdit(string action, ref ExperimentVM e)
        {
            try
            {
                ViewBag.Action = action;
                SetDDLs();
                if (action == "Edit")
                {
                    var varientModel = _eRepository.GetVariants(e.Id);
                    e.Variant = _mapper.Map<List<VariantVM>>(varientModel);
                }
            }
            catch(Exception ex)
            {
                throw ex;               
            }
        }
    }
}
