using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class Sites
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string SiteName { get; set; }
        public string Language { get; set; }
        public int ModelId { get; set; }
        public virtual MarketingModel MarketingModel { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
