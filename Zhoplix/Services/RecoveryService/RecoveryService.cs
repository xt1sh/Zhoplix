using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels.Authentication;

namespace Zhoplix.Services.RecoveryService
{
    public class RecoveryService: IRecoveryService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUrlHelper _url;
        private readonly IEmailSender _emailSender;

        public RecoveryService(UserManager<User> userManager,
                                IUrlHelper url, 
                                IEmailSender emailSender)
        {
            _userManager = userManager;
            _url = url;
            _emailSender = emailSender;
        }

        public async Task<bool> SendResetPasswordMessageAsync(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return false;

            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            var user = regex.IsMatch(identifier)
                ? await _userManager.FindByEmailAsync(identifier)
                : _userManager.Users.SingleOrDefault(u => u.PhoneNumber == identifier);

            if (user is null)
            {
                return false;
            }


            if (regex.IsMatch(identifier))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var code = await _userManager.GenerateUserTokenAsync(user, "Default", "Password restore");
                var htmlMessage = this.GeneratePasswordResetMessage(user.Id, token, code);
                await _emailSender.SendEmailAsync(user.Email, "Password restore", htmlMessage);
            }

            return true;
        }

        public string GeneratePasswordResetMessage(int userId, string token, string code)
        {
            var callbackUrl = _url.Action(
               "resetPassword",
               "profile",
               new
               {
                   userId,
                   token,
                   code
               },
               protocol: "http"
           );
            return $"<a href='{callbackUrl}'>Reset Password</a>";
        }

        public async Task<User> VerifyPasswordResetCodeAsync(ResetCodeViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
                return null;

            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "Password restore", model.Code);

            if (isValid)
                return user;

            return null;
        }
    }
}
