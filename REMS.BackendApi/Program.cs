var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

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

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

//app.Urls.Add($"http://localhost:{appSettings.Port}");

app.MapControllers();

app.Run();