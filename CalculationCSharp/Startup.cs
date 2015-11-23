using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CalculationCSharp.Startup))]
namespace CalculationCSharp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
