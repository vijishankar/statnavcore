using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class UXElements
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string UXElementName { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual ICollection<UXExperiments> UXExperiments { get; set; }
    }
}
