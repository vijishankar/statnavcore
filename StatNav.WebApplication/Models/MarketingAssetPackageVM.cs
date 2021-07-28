using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatNav.WebApplication.Models
{
    public class MarketingAssetPackageVM
    {
        public MarketingAssetPackageVM()
        {
            Experiment = new List<ExperimentVM>();
        }
        public int Id { get; set; }

        [ForeignKey("User")]
        [Display(Name = "User")]
        public int? UserId { get; set; }

        public UserVM User { get; set; }

        [ForeignKey("Team")]
        [Display(Name = "Team")]
        public int? TeamId { get; set; }

        public TeamVM Team { get; set; }

        [Display(Name = "Package Name")]
        [Required]
        public string MAPName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Problem { get; set; }

        [Display(Name = "Problem Validation")]
        [DataType(DataType.MultilineText)]
        public string ProblemValidation { get; set; }

        [DataType(DataType.MultilineText)]
        public string Hypothesis { get; set; }

        //STORY 2069 attributes removed from UI        

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [ForeignKey("MarketingAssetPackageId")]
        public ICollection<ExperimentVM> Experiment { get; set; }

        [ForeignKey("PackageContainer")]
        [Display(Name = "Container")]
        public int PackageContainerId { get; set; }

        public PackageContainerVM PackageContainer { get; set; }
    }
}