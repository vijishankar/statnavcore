using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class MetricModel
    {
        public MetricModel()
        {
            MetricVariantResult = new HashSet<MetricVariantResult>();
            VariantVariantImpactMetricModel = new HashSet<Variant>();
            VariantVariantTargetMetricModel = new HashSet<Variant>();
        }

        public int Id { get; set; }
        public int MetricModelStageId { get; set; }
        public string Title { get; set; }
        public bool GoodIsUp { get; set; }

        public virtual MetricModelStage MetricModelStage { get; set; }
        public virtual ICollection<MetricVariantResult> MetricVariantResult { get; set; }
        public virtual ICollection<Variant> VariantVariantImpactMetricModel { get; set; }
        public virtual ICollection<Variant> VariantVariantTargetMetricModel { get; set; }
    }
}
