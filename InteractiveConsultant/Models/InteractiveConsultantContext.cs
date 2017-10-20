namespace InteractiveConsultant.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InteractiveConsultantContext : DbContext
    {
        public InteractiveConsultantContext()
            : base("name=InteractiveConsultantContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>().HasKey(e => e.IDAnswer);
            modelBuilder.Entity<ExtendOfNeed>().HasKey(e => e.IDExtendOfNeed);
            modelBuilder.Entity<Interview>().HasKey(i => i.IDInterview);
            modelBuilder.Entity<Question>().HasKey(q => q.IDQuestion);
            modelBuilder.Entity<Result>().HasKey(t => t.IDResult);
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExtendOfNeed> TableBartelLouton { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}
