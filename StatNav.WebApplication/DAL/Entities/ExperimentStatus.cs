using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class ExperimentStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
