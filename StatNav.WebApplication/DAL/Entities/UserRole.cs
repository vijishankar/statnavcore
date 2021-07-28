using System;
using System.Collections.Generic;

namespace StatNav.WebApplication.DAL
{
    public partial class UserRole
    {
        public UserRole()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool ReadTeamProgrammes { get; set; }
        public bool EditTeamProgrammes { get; set; }
        public bool ReadOrganisationProgrammes { get; set; }
        public bool EditOrganisationProgrammes { get; set; }
        public bool Administrator { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
