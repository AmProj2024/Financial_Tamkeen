using Financial_Tamkeen.EmployeeManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//builder.AddDbContext<BloggingContext>(options =>
//       options.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase")));
// Add services to the container.
//builder.Services.AddDbContext<AppDbContex>(options =>
//options.UseSqlServer(
//builder.Configuration.GetConnectionString("Data Source=02-TRI-TRAINING\\TEST3;Initial Catalog=Financial_Tamekeen;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
//));



builder.Services.AddDbContext<AppDbContex>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


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
app.UseAuthorization();

app.MapControllers();

app.Run();
