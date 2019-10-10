using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Services.AuthenticationService.Response
{
    public class DefaultResponse
    {
        public bool Success { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public double? ExpirationTime { get; set; }

        public DefaultResponse(bool success, string accessToken, string refreshToken, double? expirationTime)
        {
            Success = success;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpirationTime = expirationTime;
        }

        public DefaultResponse(bool success) : this(success, null, null, null)
        { }


    }
}
