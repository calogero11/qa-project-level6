using WebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApi.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) 
    : IdentityDbContext(options)
{
    public DbSet<Feed> Feeds { get; set; }
}