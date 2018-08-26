namespace InteractiveConsultant.Models
{
    //using System;
    using System.Data.Entity;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Linq;

    /*[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]*/
    public class InteractiveConsultantContext : DbContext
    {
        public InteractiveConsultantContext()
            : base("name=InteractiveConsultantContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                        .HasKey(e => e.IDAnswer);
            modelBuilder.Entity<ExtendOfNeed>()
                        .HasKey(e => e.IDExtendOfNeed);
            modelBuilder.Entity<Interview>()
                        .HasKey(i => i.IDInterview)
                        .HasMany(i => i.Answers)
                        .WithMany(a => a.Interviews)
                        .Map(m => m.MapLeftKey("Answers_IDAnswer")
                                   .MapRightKey("Interviews_IDInterview")
                                   .ToTable("Answer_Interview"));
            modelBuilder.Entity<Question>()
                        .HasKey(q => q.IDQuestion);
            modelBuilder.Entity<Area>()
                        .HasKey(a => a.IDArea);
            modelBuilder.Entity<CentralOrganisation>()
                        .HasKey(c => c.IDOrganisation)
                        .HasMany(a => a.Areas)
                        .WithMany(w => w.CentralOrganizations)
                        .Map(mm => mm.MapLeftKey("CentralOrganisations_IDOrganisation")
                                   .MapRightKey("Areas_IDArea")
                                   .ToTable("Area_CentralOrganization"));
            modelBuilder.Entity<CentralOrganisation>()
                        .HasKey(c => c.IDOrganisation)
                        .HasMany(co => co.InerOrganisations)
                        .WithRequired(i => i.CentralOrganisation).Map(m => m.MapKey("CentralOrganisations_IDOrganisation"));

            modelBuilder.Entity<InerOrganisation>()
                        .HasKey(i => i.IDOrganisation);
            modelBuilder.Entity<SocialForm>()
                        .HasKey(s => s.IDSocialForm);
            modelBuilder.Entity<InerOrganisation>()
                        .HasKey(i => i.IDOrganisation)
                        .HasMany(s => s.SocialForms)
                        .WithMany(s => s.InerOrganisations)
                        .Map(m => m.MapLeftKey("InerOrganisations_IDOrganisation")
                                   .MapRightKey("SocialForms_IDSocialForm")
                                   .ToTable("InerOrganisation_SocialForm"));
            modelBuilder.Entity<Condition>().HasKey(c => c.IDCondition);
            modelBuilder.Entity<Result>().HasKey(r => r.IDResult)
                        .HasMany(r => r.Conditions)
                        .WithMany(c => c.Results)
                        .Map(m => m.MapLeftKey("Results_ResultID")
                        .MapRightKey("Conditions_ConditionID")
                        .ToTable("Results_Conditions"));
            modelBuilder.Entity<Recomendation>().HasKey(r => r.ID);
            modelBuilder.Entity<ViewOrganisation>().HasKey(v => v.IDOrganisation);
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExtendOfNeed> TableBartelLouton { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<CentralOrganisation> CentralOrganisations { get; set; }
        public DbSet<InerOrganisation> InerOrganisations { get; set; }
        public DbSet<SocialForm> SocialForms { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Recomendation> Recomendations { get; set; }
        public DbSet<ViewOrganisation> ViewOrganisations { get; set; }
    }
}
