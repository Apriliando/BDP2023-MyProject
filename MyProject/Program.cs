using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Repository;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowAnyCorsPolicy = "AllowAnyOriginHeaderMethod";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
}); //added 22-5-2023
builder.Services.AddAuthorization(); //added 22-5-2023

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //updated 12-4-2023
//builder.Services.AddSession();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(options => options.UseLazyLoadingProxies().UseSqlServer(
    builder.Configuration.GetConnectionString("MyProjectContext"))
);
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<DepartmentRepository>(); //added 6-4-2023
builder.Services.AddScoped<AccountRepository>(); //added 11-4-2023
builder.Services.AddProblemDetails(); //added 5-4-2023
builder.Services.AddCors(c =>
{
    c.AddPolicy(name: MyAllowAnyCorsPolicy, options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //added 17-4-2023
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) -> if wrapper commented on 17-4-2023
//{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); // options added on 17-4-2023
        o.RoutePrefix = string.Empty;
    });
//}

app.UseProblemDetails(); //added 5-4-2023

app.UseHttpsRedirection();

app.UseAuthentication(); //added 22-5-2023

app.UseCors(MyAllowAnyCorsPolicy); //added 17-4-2023

app.UseAuthorization();

//app.UseSession();

app.MapControllers();

app.UseDeveloperExceptionPage();

app.Run();
