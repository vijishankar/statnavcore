using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class UXElementsRepository : GenericRepository<UXElements>, IUXElementsRepository
    {
        public override List<UXElements> LoadList(string sortOrder, string searchString)
        {
            IQueryable<UXElements> UXElements = Db.UXElements;

            UXElements = UXElementsLogic.FilterUXElements(UXElements, searchString);
            return SortList(UXElements.ToList(), sortOrder);
        }

        public List<UXElements> SortList(List<UXElements> UXElementsList, string sortOrder)
        {
            return UXElementsLogic.SortUXElements(UXElementsList, sortOrder);
        }

        public override UXElements Load(int id)
        {
            UXElements UXElements = Db.UXElements
                                        .Include(x => x.UXExperiments)
                                        .Include(x => x.Parent)
                                        .Where(x => x.Id == id)
                                        .FirstOrDefault();

            return UXElements;
        }
        public override void Remove(int id)
        {
            UXElements uxe = Db.UXElements
                .Include(x => x.UXExperiments).ThenInclude(x => x.UXVariants)
                .FirstOrDefault(x => x.Id == id);
            if (uxe != null)
            {
                uxe?.UXExperiments.ToList().ForEach(i => i.UXVariants.ToList().ForEach(n => Db.UXVariants.Remove(n)));
                //remove marketing asset packages
                uxe?.UXExperiments.ToList().ForEach(n => Db.UXExperiments.Remove(n));

                Db.UXElements.Remove(uxe);
                Db.SaveChanges();
            }
        }

        public IList<Parent> GetParents()
        {
            IList<Parent> ps = Db.Parent.OrderBy(x => x.ParentName).ToList();
            return ps;
        }

        public List<UXExperiments> GetUXEles(int Id)
        {
            return Db.UXExperiments
                    .Where(x => x.UXElementId == Id)
                    .OrderBy(i => i.UXExperimentName)
                    .ToList();
        }
    }
}
