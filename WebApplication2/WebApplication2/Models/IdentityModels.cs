using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication2.Models
{
    public class CustomUserRole : IdentityUserRole<int> { public CustomUserRole() { } }
    public class CustomUserClaim : IdentityUserClaim<int> { public CustomUserClaim() { } }
    public class CustomUserLogin : IdentityUserLogin<int> { public CustomUserLogin() { } }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }



    //public class CustomUserRole : IdentityUserRole<int, CustomUserRole>
    //{
    //    public CustomUserRole() { }
    //    public CustomUserRole(string name) { Name = name; }
    //}

    //public class CustomUserClaim : IdentityUserClaim<int, CustomUserRole>
    //{
    //    public CustomUserClaim() { }
    //    public CustomUserClaim(string name) { Name = name; }
    //}

    //public class CustomUserLogin : IdentityUserLogin<int, CustomUserRole>
    //{
    //    public CustomUserLogin() { }
    //    public CustomUserLogin(string name) { Name = name; }
    //}







    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<IdentityUser>().ToTable("usuarios").Property(p => p.Id).HasColumnName("id_usuario");
			modelBuilder.Entity<ApplicationUser>().ToTable("usuarios").Property(p => p.Id).HasColumnName("id_usuario");    
            modelBuilder.Entity<IdentityRole>().ToTable("regras");
            modelBuilder.Entity<IdentityUserRole>().ToTable("usuarios_regras");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
        }

    }
}