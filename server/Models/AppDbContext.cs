using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
  public class AppDbContext : IdentityDbContext
  {
    // base(options) calls the parent constructor, equivalent of super(...) in Java
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    // everything goes through the EF DbContext class; therefore models should be added here
    public DbSet<AppUser> AppUsers { get; set; }
  }
}