using Microsoft.EntityFrameworkCore;

namespace RESTApi.Models
{
    public class ArtDBContext : DbContext
    {
        public ArtDBContext(DbContextOptions<ArtDBContext> options)
            : base(options)
            {
            }

        public DbSet<Artikel> Artikels { get; set; }
    }
}