using Microsoft.EntityFrameworkCore;
using RecipeApp.API.Mapper;
using WCCompetitionApp.API.Data;
using WCCompetitionApp.API.Endpoints;
using WCCompetitionApp.API.Models;
using WCCompetitionApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WCCompetitionContext>(options =>
{
    if (!builder.Environment.IsEnvironment("Testing"))
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
        options.UseNpgsql(connectionString);
    }
});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});

builder.Services.AddScoped<IRepository<Team>, Repository<Team>>();
builder.Services.AddScoped<IRepository<Match>, Repository<Match>>();
builder.Services.AddScoped<IRepository<GroupPlay>, Repository<GroupPlay>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.ConfigureTeams();
app.ConfigureMatches();
app.ConfigureGroupPlays();

app.UseAuthorization();

app.MapControllers();

app.Run();
