using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class User
    {
        public User()
        {
            MarketingAssetPackage = new HashSet<MarketingAssetPackage>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public bool Shared { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<MarketingAssetPackage> MarketingAssetPackage { get; set; }
    }
}
