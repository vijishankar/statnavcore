using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class ExperimentVM
    {
        public ExperimentVM()
        {
            Variant = new List<VariantVM>();
        }
        public int Id { get; set; }

        
        [Display(Name = "Marketing Asset Package")]
        public int MarketingAssetPackageId { get; set; }

        public MarketingAssetPackageVM MarketingAssetPackage { get; set; }

        [Display(Name = "Experiment Name")]
        [Required]
        public string ExperimentName { get; set; }

        [Display(Name = "Required Duration For Significance")]
        public string RequiredDurationForSignificance { get; set; }

        [Display(Name = "Experiment Number")]
        public int? ExperimentNumber { get; set; }

        [DataType(DataType.Date), UIHint("DatePicker"), Display(Name = "Start Date")]
        public DateTime? StartDateTime { get; set; }

        [DataType(DataType.Date), UIHint("DatePicker"), Display(Name = "End Date")]
        public DateTime? EndDateTime { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Success Outcome")]
        public string SuccessOutcome { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Failure Outcome")]
        public string FailureOutcome { get; set; }        
        public ICollection<VariantVM> Variant { get; set; }
    }
}
