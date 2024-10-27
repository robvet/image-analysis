using System.Runtime.CompilerServices;
using Azure.Storage.Blobs;
using ImageCaptionService;
using ImageCaptionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Engage plumbing for IConfiguration and user secrets
IConfiguration configuration = builder.Configuration;

// Access secrets from user secrets
string blobSasUrl = configuration["blobstorage_connectionstring"];
string blobContainerName = configuration["blobstorage_containername"];

// Create BlobClient using the SAS URL
var blobClient = new BlobClient(new Uri(blobSasUrl));
builder.Services.AddSingleton(blobClient);
//builder.Services.AddSingleton(blobContainerName);
builder.Services.AddScoped<IImageCaptionOrchestrator, ImageCaptionOrchestrator>();
builder.Services.AddScoped<IFetchImage, FetchImage>();


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
