using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StatNav.WebApplication.BLL;
using StatNav.WebApplication.Interfaces;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public class GoalsRepository : GenericRepository<Goals>, IGoalsRepository
    {
        public override List<Goals> LoadList(string sortOrder, string searchString)
        {
            IQueryable<Goals> goals = Db.Goals.Include(x => x.MetricModelStage);

            goals = GoalsLogic.FilterGoals(goals, searchString);
            return SortList(goals.ToList(), sortOrder);
        }

        public List<Goals> SortList(List<Goals> goalsList, string sortOrder)
        {
            return GoalsLogic.SortGoals(goalsList, sortOrder);
        }

        public override Goals Load(int id)
        {
            Goals goals = Db.Goals.Include(x => x.MetricModelStage)
                                  .Where(x => x.Id == id)
                                  .FirstOrDefault();

            return goals;
        }
        public override void Remove(int id)
        {
            Goals pc = Db.Goals.FirstOrDefault(x => x.Id == id);
            if (pc != null)
            {
                Db.Goals.Remove(pc);
                Db.SaveChanges();
            }
        }

        public IList<MetricModelStage> GetStages()
        {
            IList<MetricModelStage> mms = Db.MetricModelStage.OrderBy(x => x.SortOrder).ToList();
            return mms;
        }
    }
}
