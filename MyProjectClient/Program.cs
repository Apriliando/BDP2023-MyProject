using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        options.LoginPath = new PathString("/Home/Login");
//        options.AccessDeniedPath = new PathString("/Home/Error");
//    });
builder.Services.AddControllersWithViews();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(1);
//}); //added 28-4-2023
//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("AllowAnyOriginHeaderMethod", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //added 24-5-2023
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //added 24-5-2023

app.UseAuthentication();

//app.UseStatusCodePages(async context =>
//{
//    var request = context.HttpContext.Request;
//    var response = context.HttpContext.Response;
//    var path = request.Path.Value ?? "";
//    if (response.StatusCode == (int)HttpStatusCode.Unauthorized && path.StartsWith("/api", StringComparison.InvariantCultureIgnoreCase))
//    {
//        response.Redirect("/Home/Login");
//    }
//});

app.UseAuthorization();
//app.UseSession();//added 28-4-2023

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
