using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatNav.WebApplication.BLL
{
    public static class GoalsLogic
    {
        public static IQueryable<Goals> FilterGoals(IQueryable<Goals> gsl, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                gsl = gsl.Where(x => x.GoalName.ToLower().Contains(searchString.ToLower()));
            }
            return gsl;
        }
        public static List<Goals> SortGoals(List<Goals> gList, string sortOrder)
        {
            IOrderedEnumerable<Goals> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = gList.OrderByDescending(x => x.GoalName);
                    break;
                case "Id":
                    sortedList = gList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = gList.OrderByDescending(x => x.Id);
                    break;
                case "stage":
                    sortedList = gList.OrderBy(x => x.MetricModelStage.Title);
                    break;
                case "stage_desc":
                    sortedList = gList.OrderByDescending(x => x.MetricModelStage.Title);
                    break;
                default:
                    sortedList = gList.OrderBy(x => x.GoalName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}
