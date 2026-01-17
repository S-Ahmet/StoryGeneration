using GNA.API.AppSettings;
using GNA.API.Interfaces;
using GNA.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// !!! YENİ EKLENEN SATIR: HTTP İstekleri için gerekli !!!
builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Servisler
builder.Services.AddScoped<IMcpService, McpService>();
builder.Services.AddScoped<IStoryService, StoryService>();

// Settings
var settings = Settings.LoadFromConfiguration(builder.Configuration);
builder.Services.AddSingleton(settings);

var app = builder.Build();

app.UseCors("AllowAll");

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();