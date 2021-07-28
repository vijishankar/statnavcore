using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class Variant
    {
        public Variant()
        {
            MetricVariantResult = new HashSet<MetricVariantResult>();
        }

        public int Id { get; set; }
        public int ExperimentId { get; set; }
        public string VariantName { get; set; }
        public bool Control { get; set; }
        public int VariantTargetMetricModelId { get; set; }
        public bool TargetMet { get; set; }
        public int VariantImpactMetricModelId { get; set; }
        public bool ImpactMet { get; set; }

        public virtual Experiment Experiment { get; set; }
        public virtual MetricModel VariantImpactMetricModel { get; set; }
        public virtual MetricModel VariantTargetMetricModel { get; set; }
        public virtual ICollection<MetricVariantResult> MetricVariantResult { get; set; }
    }
}
