using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class UXExperimentsLogic
    {
        public static IQueryable<UXExperiments> FilterExperiments(IQueryable<UXExperiments> uxe, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                uxe = uxe.Where(x => x.UXExperimentName.ToLower().Contains(searchString.ToLower()));
            }
            return uxe;
        }
        public static List<UXExperiments> SortExperiments(List<UXExperiments> gList, string sortOrder)
        {
            IOrderedEnumerable<UXExperiments> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = gList.OrderByDescending(x => x.UXExperimentName);
                    break;
                case "Id":
                    sortedList = gList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = gList.OrderByDescending(x => x.Id);
                    break;
                case "element":
                    sortedList = gList.OrderBy(x => x.UXElements.UXElementName);
                    break;
                case "element_desc":
                    sortedList = gList.OrderByDescending(x => x.UXElements.UXElementName);
                    break;
                default:
                    sortedList = gList.OrderBy(x => x.UXExperimentName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
