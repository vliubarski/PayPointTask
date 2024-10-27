using PayPoint.DAL;
using PayPoint.Domain;
using PayPoint.PDFGeneration;
using PayPoint.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICustomerGameChargeRepository, CustomerGameChargeRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

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
