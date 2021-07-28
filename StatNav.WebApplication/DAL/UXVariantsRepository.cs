using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class UXVariantsRepository : GenericRepository<UXVariants>, IUXVariantsRepository
    {
        public override List<UXVariants> LoadList(string sortOrder, string searchString)
        {
            IQueryable<UXVariants> uxVariants = Db.UXVariants.Include(x => x.UXExperiments);

            uxVariants = UXVariantsLogic.FilterVariants(uxVariants, searchString);
            return SortList(uxVariants.ToList(), sortOrder);
        }

        public List<UXVariants> SortList(List<UXVariants> UXVariantsList, string sortOrder)
        {
            return UXVariantsLogic.SortVariants(UXVariantsList, sortOrder);
        }

        public override UXVariants Load(int id)
        {
            UXVariants UXVariants = Db.UXVariants.Where(x => x.Id == id)
                                  .Include(x => x.UXExperiments)
                                  .FirstOrDefault();

            return UXVariants;
        }
        public override void Remove(int id)
        {
            UXVariants pc = Db.UXVariants.FirstOrDefault(x => x.Id == id);
            if (pc != null)
            {
                Db.UXVariants.Remove(pc);
                Db.SaveChanges();
            }
        }

        public IList<UXExperiments> GetExperiments()
        {
            IList<UXExperiments> uxe = Db.UXExperiments.OrderBy(x => x.UXExperimentName).ToList();
            return uxe;
        }
    }
}
