
namespace StatNav.WebApplication.Models
{
    public class MetricModelStageVM
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public int DataType { get; set; }
        public int MarketingModelId { get; set; } // to be linked up later?
    }
}