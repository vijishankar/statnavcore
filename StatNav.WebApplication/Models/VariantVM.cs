using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class VariantVM
    {
        public int Id { get; set; }
        [Display(Name = "Experiment")]
        public int ExperimentId { get; set; }

        public ExperimentVM Experiment{ get; set; }

        [Display(Name = "Variant Name")]
        [Required]
        public string VariantName { get; set; }

        public bool Control { get; set; }

        
        [Display(Name = "Target Metric")]
        public int VariantTargetMetricModelId { get; set; }

        public MetricModelVM VariantTargetMetricModel { get; set; }

        [Display(Name = "Target Met")]
        public bool TargetMet { get; set; }

        [ForeignKey("VariantImpactMetricModel")]
        [Display(Name = "Impact Metric")]
        public int VariantImpactMetricModelId { get; set; }

        public MetricModelVM VariantImpactMetricModel { get; set; }

        [Display(Name = "Impact Met")]
        public bool ImpactMet { get; set; }

        
    }
}