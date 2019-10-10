using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Services.AuthenticationService.Response
{
    public class DefaultResponse : AccessTokenResponse
    {
        public string RefreshToken { get; set; }

        public DefaultResponse(string accessToken, string refreshToken, double expirationTime) : base(accessToken, expirationTime)
        {
            RefreshToken = refreshToken;
        }
    }
}
