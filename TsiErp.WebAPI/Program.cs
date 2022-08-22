using Autofac;
using Autofac.Extensions.DependencyInjection;
using Tsi.Core.Extensions;
using Tsi.Core.Modularity;
using Tsi.Logging.EntityFrameworkCore.Repositories;
using TsiErp.Business;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.DataAccess.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TsiErpDbContext>();
builder.Services.AddDbContext<LogDbContext>();



builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});

builder.Services.AddDependencyResolvers(new TsiModule[]
{
    new TsiBusinessModule()
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
