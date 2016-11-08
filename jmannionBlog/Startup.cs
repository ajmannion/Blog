using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jmannionBlog.Startup))]
namespace jmannionBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
