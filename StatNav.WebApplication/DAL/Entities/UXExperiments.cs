using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.DAL
{
    public partial class UXExperiments
    {
        public int Id { get; set; }
        public int UXElementId { get; set; }
        public string UXExperimentName { get; set; }
        public string GoogleExperimentId { get; set; }

        public UXElements UXElements { get; set; }
        public ICollection<UXVariants> UXVariants { get; set; }

    }
}
