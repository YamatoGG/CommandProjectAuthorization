using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebBaza;
using WebBaza.Classes;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataBaseContext>(o => o.UseSqlite("Data Source=baza.sqlite3"));
var app = builder.Build();
app.UseHttpsRedirection();

// оно тут в шаблоне валялось я для себя тут это оставлю
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};
//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//});


app.MapPost("/register", async (Person person, DataBaseContext db) =>
{
    bool exists = await db.People.AnyAsync(p => p.Login == person.Login);

    if(exists) return Results.BadRequest("Этот логин уже занят.");

    db.People.Add(person);
    await db.SaveChangesAsync();
    return Results.Ok(person);
});

app.MapPost("/login", async (Person person, DataBaseContext db) =>
{
    var user = await db.People.FirstOrDefaultAsync(p => p.Login == person.Login);
    if (user == null || user.Password != person.Password)
        return Results.BadRequest("Неверный логин или пароль.");
    
    return Results.Ok(new { user.Id, user.Login });
});

app.MapGet("/products", async (DataBaseContext db) => 
{
    var products = db.Products.AsNoTracking().OrderBy(p => p.Id).Take(20).ToList();

    return Results.Ok(products);
});

await app.RunAsync();
