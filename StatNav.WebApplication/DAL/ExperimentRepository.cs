using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;


namespace StatNav.WebApplication.DAL
{
    public class ExperimentRepository : GenericRepository<Experiment>, IExperimentRepository
    {       
        public override List<Experiment> LoadList(string sortOrder, string searchString)
        {
            IQueryable<Experiment> experiments = Db.Experiment;
            experiments = ExperimentLogic.FilterExperiments(experiments, searchString);
            return SortList(experiments.ToList(), sortOrder);
        }

        public List<Experiment> SortList(List<Experiment> experiments, string sortOrder)
        {
            return ExperimentLogic.SortExperiments(experiments, sortOrder);
        }
        public override Experiment Load(int id)
        {
            Experiment experiment = Db.Experiment
                .Where(x => x.Id == id)
                .Include(x => x.MarketingAssetPackage)
                .Include(x=>x.Variant)
                .FirstOrDefault();

            return experiment;
        }

        public List<Variant> GetVariants(int Id)
        {
            return Db.Variant
                     .Where(x => x.ExperimentId == Id)
                     .OrderBy(i => i.VariantName)
                     .ToList();
        }
        public override void Remove(int id)
        {
            Experiment experiment = Db.Experiment
                      .Include(x => x.Variant) 
                      .FirstOrDefault(x => x.Id == id);
            if (experiment != null)
            {
                experiment?.Variant.ToList().ForEach(n => Db.Variant.Remove(n)); 
                Db.Experiment.Remove(experiment);
                Db.SaveChanges();
            }
        }

        public IList<MarketingAssetPackage> GetMAPs()
        {
            IList<MarketingAssetPackage> ep = Db.MarketingAssetPackage
                                              .OrderBy(x => x.Mapname).ToList();
            return ep;
        }
        
    }
}