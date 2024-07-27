using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class EmailServiceExtension
    {
        // For send mails directly without scheduling
        public static Task<object[]> SendEmailConfirmationAsync(this IEmailService emailService, string firstName, string email, string link)
        {
            var body = string.Concat("Dear ", firstName, ", <br/><br/>  Your registration at KoTDA recruitement portal was successfull.<br/>" +
           "Please confirm your account by clicking the link below.<br/><br/> Login using your password and national ID. <br/><br/>" +
           $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
           @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Account Confirmation</a><br/><br/>" +
           "If you did not send this request, kindly ignore this email or reply to us.<br/><br/>" +
           "<b><i>This link is only valid for the next 30 minutes.</i></b><br/><br/>" +
           "Thanks.<br>Best Regards<br/>KoTDA ICT Team");

            return emailService.SendEmailAsync(email, "KoTDA Recruitement Portal - Confirm your email", body);
        }

        public static Task<object[]> SendEmailResetPasswordAsync(this IEmailService emailService, string firstName, string email, string link)
        {
            var body = string.Concat("Dear ", firstName, ", <br/><br/> " +
                "You have recently requested to reset your password for your KoTDA Recruitement account.<br/>" +
                "Click the link below to reset it.<br/><br/>" +
                $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
                @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Reset your Password</a><br/><br/>" +
                "If you did not request the password reset, kindly ignore this email or reply to us.<br/><br/>" +
                "<b><i>This password reset is only valid for the next 30 minutes.</i></b><br/><br/>" +
                "Thanks.<br>Best Regards<br/>KoTDA ICT  Team");

            return emailService.SendEmailAsync(email, "KoTDA Recruitement Portal - Reset Password", body);
        }

        public static Task<object[]> StaffEmailResetPasswordAsync(this IEmailService emailService, string firstName, string email, string link, string password)
        {
            var body = string.Concat("Dear ", firstName, ", <br/><br/> You have been created as a supplier of KoTDA Recruitement  Portal. Login to update your details and submit for approval. <br />" +
                "Use the link below to set your password to login or use password <strong>",password,"</strong> and national ID s as username. <br/>" +
                "Click the link below to reset it.<br/><br/>" +
                $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
                @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Reset your Password</a><br/><br/>" +
                "If you did not request the password reset, kindly ignore this email or reply to us.<br/><br/>" +
                "<b><i>This password reset is only valid for the next 30 minutes.</i></b><br/><br/>" +
                "Thanks.<br>Best Regards<br/>KoTDA ICT Team");
            return emailService.SendEmailAsync(email, "KoTDA Recruitement Portal - Reset Password", body);
        }

    }
}
