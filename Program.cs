using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ✅ Register DbContext
builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

// ✅ Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ✅ OpenAPI / Swagger (optional)
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// ✅ Dependency Injection
builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();

var app = builder.Build();

// ✅ Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var dataSeedHelper = new DataSeedHelper(dbContext);
    dataSeedHelper.InsertData();
}

// ✅ Enable middleware in correct order
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ✅ Enable CORS BEFORE MapControllers
app.UseCors("AllowCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
