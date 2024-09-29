using daily_dev.Models;
using Microsoft.EntityFrameworkCore;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }

    public DbSet<Fact_News> Fact_News { get; set; }
    public DbSet<Dim_Source> Dim_Source { get; set; }
    public DbSet<Dim_Tag> Dim_Tag { get; set; }
    public DbSet<Dim_User> Dim_User { get; set; }
    public DbSet<Fact_Article_Interaction> Fact_Article_Interactions { get; set; }
    public DbSet<Fact_Bookmark> Fact_Bookmarks { get; set; }
    public DbSet<Fact_History> Fact_Histories { get; set; }
    public DbSet<Fact_Comments> Fact_Comments { get; set; }
    public DbSet<Dim_Category> Dim_Categories { get; set; }
    public DbSet<Dim_Date> Dim_Date { get; set; }
}


