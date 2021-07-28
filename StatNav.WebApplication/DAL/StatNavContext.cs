using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StatNav.WebApplication.Models;

namespace StatNav.WebApplication.DAL
{
    public partial class StatNavContext : DbContext
    {
        private readonly string dbConnStr = "Data Source=" + AppConfigValues.DataSource + ";Initial Catalog=" + AppConfigValues.InitialCatalog + ";User ID=" + AppConfigValues.UserId+";Password=" + AppConfigValues.Password+";";
        public StatNavContext()
        {
        }

        public StatNavContext(DbContextOptions<StatNavContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Experiment> Experiment { get; set; }
        public virtual DbSet<ExperimentStatus> ExperimentStatus { get; set; }
        public virtual DbSet<MarketingAssetPackage> MarketingAssetPackage { get; set; }
        public virtual DbSet<Method> Method { get; set; }
        public virtual DbSet<MetricModel> MetricModel { get; set; }
        public virtual DbSet<MetricModelStage> MetricModelStage { get; set; }
        public virtual DbSet<MetricVariantResult> MetricVariantResult { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<PackageContainer> PackageContainer { get; set; }
        public virtual DbSet<Goals> Goals { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Variant> Variant { get; set; }
        public virtual DbSet<MarketingModel> MarketingModel { get; set; }
        public virtual DbSet<Sites> Sites { get; set; }
        public virtual DbSet<Parent> Parent { get; set; }
        public virtual DbSet<UXElements> UXElements { get; set; }
        public virtual DbSet<UXExperiments> UXExperiments { get; set; }
        public virtual DbSet<UXVariants> UXVariants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(dbConnStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Experiment>(entity =>
            {
                entity.HasIndex(e => e.MarketingAssetPackageId)
                    .HasName("IX_MarketingAssetPackageId");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExperimentName)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.MarketingAssetPackage)
                    .WithMany(p => p.Experiment)
                    .HasForeignKey(d => d.MarketingAssetPackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ExperimentIteration_dbo.MarketingAssetPackage_MarketingAssetPackageId");
            });

            modelBuilder.Entity<MarketingAssetPackage>(entity =>
            {
                entity.HasIndex(e => e.PackageContainerId)
                    .HasName("IX_PackageContainerId");

                entity.HasIndex(e => e.TeamId)
                    .HasName("IX_TeamId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserId");

                entity.Property(e => e.Mapname)
                    .IsRequired()
                    .HasColumnName("MAPName");

                entity.HasOne(d => d.PackageContainer)
                    .WithMany(p => p.MarketingAssetPackage)
                    .HasForeignKey(d => d.PackageContainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.MarketingAssetPackage_dbo.PackageContainer_PackageContainerId");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.MarketingAssetPackage)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.MarketingAssetPackage_dbo.Team_TeamId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MarketingAssetPackage)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.MarketingAssetPackage_dbo.User_UserId");
            });

            modelBuilder.Entity<MetricModel>(entity =>
            {
                entity.HasIndex(e => e.MetricModelStageId)
                    .HasName("IX_MetricModelStageId");

                entity.HasOne(d => d.MetricModelStage)
                    .WithMany(p => p.MetricModel)
                    .HasForeignKey(d => d.MetricModelStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.MetricModel_dbo.MetricModelStage_MetricModelStageId");
            });

            modelBuilder.Entity<MetricVariantResult>(entity =>
            {
                entity.HasIndex(e => e.MetricId)
                    .HasName("IX_MetricId");

                entity.HasIndex(e => e.VariantId)
                    .HasName("IX_VariantId");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.MetricVariantResult)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.MetricVariantResult_dbo.MetricModel_MetricId");

                entity.HasOne(d => d.Variant)
                    .WithMany(p => p.MetricVariantResult)
                    .HasForeignKey(d => d.VariantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.MetricVariantResult_dbo.Variant_VariantId");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<PackageContainer>(entity =>
            {
                entity.HasIndex(e => e.MetricModelStageId)
                    .HasName("IX_MetricModelStageId");

                entity.Property(e => e.PackageContainerName).IsRequired();

                entity.Property(e => e.Type).IsRequired();

                entity.HasOne(d => d.MetricModelStage)
                    .WithMany(p => p.PackageContainer)
                    .HasForeignKey(d => d.MetricModelStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.PackageContainer_dbo.MetricModelStage_MetricModelStageId");
            });

            modelBuilder.Entity<Goals>(entity =>
            {
                entity.HasIndex(e => e.MetricModelStageId)
                    .HasName("IX_MetricModelStageId");

                entity.Property(e => e.GoalName).IsRequired();

                entity.HasOne(d => d.MetricModelStage)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.MetricModelStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Goals_dbo.MetricModelStage_MetricModelStageId");
            });

            modelBuilder.Entity<Sites>(entity =>
            {
                entity.HasIndex(e => e.ModelId)
                    .HasName("IX_ModelId");

                entity.Property(e => e.SiteName).IsRequired();

                entity.HasOne(d => d.MarketingModel)
                    .WithMany(p => p.Sites)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("[FK_dbo.Sites_dbo.Model_ModelId]");
            });

            modelBuilder.Entity<UXElements>(entity =>
            {
                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_ParentId");

                entity.Property(e => e.UXElementName).IsRequired();

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.UXElements)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                  //  .HasConstraintName("[FK_dbo.Sites_dbo.Model_ModelId]");
            });

            modelBuilder.Entity<UXExperiments>(entity =>
            {
                entity.HasIndex(e => e.UXElementId)
                    .HasName("IX_UXElementId");

                entity.Property(e => e.UXExperimentName).IsRequired();

                entity.HasOne(d => d.UXElements)
                    .WithMany(p => p.UXExperiments)
                    .HasForeignKey(d => d.UXElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("[FK_dbo.UXExperiments_dbo.UXElement_UXElementId]");
            });

            modelBuilder.Entity<UXVariants>(entity =>
            {
                entity.HasIndex(e => e.UXExperimentId)
                    .HasName("IX_UXElementId");

                entity.Property(e => e.UXVariantName).IsRequired();

                entity.HasOne(d => d.UXExperiments)
                    .WithMany(p => p.UXVariants)
                    .HasForeignKey(d => d.UXExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("[FK_dbo.UXExperiments_dbo.UXExperiments_UXExperimentId]");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.OrganisationId)
                    .HasName("IX_OrganisationId");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Team_dbo.Organisation_OrganisationId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_RoleId");

                entity.HasIndex(e => e.TeamId)
                    .HasName("IX_TeamId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.User_dbo.UserRole_RoleId");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.User_dbo.Team_TeamId");
            });

            modelBuilder.Entity<Variant>(entity =>
            {
                entity.HasIndex(e => e.ExperimentId)
                    .HasName("IX_ExperimentId");

                entity.HasIndex(e => e.VariantImpactMetricModelId)
                    .HasName("IX_VariantImpactMetricModelId");

                entity.HasIndex(e => e.VariantTargetMetricModelId)
                    .HasName("IX_VariantTargetMetricModelId");

                entity.Property(e => e.VariantName).IsRequired();

                entity.HasOne(d => d.Experiment)
                    .WithMany(p => p.Variant)
                    .HasForeignKey(d => d.ExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Variant_dbo.Experiment_ExperimentId");

                entity.HasOne(d => d.VariantImpactMetricModel)
                    .WithMany(p => p.VariantVariantImpactMetricModel)
                    .HasForeignKey(d => d.VariantImpactMetricModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Variant_dbo.MetricModel_VariantImpactMetricModelId");

                entity.HasOne(d => d.VariantTargetMetricModel)
                    .WithMany(p => p.VariantVariantTargetMetricModel)
                    .HasForeignKey(d => d.VariantTargetMetricModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Variant_dbo.MetricModel_VariantTargetMetricModelId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
