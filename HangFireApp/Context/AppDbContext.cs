using Microsoft.EntityFrameworkCore;

namespace HangFireApp.Context
{
    public  class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Kullanici> Kullanicis { get; set; }
        public DbSet<KullaniciSayisi> KullaniciSayisis { get; set; }
    }
}
