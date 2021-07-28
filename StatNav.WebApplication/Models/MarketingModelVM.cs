using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class MarketingModelVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }
    }
}
