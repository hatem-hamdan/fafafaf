using MyStoreBis; // ريفرنس لطبقة البزنس فقط
using MyStore.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<OrderBusiness>();
// 1. قراءة نص الاتصال من الـ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddInfrastructureServices(connectionString);


builder.Services.AddHttpClient<IAdTrackingService, AdTrackingService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {

        ValidateIssuer = true, // إيقاف مؤقت لضمان عدم التعارض
        ValidateAudience = true, // إيقاف مؤقت لضمان عدم التعارض


    
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ClockSkew = TimeSpan.Zero, // 👈 هذا السطر يلغي الـ 5 دقائق الإضافية تماماً!

        ValidIssuer = "MyStoreApi", 
        ValidAudience = "MyStoreUsers", 
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456_MYSTORE_PROJECT")) // 👈 نفس المفتاح الموجود في البزنس بالملي!
    };



    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };






});



builder.Services.AddCors(options =>
{
    options.AddPolicy("MyStoreApiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7217",
                "http://localhost:5215",
                "https://localhost:5173",
                "https://my-store5.vercel.app",
                            "https://nexuvostore.vercel.app"


            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});






// الإعدادات الافتراضية للسيرفر والـ Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("MyStoreApiCorsPolicy");
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();

