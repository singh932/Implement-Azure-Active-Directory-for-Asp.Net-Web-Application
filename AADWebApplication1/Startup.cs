using AADWebApplication1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //var connectionString = Configuration.GetConnectionString("AADWebApplication1ContextConnection")
        //    ?? throw new InvalidOperationException("Connection string 'AADWebApplication1ContextConnection' not found.");

        //services.AddDbContext<AADWebApplication1Context>(options =>
        //    options.UseSqlServer(connectionString));

        //services.AddDefaultIdentity<IdentityUser>(options =>
        //    options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<AADWebApplication1Context>();

        services.AddControllersWithViews();
        services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
        services.AddMvc(option =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            option.Filters.Add(new AuthorizeFilter(policy));
        }).AddMicrosoftIdentityUI();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
