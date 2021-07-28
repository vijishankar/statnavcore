using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class Team
    {
        public Team()
        {
            MarketingAssetPackage = new HashSet<MarketingAssetPackage>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string TeamName { get; set; }
        public bool Shared { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<MarketingAssetPackage> MarketingAssetPackage { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
