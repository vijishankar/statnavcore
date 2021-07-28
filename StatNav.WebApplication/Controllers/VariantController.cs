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
    public class VariantController : Controller
    {
        private readonly IVariantRepository _vRepository;
        private readonly ILogger<VariantController> _logger;
        private readonly IMapper _mapper;
        private readonly string pageName = "Variant";
        public VariantController(IVariantRepository variantRepository, IMapper mapper, ILogger<VariantController> logger)
        {
            _vRepository = variantRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                List<Variant> variants = _vRepository.LoadList(sortOrder, searchString);
                ViewBag.SelectedType = "Variant";
                ViewBag.Sortable = true;
                ViewBag.SearchString = searchString;
                var variantsModelList = _mapper.Map<List<VariantVM>>(variants);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(variantsModelList);
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
                Variant thisVariant = _vRepository.Load(id);
                if (thisVariant == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var variantsModel = _mapper.Map<VariantVM>(thisVariant);
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View(variantsModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }


        }

        public IActionResult Create(int? experimentId = 0)
        {
            try
            {
                ViewBag.Action = "Create";
                VariantVM newVariant = new VariantVM();
                if (experimentId != null) { newVariant.ExperimentId = experimentId.GetValueOrDefault(); }

                SetDDLs();
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName));
                return View("Edit", newVariant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(VariantVM newVariant)
        {
            string pageAction = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var variantsModel = _mapper.Map<Variant>(newVariant);
                    _vRepository.Add(variantsModel);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{page}", pageName));
                    return RedirectToAction("Edit", new { id = variantsModel.Id, fromSave = "Saved" });
                }
                _logger.LogWarning(ConstantMessages.CreateFailure.Replace("{page}", pageName));
                returnModelToEdit(pageAction);
                return View("Edit", newVariant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction);
                return View("Edit", newVariant);
            }
        }

        public IActionResult Edit(int id, string fromSave)
        {
            try
            {
                ViewBag.Action = "Edit";
                Variant thisVariant = _vRepository.Load(id);

                if (thisVariant == null)
                {
                    _logger.LogWarning(ConstantMessages.LoadFailure.Replace("{page}", pageName));
                    return NotFound();
                }
                var variantsModel = _mapper.Map<VariantVM>(thisVariant);
                SetDDLs();
                if (fromSave == "Saved")
                { ViewBag.Notification = "Save Successful"; }
                _logger.LogInformation(ConstantMessages.Load.Replace("{page}", pageName), id);
                return View("Edit", variantsModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public IActionResult Edit(VariantVM editedVariant)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    var variantsModel = _mapper.Map<Variant>(editedVariant);
                    _vRepository.Edit(variantsModel);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{page}", pageName));
                    return RedirectToAction(pageAction, new { id = editedVariant.Id, fromSave = "Saved" });
                }
                returnModelToEdit(pageAction);
                _logger.LogWarning(ConstantMessages.UpdateFailure.Replace("{page}", pageName), editedVariant.Id);
                return View(pageAction, editedVariant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                returnModelToEdit(pageAction);
                return View(pageAction, editedVariant);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Variant delVariant = _vRepository.Load(id);
                if (delVariant == null)
                {
                    _logger.LogWarning(ConstantMessages.DeleteFailure.Replace("{page}", pageName), id);
                    return NotFound();
                }
                var variantsModel = _mapper.Map<VariantVM>(delVariant);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName));
                return View(variantsModel);
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
                _vRepository.Remove(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{page}", pageName), id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Variant thisVariant = _vRepository.Load(id);
                var variantsModel = _mapper.Map<VariantVM>(thisVariant);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                _logger.LogError(ex, ConstantMessages.Error);
                return View(variantsModel);
            }
        }

        private void SetDDLs()
        {
            try
            {
                ViewBag.MetricModels = _vRepository.GetMetricModels();
                ViewBag.Experiments = _vRepository.GetExperiments();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void returnModelToEdit(string action)
        {
            try
            {
                ViewBag.Action = action;
                SetDDLs();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
