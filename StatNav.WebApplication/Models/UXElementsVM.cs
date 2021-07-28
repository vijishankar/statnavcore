using StatNav.WebApplication.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class UXElementsVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Parent")]
        public int ParentId { get; set; }

        [Required]
        [Display(Name = "UX Element Name")]
        public string UXElementName { get; set; }
        public Parent Parent { get; set; }

        [ForeignKey("UXElementsId")]
        public ICollection<UXExperimentsVM> UXExperiments { get; set; }

    }
}
