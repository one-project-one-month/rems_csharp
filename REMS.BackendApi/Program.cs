
using REMS.Models.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers();
builder
    .AddSwagger()
    .AddDbService()
    .AddDataAccessService()
    .AddBusinessLogicService()
    .AddJwtAuthorization();

builder.Services.Configure<JwtTokenModel>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Urls.Add($"http://localhost:{appSettings.Port}");

app.MapControllers();

app.Run();

public class AppSettings
{
    public int Port { get; set; }
}