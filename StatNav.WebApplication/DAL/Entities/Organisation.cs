using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class Organisation
    {
        public Organisation()
        {
            Team = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public bool Shared { get; set; }

        public virtual ICollection<Team> Team { get; set; }
    }
}
