
using REMS.Models.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder
    .AddSwagger()
    .AddDbService()
    .AddDataAccessService()
    .AddBusinessLogicService()
    .AddJwtAuthorization();

builder.Services.Configure<JwtTokenModel>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();
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

app.MapControllers();

app.Run();