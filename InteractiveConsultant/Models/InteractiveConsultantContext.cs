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
            modelBuilder.Entity<Answer>().HasKey(e => e.IDAnswer);
            modelBuilder.Entity<ExtendOfNeed>().HasKey(e => e.IDExtendOfNeed);
            modelBuilder.Entity<Interview>().HasKey(i => i.IDInterview)
                                            .HasMany(i => i.Answers)
                                            .WithMany(a => a.Interviews)
                                            .Map(m => m.MapLeftKey("IDAnswer").MapRightKey("IDInterview").ToTable("InterviewsAnswers"));
            modelBuilder.Entity<Question>().HasKey(q => q.IDQuestion);
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExtendOfNeed> TableBartelLouton { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
