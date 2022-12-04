using API.Extensions;
using API.Middleware;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddCacheServices(builder.Configuration);

builder.Services.AddCors(options => 
{
    options.AddPolicy("CorsPolicy",  policy => policy
        .WithOrigins("http://localhost:3000")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!roleManager.RoleExistsAsync("Admin").Result) 
    {
        var adminRole = new IdentityRole("Admin");
        _ = roleManager.CreateAsync(adminRole); 
    }
    
    if (!roleManager.RoleExistsAsync("User").Result) 
    {
        var userRole = new IdentityRole("User");
        _ = roleManager.CreateAsync(userRole);
    }

    // var userManager = services.GetRequiredService<UserManager<User>>();
    // await Seed.SeedData(context, userManager);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occured during migration");
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
