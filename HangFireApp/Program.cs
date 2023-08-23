using Hangfire;
using HangFireApp;
using HangFireApp.Context;
using HangFireApp.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
//sql server kayýt yapabilsin diye konfigürasyon.
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<KullaniciToplamSayiHesapla, HesaplaManager>();


builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
    RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(), Cron.Minutely());//otomatik belirtilen süre için iþlem yapar.Bu method async methotlarýda otomatik olarak çalýþtýrabilme özelliðine sahip.
    RecurringJob.RemoveIfExists(nameof(HesaplaManager));//ayný methodun çoklamasý varsa ise tekini alýr. 
    RecurringJob.AddOrUpdate<HesaplaManager>(nameof(HesaplaManager), j => j.Topla(), Cron.());
    //RecurringJob.AddOrUpdate<HesaplaManager>("hesapla-manager-test", j => j.Topla(), Cron.Minutely()); isim olarak kendimiz verebiliriz

});
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();//görsel bir dasboard çýktýsý veriyor

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
