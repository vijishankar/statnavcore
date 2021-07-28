using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class Parent
    {
        public int Id { get; set; }
        public string ParentName { get; set; }

        public virtual ICollection<UXElements> UXElements { get; set; }
        public virtual ICollection<Sites> Sites { get; set; }
    }
}
