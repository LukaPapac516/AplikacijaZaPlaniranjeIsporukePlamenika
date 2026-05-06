using PlameniciAplikacija.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// EF Core DbContext sa SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "projekti_lista",
    pattern: "projekti",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "projekt_detalji",
    pattern: "projekti/{id:int}",
    defaults: new { controller = "Home", action = "Details" });

app.MapControllerRoute(
    name: "kupci_lista",
    pattern: "kupci",
    defaults: new { controller = "Kupci", action = "Index" });

app.MapControllerRoute(
    name: "djelatnici_lista",
    pattern: "djelatnici",
    defaults: new { controller = "Djelatnici", action = "Index" });

app.MapControllerRoute(
    name: "projekt_novi",
    pattern: "projekti/novi",
    defaults: new { controller = "Home", action = "Create" });

app.MapControllerRoute(
    name: "kupac_novi",
    pattern: "kupci/novi",
    defaults: new { controller = "Kupci", action = "Create" });

app.MapControllerRoute(
    name: "djelatnik_novi",
    pattern: "djelatnici/novi",
    defaults: new { controller = "Djelatnici", action = "Create" });

app.MapControllerRoute(
    name: "djelatnik_uredi",
    pattern: "djelatnici/{id:int}/uredi",
    defaults: new { controller = "Djelatnici", action = "Edit" });

app.MapControllerRoute(
    name: "radni_nalozi_lista",
    pattern: "radni-nalozi",
    defaults: new { controller = "RadniNalozi", action = "Index" });

app.MapControllerRoute(
    name: "radni_nalog_uredi",
    pattern: "radni-nalozi/{id:int}/uredi",
    defaults: new { controller = "RadniNalozi", action = "Edit" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

app.Run();
