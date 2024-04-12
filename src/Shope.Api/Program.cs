using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shope.Api.Extensions.Endpoints;
using Shope.Application.Base.Database;
using Shope.Application.Base.Notifications;
using Shope.Application.Base.Reports;
using Shope.Application.Events;
using Shope.Infrastructure;
using Shope.Infrastructure.Notifications;
using Shope.Infrastructure.Reports;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<IShopeeContext, ShopeeContext>(options =>
    options.UseInMemoryDatabase("ShopeeDatabase")
);
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<OrderConfirmedEvent>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<ShopeeContext>()!;
    ShopeeContextSeed.Seed(context);
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
