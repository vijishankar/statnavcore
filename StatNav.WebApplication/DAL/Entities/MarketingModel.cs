using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class MarketingModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public virtual ICollection<Sites> Sites { get; set; }
    }
}
