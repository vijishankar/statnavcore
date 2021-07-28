using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class GoalsVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Goal Name")]
        public string GoalName { get; set; }

        [Display(Name = "Stage")]
        public int MetricModelStageId { get; set; }

        public MetricModelStageVM MetricModelStage { get; set; }

        [Display(Name = "GA Goal")]
        public string GAGoal { get; set; }
    }
}
