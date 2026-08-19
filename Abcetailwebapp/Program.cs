using Abcetailwebapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AzureTableService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Abcetailwebapp.Services.AzureStorageService>();
builder.Services.AddSingleton<Abcetailwebapp.Services.AzureTableService>();
builder.Services.AddSingleton<Abcetailwebapp.Services.AzureBlobService>();
builder.Services.AddSingleton<Abcetailwebapp.Services.AzureQueueService>();
builder.Services.AddSingleton<Abcetailwebapp.Services.AzureFileService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var azureTableService = services.GetRequiredService<Abcetailwebapp.Services.AzureTableService>();
    await azureTableService.CreateTablesAsync();
}
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
