using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class SitesRepository : GenericRepository<Sites>, ISitesRepository
    {
        public override List<Sites> LoadList(string sortOrder, string searchString)
        {
            IQueryable<Sites> sites = Db.Sites.Include(x => x.MarketingModel);

            sites = SitesLogic.FilterSites(sites, searchString);
            return SortList(sites.ToList(), sortOrder);
        }

        public List<Sites> SortList(List<Sites> SitesList, string sortOrder)
        {
            return SitesLogic.SortSites(SitesList, sortOrder);
        }

        public override Sites Load(int id)
        {
            Sites Sites = Db.Sites.Include(x => x.MarketingModel)
                                  .Include(x => x.Parent)
                                  .Where(x => x.Id == id)
                                  .FirstOrDefault();

            return Sites;
        }
        public override void Remove(int id)
        {
            Sites si = Db.Sites.FirstOrDefault(x => x.Id == id);
            if (si != null)
            {
                Db.Sites.Remove(si);
                Db.SaveChanges();
            }
        }

        public IList<MarketingModel> GetModels()
        {
            IList<MarketingModel> mms = Db.MarketingModel.OrderBy(x => x.ModelName).ToList();
            return mms;
        }

        public IList<Parent> GetParents()
        {
            IList<Parent> ps = Db.Parent.OrderBy(x => x.ParentName).ToList();
            return ps;
        }
    }
}
