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
    public class GoalsController: Controller
    {
        private readonly IGoalsRepository _goalsRepository;
        private readonly ILogger<GoalsController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "Goals";

        public GoalsController(IGoalsRepository goalsRepository, IMapper mapper, ILogger<GoalsController> logger)
        {
            _goalsRepository = goalsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.StageSortParm = sortOrder == "stage" ? "stage_desc" : "stage";
                List<Goals> goals = _goalsRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "Goals";
                ViewBag.SearchString = searchString;
                var goalsModelList = _mapper.Map<List<GoalsVM>>(goals);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(goalsModelList);
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
                Goals thisGoals = _goalsRepository.Load(id);

                if (thisGoals == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var goalModel = _mapper.Map<GoalsVM>(thisGoals);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(goalModel);
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
                GoalsVM newGoal = new GoalsVM();
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
        public IActionResult Create(GoalsVM newGoal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var goalModel = _mapper.Map<Goals>(newGoal);
                    _goalsRepository.Add(goalModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = goalModel.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                return View("Edit", newGoal);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", newGoal);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                Goals thisGoal = _goalsRepository.Load(id);
                if (thisGoal == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var goalModel = _mapper.Map<GoalsVM>(thisGoal);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", goalModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Edit(GoalsVM editedGoal)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var goalModel = _mapper.Map<Goals>(editedGoal);
                    _goalsRepository.Edit(goalModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedGoal.Id, fromSave = "Saved" });
                }
                _logger.LogInformation(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedGoal.Id);
                return View(pageAction, editedGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(pageAction, editedGoal);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Goals delGoal = _goalsRepository.Load(id);
                if (delGoal == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var goalModel = _mapper.Map<GoalsVM>(delGoal);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View(goalModel);
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
                _goalsRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                _logger.LogError(ex, ConstantMessages.Error);
                Goals thisGoal = _goalsRepository.Load(id);
                var goalModel = _mapper.Map<GoalsVM>(thisGoal);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(goalModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.Stages = _goalsRepository.GetStages();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
