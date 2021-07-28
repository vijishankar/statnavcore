using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class UXVariantsLogic
    {
        public static IQueryable<UXVariants> FilterVariants(IQueryable<UXVariants> uxe, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                uxe = uxe.Where(x => x.UXVariantName.ToLower().Contains(searchString.ToLower()));
            }
            return uxe;
        }
        public static List<UXVariants> SortVariants(List<UXVariants> gList, string sortOrder)
        {
            IOrderedEnumerable<UXVariants> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = gList.OrderByDescending(x => x.UXVariantName);
                    break;
                case "Id":
                    sortedList = gList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = gList.OrderByDescending(x => x.Id);
                    break;
                case "experiment":
                    sortedList = gList.OrderBy(x => x.UXExperiments.UXExperimentName);
                    break;
                case "experiment_desc":
                    sortedList = gList.OrderByDescending(x => x.UXExperiments.UXExperimentName);
                    break;
                default:
                    sortedList = gList.OrderBy(x => x.UXVariantName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}

