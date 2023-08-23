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
//sql server kay�t yapabilsin diye konfig�rasyon.
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<KullaniciToplamSayiHesapla, HesaplaManager>();


builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
    RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(), Cron.Minutely());//otomatik belirtilen s�re i�in i�lem yapar.Bu method async methotlar�da otomatik olarak �al��t�rabilme �zelli�ine sahip.
    RecurringJob.RemoveIfExists(nameof(HesaplaManager));//ayn� methodun �oklamas� varsa ise tekini al�r. 
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
app.UseHangfireDashboard();//g�rsel bir dasboard ��kt�s� veriyor

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
