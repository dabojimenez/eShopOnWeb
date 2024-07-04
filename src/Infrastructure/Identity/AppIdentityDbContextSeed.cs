using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Constants;

namespace Microsoft.eShopWeb.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {

        if (identityDbContext.Database.IsSqlServer())
        {
            identityDbContext.Database.Migrate();
        }

        await roleManager.CreateAsync(new IdentityRole(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS));

        var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
        await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

        string adminUserName = "admin@microsoft.com";
        var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
        await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
        adminUser = await userManager.FindByNameAsync(adminUserName);
        if (adminUser != null)
        {
            await userManager.AddToRoleAsync(adminUser, BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
        }
    }

    /// <summary>
    /// Esta funcion se encargara de actualizar la cuenta del usuario "demouser@microsoft.com" a "sistemaCompras@pruebas.com"
    /// </summary>
    /// <param name="identityDbContext"></param>
    /// <returns></returns>
    public static async Task UpdateAccountDemoToSistema(AppIdentityDbContext identityDbContext)
    {
        string accounUser = "demouser@microsoft.com";
        // Obtenemos al cuenta "demo"
        ApplicationUser? userDemoExist = await identityDbContext.Users
            .FirstOrDefaultAsync( u => u.UserName == accounUser || u.NormalizedUserName == accounUser.ToUpper());

        // Validamos que la cuenta "demouser" exista
        if (userDemoExist is null)
        {
            return;
        }

        // Cambiamos el nombre de la cuenta a "sistemaCompras"
        userDemoExist.UserName = "sistemaCompras@pruebas.com";
        // Tambiaen actializamos el usernameNormalizado
        userDemoExist.NormalizedUserName = userDemoExist.UserName.ToUpper();

        try
        {
            identityDbContext.SaveChanges();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message );
        }
    }
}
