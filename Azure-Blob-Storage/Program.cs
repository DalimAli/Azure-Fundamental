using Azure.Storage.Blobs;
using Azure_Blob_Storage.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

var blobConnectionString = builder.Configuration["AzureBlobConnection"];
//var test = builder.Configuration.GetValue<string>("AzureBlobConnection");   
builder.Services.AddSingleton(u => new BlobServiceClient(blobConnectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IContainerService, ContainerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
