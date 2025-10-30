using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using EmployeeManagement22.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Add Controllers
builder.Services.AddControllers();

// ✅ 2. Configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

// ✅ 3. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ✅ 4. Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// ✅ 5. Add Authorization
builder.Services.AddAuthorization();

// ✅ 6. Swagger with JWT Support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1"
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter 'Bearer {your token}'",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// ✅ 7. Dependency Injection
builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();
builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Leave>, Repository<Leave>>();
builder.Services.AddScoped<IRepository<Attendance>, Repository<Attendance>>();


builder.Services.AddScoped<UserHelper>();
// ✅ 8. App Build
var app = builder.Build();

// ✅ 9. Seed Data (Optional)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = new DataSeedHelper(dbContext);
    seeder.InsertData();
}

// ✅ 10. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowCorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

app.MapControllers();
app.Run();


//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// ✅ 1. Add Controllers
//builder.Services.AddControllers();

//// ✅ 2. Configure EF Core
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

//// ✅ 3. Configure CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowCorsPolicy", policy =>
//    {
//        policy
//            .WithOrigins("http://localhost:4200", "http://localhost:53255") // Add all Angular dev ports here
//            .AllowAnyMethod()
//            .AllowAnyHeader();
//    });
//});

//// ✅ 4. Configure JWT Authentication
//var jwtKey = builder.Configuration["Jwt:Key"];
//var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
//    };
//});

//// ✅ 5. Add Authorization
//builder.Services.AddAuthorization();

//// ✅ 6. Swagger with JWT Support
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Employee Management API",
//        Version = "v1"
//    });

//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Description = "Enter 'Bearer {your JWT token}'",

//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };

//    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { jwtSecurityScheme, Array.Empty<string>() }
//    });
//});

//// ✅ 7. Dependency Injection
//builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();
//builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
//builder.Services.AddScoped<IRepository<User>, Repository<User>>();
//builder.Services.AddScoped<IRepository<Leave>, Repository<Leave>>();
//builder.Services.AddScoped<UserHelper>();

//// ✅ 8. Build App
//var app = builder.Build();

//// ✅ 9. Seed Initial Data (Optional)
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    var seeder = new DataSeedHelper(dbContext);
//    seeder.InsertData();
//}

//// ✅ 10. Use Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// ❗ Order matters
//app.UseCors("AllowCorsPolicy"); // CORS before Auth
//app.UseHttpsRedirection();
//app.UseAuthentication(); // Must come before Authorization
//app.UseAuthorization();

//app.MapControllers();
//app.Run();
