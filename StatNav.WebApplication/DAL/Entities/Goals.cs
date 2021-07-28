using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class Goals
    {
        public int Id { get; set; }
        public string GoalName { get; set; }
        public int MetricModelStageId { get; set; }
        public string GAGoal { get; set; }

        public virtual MetricModelStage MetricModelStage { get; set; }
    }
}
