using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Services.AuthenticationService.Response
{
    public class AccessTokenResponse
    {
        public string AccessToken { get; set; }

        public double ExpirationTime { get; set; }

        public AccessTokenResponse(string accessToken, double expirationTime)
        {
            AccessToken = accessToken;
            ExpirationTime = expirationTime;
        }
    }
}
