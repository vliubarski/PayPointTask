using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Domain;
using CustomerChargeNotification.PDFGeneration;
using CustomerChargeNotification.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustomerGameChargeContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerGameChargeRepository, CustomerGameChargeRepository>();


builder.Services.AddDbContext<CustomerContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddTransient<IChargeNotificationService, ChargeNotificationService>();
builder.Services.AddTransient<IPdfGenerator, PdfGenerator>();
builder.Services.AddTransient<IChargeNotificationProcessor, ChargeNotificationProcessor>();


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
