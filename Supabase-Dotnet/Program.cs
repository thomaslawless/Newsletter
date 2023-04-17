﻿using Supabase;
using Supabase.Interfaces;
using Supabase_Dotnet.Contracts;
using Supabase_Dotnet.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
            builder.Configuration["SupabaseUrl"],
            builder.Configuration["SupabaseKey"],
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
        CreateNewsletterRequest request, Supabase.Client client) =>
    {
        var newsletter = new Newsletter
        {
            Name = request.Name,
            Description = request.Description,
            ReadTime = request.ReadTime
        }
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

