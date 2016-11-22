using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GrataWork.Startup))]
namespace GrataWork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
