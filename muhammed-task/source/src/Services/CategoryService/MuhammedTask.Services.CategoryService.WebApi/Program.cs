using MuhammedTask.BuildingBlocks.Database.Migrator;
using MuhammedTask.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
using MuhammedTask.Services.CategoryService.WebApi.ServiceRegistrations;
using MuhammedTask.Services.Info;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceRegistrations(builder.Environment, builder.Configuration);
builder.AddSeqEndpoint(ServiceKeys.Seq);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    await app.MigrateAsync<ApplicationWriteDbContext>();

app.UseServices();

await app.RunAsync();
