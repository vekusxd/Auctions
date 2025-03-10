using Auctions.Database;
using Auctions.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(opts =>
    {
        opts.DocumentSettings = s =>
        {
            s.DocumentName = "AuctionsAPI";
            s.Title = "AuctionsAPI";
            s.Version = "v1";
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new Exception("No connection string was found");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
        });
});

builder.Services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(connectionString));
builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddMinioStorage(builder.Configuration);
builder.Services.AddAddSignEngine(builder.Configuration);

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(opts => opts.UseNpgsqlConnection(connectionString)));

builder.Services.SwaggerDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
    app.MapScalarApiReference();
    app.UseHangfireDashboard();
}

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config => { config.Endpoints.RoutePrefix = "api"; });
app.Run();