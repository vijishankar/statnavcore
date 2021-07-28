using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class MetricVariantResultVM
    {
        public int Id { get; set; }

        [ForeignKey("MetricModel")]
        public int MetricId { get; set; }

        public MetricModelVM MetricModel { get; set; }

        [ForeignKey("Variant")]
        public int  VariantId { get; set; }

        public VariantVM Variant { get; set; }

        public float Value { get; set; }

        public float SampleSize { get; set; }

    }
}