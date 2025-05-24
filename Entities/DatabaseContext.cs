using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace webapi.Entities;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : IdentityDbContext(options);