using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class UXElementsLogic
    {
        public static IQueryable<UXElements> FilterUXElements(IQueryable<UXElements> uxe, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                uxe = uxe.Where(x => x.UXElementName.ToLower().Contains(searchString.ToLower()));
            }
            return uxe;
        }
        public static List<UXElements> SortUXElements(List<UXElements> eList, string sortOrder)
        {
            IOrderedEnumerable<UXElements> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = eList.OrderByDescending(x => x.UXElementName);
                    break;
                case "Id":
                    sortedList = eList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = eList.OrderByDescending(x => x.Id);
                    break;
                default:
                    sortedList = eList.OrderBy(x => x.UXElementName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
