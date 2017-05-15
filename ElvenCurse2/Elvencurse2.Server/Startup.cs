using System.Configuration;
using Elvencurse2.Server.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Owin.Security.AesDataProtectorProvider;

namespace Elvencurse2.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var cookiename = "ElvenCurseAuthcookie";

            var cookie = new CookieAuthenticationOptions
            {
                CookieName = cookiename,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            };

            app.Map("/signalr", map =>
            {
                map.UseCookieAuthentication(cookie);
                map.UseAesDataProtectorProvider(ConfigurationManager.AppSettings["cryptokey"]);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                {
                    Provider = new ApplicationOAuthBearerAuthenticationProvider(cookiename)
                });

                map.UseCors(CorsOptions.AllowAll);
                var config = new HubConfiguration { };
                map.RunSignalR(config);
            });
        }
    }
}
