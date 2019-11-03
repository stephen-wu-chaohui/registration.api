using Microsoft.EntityFrameworkCore;

namespace DocuSQL.API.Models
{
    public class DocuSQLContext : DbContext
    {
        public DocuSQLContext(DbContextOptions<DocuSQLContext> options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().Property(b => b.SubmissionTime).HasDefaultValueSql("GETUTCDATE()");
        }

        public DbSet<Document> Document { get; set; }

        public DbSet<Field> Field { get; set; }
    }
}
