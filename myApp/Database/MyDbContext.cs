using Microsoft.EntityFrameworkCore;

namespace myApp;
public class MyDbContext : DbContext
{
    public DbSet<Person> persons { get; set; }

    public MyDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ProjectConstants.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().Property(p => p.personid).IsRequired();
        modelBuilder.Entity<Person>().HasKey(p => p.personid);


    }
}