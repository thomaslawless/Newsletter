using Supabase;
using Supabase.Interfaces;
using Supabase_Dotnet.Contracts;
using Supabase_Dotnet.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
            builder.Configuration["https://oiwoswtksltxzxfjpdif.supabase.co"],
            builder.Configuration["key"],
            new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapPost("/newsletters",
    async (
        CreateNewsletterRequest request,
         Supabase.Client client) =>
    {
        var newsletter = new Newsletter
        {
            Name = request.Name,
            Description = request.Description,
            ReadTime = request.ReadTime
        };

        var response = await client.From<Newsletter>().Insert(newsletter);

        var newNewsletter = response.Models.First();

        return Results.Ok(newNewsletter.Id);
});

app.MapGet("/newsletters/{id}", async (long id, Supabase.Client client) =>
{
    var response = await client
        .From<Newsletter>()
        .Where(n => n.Id == id)
        .Get();
    
    var newsletter = response.Models.FirstOrDefault();

    if (newsletter is null)
    {
        return Results.NotFound();
    }

    var newsletterResponse = new CreateNewsletterResponse
    {
        Id = newsletter.Id,
        Name = newsletter.Name,
        Description = newsletter.Description,
        ReadTime = newsletter.ReadTime,
        CreatedAt = newsletter.CreateedAt
    };

    return Results.Ok(newsletterResponse);
});


app.MapDelete("/newsletters/{id}", async (long id, Supabase.Client client) =>
{
    await client
    .From<Newsletter>()
    .Where(n => n.Id == id)
    .Delete();

    return Results.NoContent();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

