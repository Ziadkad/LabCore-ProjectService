using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectService.Api.Middlewares;
using ProjectService.Api.Security;
using ProjectService.Application;
using ProjectService.Infrastructure;
using ProjectService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.RegisterApplicationServices();
builder.Services.RegisterAuthServices(builder.Configuration);
builder.Services.RegisterSwaggerServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

MigrateDbToLatestVersion(app);


await app.RunAsync();



static void MigrateDbToLatestVersion(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        context.Database.Migrate();
    }
    catch (SqlException ex) when (ex.Message.Contains("already exists"))
    {
        // Optional: log or skip
    }
}