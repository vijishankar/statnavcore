using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class PackageContainerRepository : GenericRepository<PackageContainer>, IPackageContainerRepository
    {

        public override List<PackageContainer> LoadList(string sortOrder, string searchString)
        {
            IQueryable<PackageContainer> containers = Db.PackageContainer
                               .Include(x => x.MetricModelStage);


            containers = PCLogic.FilterPCs(containers, searchString);
            return SortList(containers.ToList(), sortOrder);
        }       

        public List<PackageContainer> SortList(List<PackageContainer> containers, string sortOrder)
        {
            return PCLogic.SortPCs(containers, sortOrder);
        }

        public override PackageContainer Load(int id)
        {
            PackageContainer container = Db.PackageContainer
                                              .Include(x => x.MarketingAssetPackage)
                                              .Include(x => x.MetricModelStage)
                                              .Where(x => x.Id == id)
                                              .FirstOrDefault();

            return container;
        }
        public override void Remove(int id)
        {
            PackageContainer pc = Db.PackageContainer
                      .Include(p => p.MarketingAssetPackage).ThenInclude(x => x.Experiment).ThenInclude(x => x.Variant)
                      .FirstOrDefault(x => x.Id == id);
            if (pc != null)
            {
                //remove variants
                pc?.MarketingAssetPackage.ToList().ForEach(i => i.Experiment.ToList().ForEach(v => v.Variant.ToList().ForEach(n => Db.Variant.Remove(n))));
                //remove experiments
                pc?.MarketingAssetPackage.ToList().ForEach(i => i.Experiment.ToList().ForEach(n => Db.Experiment.Remove(n)));
                //remove marketing asset packages
                pc?.MarketingAssetPackage.ToList().ForEach(n => Db.MarketingAssetPackage.Remove(n));
                Db.PackageContainer.Remove(pc);
                Db.SaveChanges();
            }
        }

        public IList<MetricModelStage> GetStages()
        {
            IList<MetricModelStage> mms = Db.MetricModelStage
                                            .OrderBy(x => x.SortOrder).ToList();
            return mms;
        }

        public List<MarketingAssetPackage> GetMAPs(int Id)
        {
            return Db.MarketingAssetPackage
                    .Where(x => x.PackageContainerId == Id)
                    .OrderBy(i => i.Mapname)
                    .ToList();
        }
    }
}