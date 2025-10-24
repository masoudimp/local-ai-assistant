using LocalAiAssistantService.Services.Implementations;
using LocalAiAssistantService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LocalAiAssistantService.Settings.ModelBridgeSettings.OllamaSettings>(builder.Configuration.GetSection("Ollama"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IModelService, GemmiModelService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpClient<IModelBridgeService, OllamaBridgeService>();

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
