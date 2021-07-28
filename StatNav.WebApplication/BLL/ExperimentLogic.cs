using StatNav.WebApplication.DAL;
using System;
using System.Collections.Generic;
using System.Linq;


namespace StatNav.WebApplication.BLL
{
    public static class ExperimentLogic
    {
        public static IQueryable<Experiment> FilterExperiments(IQueryable<Experiment> experiments, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                experiments = experiments.Where(x => x.ExperimentName.ToLower().Contains(searchString.ToLower()));
            }
            return experiments;
        }
        public static List<Experiment> SortExperiments(List<Experiment> experimentList, string sortOrder)
        {            
            IOrderedEnumerable<Experiment> sortedList;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedList = experimentList.OrderByDescending(x => x.ExperimentName);
                    break;
                case "StartDate":
                    sortedList = experimentList.OrderBy(x => x.StartDateTime);
                    break;
                case "startDate_desc":
                    sortedList = experimentList.OrderByDescending(x => x.StartDateTime);
                    break;
                case "EndDate":
                    sortedList = experimentList.OrderBy(x => x.EndDateTime);
                    break;
                case "endDate_desc":
                    sortedList = experimentList.OrderByDescending(x => x.EndDateTime);
                    break;
                case "Id":
                    sortedList = experimentList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    sortedList = experimentList.OrderByDescending(x => x.Id);
                    break;
                default:
                    sortedList = experimentList.OrderBy(x => x.ExperimentName);
                    break;
            }
            return sortedList.ToList();
        }
    }
}