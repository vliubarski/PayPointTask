using Microsoft.EntityFrameworkCore;
using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Domain;
using CustomerChargeNotification.PDFGeneration;
using CustomerChargeNotification.Services;
using CustomerChargeNotification.PdfUtils;
using iText.Kernel.Pdf;
using Hangfire;
using CustomerChargeNotification.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IChargeNotificationProcessor, ChargeNotificationProcessor>();

builder.Services.Configure<PdfSettings>(builder.Configuration.GetSection("PdfSettings"));

builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.AddScoped<IPdfGenerator>(sp =>
    new PdfGenerator(
        memoryStreamFactory: () => new MemoryStream(),
        pdfWriterFactory: ms => new PdfWriter(ms)
    ));

builder.Services.AddScoped<IPdfSaver, PdfSaver>();
builder.Services.AddSingleton<IFileSystem, FileSystem>();

builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();


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

HangfireConfig.ConfigureHangfireJobs(app, builder.Configuration);

app.Run();
