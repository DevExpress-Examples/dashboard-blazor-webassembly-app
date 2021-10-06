using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.DataProtection;

namespace BlazorDashboardApp.Server {
    public class DefaultDashboardController : DashboardController {
        public DefaultDashboardController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider = null)
            : base(configurator, dataProtectionProvider) {
        }
    }
}