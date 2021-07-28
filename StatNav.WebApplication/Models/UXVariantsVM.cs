using StatNav.WebApplication.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class UXVariantsVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "UX Experiment")]
        public int UXExperimentId { get; set; }

        [Required]
        [Display(Name = "UX Variant Name")]
        public string UXVariantName { get; set; }

        [Display(Name = "Variant URL")]
        public string VarientURL { get; set; }

        public UXExperiments UXExperiments { get; set; }
    }
}
