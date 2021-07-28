using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class VariantRepository : GenericRepository<Variant>, IVariantRepository
    {
        public override List<Variant> LoadList(string sortOrder, string searchString)
        {
            IQueryable<Variant> variants = Db.Variant;
            variants = VariantLogic.FilterVariants(variants, searchString);
            return SortList(variants.ToList(), sortOrder);

        }

        public List<Variant> SortList(List<Variant> variants, string sortOrder)
        {
            return VariantLogic.SortVariants(variants, sortOrder);
        }

        public override Variant Load(int id)
        {
            Variant variant = Db.Variant
                                .Where(x => x.Id == id)
                                .Include(x => x.Experiment)
                                .Include(x=>x.VariantImpactMetricModel)
                                .Include(x=>x.VariantTargetMetricModel)
                                .FirstOrDefault();
            return variant;
        }        

        public IList<Experiment> GetExperiments()
        {
            IList<Experiment> ex = Db.Experiment
                                              .OrderBy(x => x.ExperimentName).ToList();
            return ex;
        }
        public IList<MetricModel> GetMetricModels()
        {
            IList<MetricModel> mm = Db.MetricModel
              .OrderBy(x => x.Title).ToList();
            return mm;
        }
    }
}