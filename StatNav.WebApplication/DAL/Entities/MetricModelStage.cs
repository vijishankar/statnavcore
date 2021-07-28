using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class MetricModelStage
    {
        public MetricModelStage()
        {
            MetricModel = new HashSet<MetricModel>();
            PackageContainer = new HashSet<PackageContainer>();
            Goals = new HashSet<Goals>();
        }

        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public int DataType { get; set; }
        public int MarketingModelId { get; set; }

        public virtual ICollection<MetricModel> MetricModel { get; set; }
        public virtual ICollection<PackageContainer> PackageContainer { get; set; }
        public virtual ICollection<Goals> Goals { get; set; }
    }
}
