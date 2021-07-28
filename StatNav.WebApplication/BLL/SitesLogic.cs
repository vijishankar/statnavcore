using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class SitesLogic
    {
        public static IQueryable<Sites> FilterSites(IQueryable<Sites> sts, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                sts = sts.Where(x => x.SiteName.ToLower().Contains(searchString.ToLower()));
            }
            return sts;
        }
        public static List<Sites> SortSites(List<Sites> gList, string sortOrder)
        {
            IOrderedEnumerable<Sites> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = gList.OrderByDescending(x => x.SiteName);
                    break;
                case "Id":
                    sortedList = gList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = gList.OrderByDescending(x => x.Id);
                    break;
                case "model":
                    sortedList = gList.OrderBy(x => x.MarketingModel.ModelName);
                    break;
                case "model_desc":
                    sortedList = gList.OrderByDescending(x => x.MarketingModel.ModelName);
                    break;
                case "language":
                    sortedList = gList.OrderBy(x => x.Language);
                    break;
                case "language_desc":
                    sortedList = gList.OrderByDescending(x => x.Language);
                    break;
                default:
                    sortedList = gList.OrderBy(x => x.SiteName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
