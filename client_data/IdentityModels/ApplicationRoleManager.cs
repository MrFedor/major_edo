
namespace client_data.IdentityModels
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
                    : base(store)
        { }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            ClientContext db = context.Get<ClientContext>();
            ApplicationRoleManager role = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ClientContext>()));
            return role;
        }
    }
}