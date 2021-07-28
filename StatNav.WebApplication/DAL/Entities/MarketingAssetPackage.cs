using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class MarketingAssetPackage
    {
        public MarketingAssetPackage()
        {
            Experiment = new HashSet<Experiment>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public string Mapname { get; set; }
        public string Problem { get; set; }
        public string ProblemValidation { get; set; }
        public string Hypothesis { get; set; }
        public string Notes { get; set; }
        public int PackageContainerId { get; set; }

        public virtual PackageContainer PackageContainer { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Experiment> Experiment { get; set; }
    }
}
