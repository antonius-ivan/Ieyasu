using Microsoft.EntityFrameworkCore;

namespace AiclaRM.Server.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
