using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Give_Aid.Areas.Identity;
using Give_Aid.Models;
using Give_Aid.Areas.Identity.Data;
using Give_Aid.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new Give_Aid.Helpers.AdminViewLocationExpander());
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("smtpsettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    string adminEmail = "Admin@gmail.com"; // Replace with your admin email
    string adminPassword = "Admin@123"; // Replace with your admin password

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(newAdmin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }

    // Promote specific already-registered accounts to Admin. This is separate
    // from the block above: it doesn't create a new account, it just adds the
    // Admin role to an account that registered itself through the normal
    // Register page. Add any emails you want promoted to this list — it's
    // safe to leave running every startup, since it skips anyone already
    // in the role.
    string[] promoteToAdmin = { };

    foreach (var email in promoteToAdmin)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }

    // ONE-TIME FIX: remove the Admin role that was mistakenly granted to this
    // account earlier. Delete this whole block after running it once —
    // leaving it in permanently would let anyone re-register this exact
    // email and immediately get demoted, which is pointless noise long-term.
    var accidentalAdmin = await userManager.FindByEmailAsync("mutahirali115@gmail.com");
    if (accidentalAdmin != null && await userManager.IsInRoleAsync(accidentalAdmin, "Admin"))
    {
        await userManager.RemoveFromRoleAsync(accidentalAdmin, "Admin");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();