using HangFireApp.Context;

namespace HangFireApp
{
    public class HesaplaManager : KullaniciToplamSayiHesapla
    {
        private readonly AppDbContext _context;

        public HesaplaManager(AppDbContext context)
        {
            _context = context;
        }

        public void Topla()
        {
            var kullanici = _context.Kullanicis.Count();
            _context.KullaniciSayisis.Add(new KullaniciSayisi
            {
                ToplamSayi = kullanici,
            });
            _context.SaveChanges();
        }
    }
}
