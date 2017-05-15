using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Elvencurse2.Server.Infrastructure
{
    public class ApplicationOAuthBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        private readonly string _cookieName;

        public ApplicationOAuthBearerAuthenticationProvider(string cookieName)
        {
            _cookieName = cookieName;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var tokenCookie = context.OwinContext.Request.Cookies[_cookieName];
            if (!string.IsNullOrEmpty(tokenCookie))
            {
                context.Token = tokenCookie;
            }

            return base.RequestToken(context);
        }
    }
}
