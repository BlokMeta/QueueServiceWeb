using SenderQueueMessageServices.NetCore;
using SenderQueueMessageServices.NetCore.Configuration;
using SenderQueueMessageServices.NetCore.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISenders, Senders>();


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
QueueSettings settings = config.GetRequiredSection("QueueSettings").Get<QueueSettings>();


//builder.Services.AddSingleton<IHttpContextAccessor>();
var app = builder.Build();
builder.Services.AddOptions();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
