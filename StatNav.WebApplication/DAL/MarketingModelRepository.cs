using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class MarketingModelRepository : GenericRepository<MarketingModel>, IMarketingModelRepository
    {
        public override List<MarketingModel> LoadList(string sortOrder, string searchString)
        {
            IQueryable<MarketingModel> marketinModel = Db.MarketingModel;

            marketinModel = MarketingModelLogic.FilterModels(marketinModel, searchString);
            return SortList(marketinModel.ToList(), sortOrder);
        }

        public List<MarketingModel> SortList(List<MarketingModel> mmList, string sortOrder)
        {
            return MarketingModelLogic.SortModels(mmList, sortOrder);
        }

        public override MarketingModel Load(int id)
        {
            MarketingModel container = Db.MarketingModel.Where(x => x.Id == id).FirstOrDefault();

            return container;
        }

        public override void Remove(int id)
        {
            MarketingModel pc = Db.MarketingModel.FirstOrDefault(x => x.Id == id);
            if (pc != null)
            {
                Db.MarketingModel.Remove(pc);
                Db.SaveChanges();
            }
        }
    }
}
