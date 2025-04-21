using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database.Models;

namespace sylas_api.Database;

public class SyContext(DbContextOptions<SyContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<IssueActivity> IssueActivities { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<GlobalParameter> GlobalParameters { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entity>().UseTptMappingStrategy().ToTable("Entity");
        modelBuilder.Entity<Entity>().HasKey(e => e.Id);
        modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<User>().HasMany(u=>u.Customers).WithMany(c=>c.Members);
        modelBuilder.Entity<User>().HasMany(u=>u.Quests).WithOne(c=>c.Assignee);
        modelBuilder.Entity<Project>().HasOne(u=>u.Owner).WithMany(p=>p.OwningProjects);
        modelBuilder.Entity<Project>().HasMany(p=>p.Issues).WithOne(i=>i.Project);
        modelBuilder.Entity<Project>().HasOne(p=>p.Customer).WithMany(c=>c.Projects);
        modelBuilder.Entity<Project>().HasMany(p=>p.Documents).WithOne(c=>c.Project).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Issue>().HasMany(i=>i.Labels).WithMany(l=>l.Issues);
        modelBuilder.Entity<Issue>().HasOne(m=>m.Milestone).WithMany(i=>i.Issues);
        modelBuilder.Entity<Issue>().HasMany(i=>i.Activities).WithOne(a=>a.Issue).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Issue>().HasMany(i=>i.Quests).WithOne(u=>u.Issue);

        modelBuilder.Entity<Label>().HasMany(l=>l.Issues).WithMany(i=>i.Labels);

        modelBuilder.Entity<Customer>().HasMany(c=>c.Members).WithMany(p=>p.Customers);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
        
        IConfiguration Config = configBuilder.Build();
        string connectionString = Config.GetConnectionString("Default") ?? "";

        string centralConnectionString = Config.GetConnectionString("Default") ?? "";
        optionsBuilder.UseNpgsql(connectionString);
    }
}
