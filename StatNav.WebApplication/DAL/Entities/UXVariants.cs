using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class UXVariants
    {
        public int Id { get; set; }
        public int UXExperimentId { get; set; }
        public string UXVariantName { get; set; }
        public string VarientURL { get; set; }

        public virtual UXExperiments UXExperiments { get; set; }
    }
}
