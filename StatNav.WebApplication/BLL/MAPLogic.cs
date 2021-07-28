using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class MAPLogic
    {
        public static IQueryable<MarketingAssetPackage> FilterMAPs(IQueryable<MarketingAssetPackage> maps, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                maps = maps.Where(x => x.Mapname.ToLower().Contains(searchString.ToLower()) );
            }
            return maps;
        }
        public static List<MarketingAssetPackage> SortMAPs(List<MarketingAssetPackage> mapList, string sortOrder)
        {
            IOrderedEnumerable<MarketingAssetPackage> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = mapList.OrderByDescending(x => x.Mapname);
                    break;
                //story 2069 status and ID removed from UI              
                default:
                    sortedList = mapList.OrderBy(x => x.Mapname);
                    break;
            }

            return sortedList.ToList();
        }
    }
}