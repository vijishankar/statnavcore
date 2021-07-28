using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class MetricVariantResult
    {
        public int Id { get; set; }
        public int MetricId { get; set; }
        public int VariantId { get; set; }
        public float Value { get; set; }
        public float SampleSize { get; set; }

        public virtual MetricModel Metric { get; set; }
        public virtual Variant Variant { get; set; }
    }
}
