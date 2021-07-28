using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class Experiment
    {
        public Experiment()
        {
            Variant = new HashSet<Variant>();
        }

        public int Id { get; set; }
        public int MarketingAssetPackageId { get; set; }
        public string RequiredDurationForSignificance { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string SuccessOutcome { get; set; }
        public string FailureOutcome { get; set; }
        public string ExperimentName { get; set; }
        public int? ExperimentNumber { get; set; }

        public virtual MarketingAssetPackage MarketingAssetPackage { get; set; }
        public virtual ICollection<Variant> Variant { get; set; }
    }
}
