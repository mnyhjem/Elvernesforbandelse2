using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace Elvencurse2.Client.Components
{
    public static class Authentication
    {
        public static Cookie AuthCookie;

        public async static Task<bool> Authenticate(string username, string password)
        {
            AuthCookie = await AuthenticateUser(username, password);
            if (AuthCookie == null)
            {
                return false;
            }

            return true;
        }

        private async static Task<Cookie> AuthenticateUser(string user, string password)
        {
            var authenticationServer = ConfigurationManager.AppSettings["authserver"] + "/account/RemoteLogin";

            var request = WebRequest.Create(authenticationServer) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            var cookieName = "ElvenCurseAuthcookie";

            var authCredentials = "UserName=" + user + "&Password=" + password;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;

            try
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (WebException)
            {
                return null;
            }

            Cookie authCookie;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                authCookie = response.Cookies[cookieName];
            }

            if (authCookie != null)
            {
                return authCookie;
            }
            return null;
        }
    }
}
