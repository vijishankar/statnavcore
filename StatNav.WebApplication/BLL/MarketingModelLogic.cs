using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class MarketingModelLogic
    {
        public static IQueryable<MarketingModel> FilterModels(IQueryable<MarketingModel> mml, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                mml = mml.Where(x => x.ModelName.ToLower().Contains(searchString.ToLower()));
            }
            return mml;
        }
        public static List<MarketingModel> SortModels(List<MarketingModel> mmList, string sortOrder)
        {
            IOrderedEnumerable<MarketingModel> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = mmList.OrderByDescending(x => x.ModelName);
                    break;
                case "Id":
                    sortedList = mmList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = mmList.OrderByDescending(x => x.Id);
                    break;
                default:
                    sortedList = mmList.OrderBy(x => x.ModelName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
