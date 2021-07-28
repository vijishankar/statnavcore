using StatNav.WebApplication.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class SitesVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Parent")]
        public int ParentId { get; set; }

        [Required]
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        public string Language { get; set; }

        [Display(Name = "Model")]
        public int ModelId { get; set; }

        public MarketingModel MarketingModel { get; set; }

        public Parent Parent { get; set; }

    }
}
