using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class UXExperimentsRepository : GenericRepository<UXExperiments>, IUXExperimentsRepository
    {
        public override List<UXExperiments> LoadList(string sortOrder, string searchString)
        {
            IQueryable<UXExperiments> uxExperiments = Db.UXExperiments.Include(x => x.UXElements);

            uxExperiments = UXExperimentsLogic.FilterExperiments(uxExperiments, searchString);
            return SortList(uxExperiments.ToList(), sortOrder);
        }

        public List<UXExperiments> SortList(List<UXExperiments> UXExperimentsList, string sortOrder)
        {
            return UXExperimentsLogic.SortExperiments(UXExperimentsList, sortOrder);
        }

        public override UXExperiments Load(int id)
        {
            UXExperiments UXExperiments = Db.UXExperiments.Where(x => x.Id == id)
                                  .Include(x => x.UXElements)
                                  .FirstOrDefault();

            return UXExperiments;
        }
        public override void Remove(int id)
        {
            UXExperiments uxe = Db.UXExperiments
                .Include(x => x.UXVariants)
                .FirstOrDefault(x => x.Id == id);
            if (uxe != null)
            {
                uxe?.UXVariants.ToList().ForEach(n => Db.UXVariants.Remove(n));
                Db.UXExperiments.Remove(uxe);
                Db.SaveChanges();
            }
        }

        public IList<UXElements> GetElements()
        {
            IList<UXElements> uxe = Db.UXElements.OrderBy(x => x.UXElementName).ToList();
            return uxe;
        }
    }
}
