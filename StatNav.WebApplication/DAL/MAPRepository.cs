using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class MAPRepository : GenericRepository<MarketingAssetPackage>, IMAPRepository
    {

        public override List<MarketingAssetPackage> LoadList(string sortOrder, string searchString)
        {
            IQueryable<MarketingAssetPackage> maps = Db.MarketingAssetPackage;
                               //.Include(x => x.ExperimentStatus); //story 2069

            maps = MAPLogic.FilterMAPs(maps, searchString);
            return SortList(maps.ToList(), sortOrder);
        }

        public List<Experiment> GetExperiments(int Id)
        {            
            return Db.Experiment
                     .Where(x => x.MarketingAssetPackageId == Id)
                     .OrderBy(i => i.ExperimentName)
                     .ToList();
        }

        public List<MarketingAssetPackage> SortList(List<MarketingAssetPackage> maps, string sortOrder)
        {
            return MAPLogic.SortMAPs(maps, sortOrder);
        }

        public override MarketingAssetPackage Load(int id)
        {
            MarketingAssetPackage map = Db.MarketingAssetPackage
                                              .Where(x => x.Id == id)
                                              //.Include(x => x.ExperimentStatus) story 2069
                                              //.Include(x => x.MAPTargetMetricModel)
                                              //.Include(x => x.MAPImpactMetricModel)
                                              .Include(x => x.Experiment)
                                              //.Include(x=>x.MAPMethod)
                                              .Include(x=>x.PackageContainer)
                                              .FirstOrDefault();

            return map;
        }
        public override void Remove(int id)
        {
            MarketingAssetPackage map = Db.MarketingAssetPackage
                      .Include(x => x.Experiment).ThenInclude(x => x.Variant)
                      .FirstOrDefault(x => x.Id == id);
            if (map != null)
            {
                map?.Experiment.ToList().ForEach(v => v.Variant.ToList().ForEach(n => Db.Variant.Remove(n)));
                map?.Experiment.ToList().ForEach(n => Db.Experiment.Remove(n));
                Db.MarketingAssetPackage.Remove(map);
                Db.SaveChanges();
            }
        }

        public IList<MetricModel> GetMetricModels()
        {
            IList<MetricModel> mm = Db.MetricModel
              .OrderBy(x => x.Title).ToList();
            return mm;
        }
        public IList<ExperimentStatus> GetStatuses()
        {
            IList<ExperimentStatus> es = Db.ExperimentStatus
            .OrderBy(x => x.DisplayOrder).ToList();
            return es;
        }

        public IList<Method> GetMethods()
        {
            IList<Method> m = Db.Method
            .OrderBy(x => x.SortOrder).ToList();
            return m;
        }

        public IList<PackageContainer> GetPCs()
        {
            IList<PackageContainer> pcs = Db.PackageContainer
                                               .OrderBy(x => x.PackageContainerName).ToList();
            return pcs;
        }
    }
}