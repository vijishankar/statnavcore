using StatNav.WebApplication.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class UXExperimentsVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "UX Element")]
        public int UXElementId { get; set; }

        [Required]
        [Display(Name = "UX Experiment Name")]
        public string UXExperimentName { get; set; }

        [Display(Name = "Google Experiment")]
        public string GoogleExperimentId { get; set; }

        public UXElements UXElements { get; set; }
    }
}
