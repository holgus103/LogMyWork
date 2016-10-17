using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogMyWork.Startup))]
namespace LogMyWork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
