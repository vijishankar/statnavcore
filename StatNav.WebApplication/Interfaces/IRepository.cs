using System.Collections.Generic;
using StatNav.WebApplication.DAL;

namespace StatNav.WebApplication.Interfaces
{
    public interface IRepository<T>
    {
        List<T> LoadList(string sortOrder, string searchString);
        T Load(int id);
        void Add(T t);
        void Edit(T t);
        void Remove(int id);

    }

    public interface IPackageContainerRepository : IRepository<PackageContainer>
    {
        IList<MetricModelStage> GetStages();
        List<MarketingAssetPackage> GetMAPs(int Id);
    }
    public interface IMAPRepository : IRepository<MarketingAssetPackage>
    {
        IList<MetricModel> GetMetricModels();
        IList<ExperimentStatus> GetStatuses();
        IList<Method> GetMethods();
        List<MarketingAssetPackage> SortList(List<MarketingAssetPackage> maps, string sortOrder);
        List<Experiment> GetExperiments(int Id);
        IList<PackageContainer> GetPCs();
    }
    public interface IExperimentRepository : IRepository<Experiment>
    {
        IList<MarketingAssetPackage> GetMAPs();
        List<Experiment> SortList(List<Experiment> experiments, string sortOrder);
        List<Variant> GetVariants(int Id);
    }

    public interface IVariantRepository : IRepository<Variant>
    {
        IList<Experiment> GetExperiments();
        IList<MetricModel> GetMetricModels();
        List<Variant> SortList(List<Variant> variants, string sortOrder);
    }

    public interface IGoalsRepository : IRepository<Goals>
    {
        IList<MetricModelStage> GetStages();
        List<Goals> SortList(List<Goals> goals, string sortOrder);
    }

    public interface IMarketingModelRepository : IRepository<MarketingModel>
    {
        List<MarketingModel> SortList(List<MarketingModel> goals, string sortOrder);
    }

    public interface ISitesRepository : IRepository<Sites>
    {
        IList<MarketingModel> GetModels();
        IList<Parent> GetParents();
        List<Sites> SortList(List<Sites> sites, string sortOrder);
    }

    public interface IUXElementsRepository : IRepository<UXElements>
    {
        IList<Parent> GetParents();
        List<UXElements> SortList(List<UXElements> sites, string sortOrder);
        List<UXExperiments> GetUXEles(int Id);

    }

    public interface IUXExperimentsRepository : IRepository<UXExperiments>
    {
        IList<UXElements> GetElements();
        List<UXExperiments> SortList(List<UXExperiments> uxExperiments, string sortOrder);
    }

    public interface IUXVariantsRepository : IRepository<UXVariants>
    {
        IList<UXExperiments> GetExperiments();
        List<UXVariants> SortList(List<UXVariants> uxVariants, string sortOrder);
    }
}
