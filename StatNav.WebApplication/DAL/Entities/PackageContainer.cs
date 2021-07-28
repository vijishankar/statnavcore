using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class PackageContainer
    {
        public PackageContainer()
        {
            MarketingAssetPackage = new HashSet<MarketingAssetPackage>();
        }

        public int Id { get; set; }
        public string PackageContainerName { get; set; }
        public string Type { get; set; }
        public int MetricModelStageId { get; set; }
        public string Notes { get; set; }

        public virtual MetricModelStage MetricModelStage { get; set; }
        public virtual ICollection<MarketingAssetPackage> MarketingAssetPackage { get; set; }
    }
}
