using Data.Interfaces;
using Data.Entities;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class NotificationExtension
    {
        // For scheduling emails
        public static Task<int> SendEmailConfirmationAsync(this INotificationService notification, ApplicationUser user, string link)
        {
            var body = string.Concat("Dear ", user.FullNames, ", <br/><br/>  Your registration at DPW Assessment was successfull.<br/>" +
           "Please confirm your account by clicking the link below.<br/><br/> Login using your password and company email address. <br/><br/>" +
           $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
           @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Account Confirmation</a><br/><br/>" +
           "If you did not send this request, kindly ignore this email or reply to us.<br/><br/>" +
           "<b><i>This link is only valid for the next 30 minutes.</i></b><br/><br/>" +
           "Thanks.<br>Best Regards<br/>DPW Assessment ICT Team");

            return notification.ScheduleEmailNotificationAsync(user.Email, "Email Account Confirmation ", body, "Registration",user.Id, user.FullNames, "");
        }

        public static Task<int> SendEmailResetPasswordAsync(this INotificationService notification, ApplicationUser user, string link)
        {
            var body = string.Concat("Dear ", user.FullNames, ", <br/><br/> " +
                "You have recently requested to reset your password for your DPW Assessment account.<br/>" +
                "Click the link below to reset it.<br/><br/>" +
                $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
                @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Reset your Password</a><br/><br/>" +
                "If you did not request the password reset, kindly ignore this email or reply to us.<br/><br/>" +
                "<b><i>This password reset is only valid for the next 30 minutes.</i></b><br/><br/>" +
                "Thanks.<br>Best Regards<br/>DPW Assessment ICT  Team");

            return notification.ScheduleEmailNotificationAsync(user.Email, "Password Reset", body, "ResetPassword", user.Id, user.FullNames, "");
        }

        public static Task<int> VendorEmailResetPasswordAsync(this INotificationService notification, ApplicationUser user, string link, string password)
        {
            var body = string.Concat("Dear ", user.FullNames, ", <br/><br/> You have been created as a supplier of DPW Assessmentt Portal. Login to update your details and submit for approval. <br />" +
                "Use the link below to set your password to login or use password <strong>", password, "</strong> and company email address as username. <br/>" +
                "Click the link below to reset it.<br/><br/>" +
                $"<a href='{HtmlEncoder.Default.Encode(link)}' " +
                @"style=""font-size: 14px; color:#FFFFFF; padding:7px;  background-color:dodgerblue"">Reset your Password</a><br/><br/>" +
                "If you did not request the password reset, kindly ignore this email or reply to us.<br/><br/>" +
                "<b><i>This password reset is only valid for the next 30 minutes.</i></b><br/><br/>" +
                "Thanks.<br>Best Regards<br/>DPW Assessment ICT Team");
            return notification.ScheduleEmailNotificationAsync(user.Email, "Account Activation Request", body, "AccountActivation", user.Id, user.FullNames, "");
        }
    }
}
