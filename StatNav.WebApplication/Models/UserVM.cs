using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class UserVM
    {
        public int Id { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public TeamVM Team { get; set; }

        [ForeignKey("UserRole")]
        public int RoleId { get; set; }

        public UserRoleVM UserRole { get; set; }

        public string Username { get; set; }

        public bool Shared { get; set; }

    }
}