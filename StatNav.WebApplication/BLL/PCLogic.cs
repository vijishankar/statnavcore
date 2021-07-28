using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class PCLogic
    {
        public static IQueryable<PackageContainer> FilterPCs(IQueryable<PackageContainer> pcs, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                pcs = pcs.Where(x => x.PackageContainerName.ToLower().Contains(searchString.ToLower()) );
            }
            return pcs;
        }
        public static List<PackageContainer> SortPCs(List<PackageContainer> pcList, string sortOrder)
        {
            IOrderedEnumerable<PackageContainer> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = pcList.OrderByDescending(x => x.PackageContainerName);
                    break;
                case "type":
                    sortedList = pcList.OrderBy(x => x.Type);
                    break;
                case "type_desc":
                    sortedList = pcList.OrderByDescending(x => x.Type);
                    break;               
                case "stage":
                    sortedList = pcList.OrderBy(x => x.MetricModelStage.Title);
                    break;
                case "stage_desc":
                    sortedList = pcList.OrderByDescending(x => x.MetricModelStage.Title);
                    break;
                default:
                    sortedList = pcList.OrderBy(x => x.PackageContainerName);
                    break;
            }

            return sortedList.ToList();
        }
    }
}