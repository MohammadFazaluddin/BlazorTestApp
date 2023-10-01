using BlazorTest.Data;
using BlazorTest.Data.Interfaces;
using BlazorTest.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection"), 
        assembly => assembly.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUsers, UsersRepository>();
builder.Services.AddScoped<IRoles, RolesRepository>();

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//securing api with authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
