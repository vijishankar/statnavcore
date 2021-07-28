using AutoMapper;
using StatNav.WebApplication.DAL;
using StatNav.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //  DB Entities into View models
            CreateMap<Experiment, ExperimentVM>();
            CreateMap<ExperimentStatus, ExperimentStatusVM>();
            CreateMap<MarketingAssetPackage, MarketingAssetPackageVM>();
            CreateMap<Method, MethodVM>();
            CreateMap<MetricModel, MetricModelVM>();
            CreateMap<MetricModelStage, MetricModelStageVM>();
            CreateMap<MetricVariantResult, MetricVariantResultVM>();
            CreateMap<Organisation, OrganisationVM>();
            CreateMap<PackageContainer, PackageContainerVM>();
            CreateMap<Team, TeamVM>();
            CreateMap<User, UserVM>();
            CreateMap<UserRole, UserRoleVM>();
            CreateMap<Variant, VariantVM>();
            CreateMap<Goals, GoalsVM>();
            //CreateMap<MarketingModel, MarketingModelVM>();
            CreateMap<Sites, SitesVM>();
            CreateMap<UXElements, UXElementsVM>();
            CreateMap<UXExperiments, UXExperimentsVM>();
            CreateMap<UXVariants, UXVariantsVM>();

            // .ForMember(dest => dest.Experiment, opt => opt.Ignore());

            // View models into DB Entities            
            CreateMap<ExperimentVM, Experiment>();
            //.ForMember(dest => dest.VariantVM, opt => opt.Ignore());

            CreateMap<ExperimentStatusVM, ExperimentStatus>();
            CreateMap<MarketingAssetPackageVM, MarketingAssetPackage>();
            CreateMap<MethodVM, Method>();
            CreateMap<MetricModelVM, MetricModel>();
            CreateMap<MetricModelStageVM, MetricModelStage>();
            CreateMap<MetricVariantResultVM, MetricVariantResult>();
            CreateMap<OrganisationVM, Organisation>();
            CreateMap<PackageContainerVM, PackageContainer>();
            CreateMap<GoalsVM, Goals>();
            CreateMap<TeamVM, Team>();
            CreateMap<UserVM, User>();
            CreateMap<UserRoleVM, UserRole>();
            CreateMap<VariantVM, Variant>();
            //CreateMap<MarketingModelVM, MarketingModel>();
            CreateMap<SitesVM, Sites>();
            CreateMap<UXElementsVM, UXElements>();
            CreateMap<UXExperimentsVM, UXExperiments>();
            CreateMap<UXVariantsVM, UXVariants>();
        }
    }
}
