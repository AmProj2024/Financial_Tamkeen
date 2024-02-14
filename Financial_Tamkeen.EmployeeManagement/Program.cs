using Financial_Tamkeen.EmployeeManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.AddDbContext<BloggingContext>(options =>
//       options.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase")));
// Add services to the container.
//builder.Services.AddDbContext<AppDbContex>(options =>
//options.UseSqlServer(
//builder.Configuration.GetConnectionString("Data Source=02-TRI-TRAINING\\TEST3;Initial Catalog=Financial_Tamekeen;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
//));




var jwtOptions = builder.Configuration.GetSection("Jwt");
var issuer = jwtOptions.GetValue<string>("Issuer");
var key = jwtOptions.GetValue<string>("Key");

builder.Services.AddDbContext<AppDbContex>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//Jwt configuration starts here
//var UserName = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
//var Password = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = issuer,
                       ValidAudience = issuer,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                   };
               });
//Jwt configuration ends here


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<AppDbContex>();
//    context.Database.EnsureCreated();
//    //DbInitializer.Initialize(context);
//}

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();
