namespace LXP.Api.Controllers;

using System.Collections.Concurrent;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmailController(IEmailService emailService) : ControllerBase
{
    private readonly IEmailService _emailService = emailService;
    private static readonly ConcurrentDictionary<string, string> emailOtpMap = new();
    private static DateTime _currentTIme;

    ///<summary>
    ///Generating email to the repected mail they entered
    ///</summary>
    [HttpPost("EmailVerification")]
    public async Task<IActionResult> GenerateOTP([FromBody] string email)
    {
        string[] saAllowedCharacters = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
        var sOTP = "";
        var rand = new Random();

        for (var i = 0; i < 6; i++)
        {
            var p = rand.Next(0, saAllowedCharacters.Length);
            sOTP += saAllowedCharacters[p];
        }

        emailOtpMap[email] = sOTP;

        var subject = "LXP Email Verification";
        var body = $"The OTP to Verify Your Email is: {sOTP}";


        var emailSent = await this._emailService.SendEmailAsync(email, subject, body);

        if (emailSent)
        {
            _currentTIme = DateTime.Now;
            return this.Ok(
                new
                {
                    Message = "Email sent successfully",
                    Email = email,
                    OTP = sOTP
                }
            );
        }
        else
        {
            return this.BadRequest(new { Message = "Error sending email" });
        }
    }

    ///<summary>
    ///Validating the OTP
    ///</summary>
    [HttpPost("VerifyOTP")]
    public IActionResult VerifyOTP([FromBody] OTPVerificationViewModel otpverify)
    {
        if (emailOtpMap.TryGetValue(otpverify.Email, out var value))
        {
            var otpData = value;
            var storedTimestamp = _currentTIme;
            var currentTimestamp = DateTime.Now;
            var timeDifference = currentTimestamp - storedTimestamp;

            if (timeDifference.TotalMinutes < 2)
            {
                if (otpData == otpverify.OTP)
                {
                    emailOtpMap.Remove(otpverify.Email, out _);

                    return this.Ok("OTP verified successfully!");
                }
                else
                {
                    return this.BadRequest("Invalid OTP provided.");
                }
            }
            else
            {
                emailOtpMap.Remove(otpverify.Email, out _);
                return this.BadRequest(
                    "OTP has expired." + storedTimestamp + "#####" + _currentTIme
                );
            }
        }
        else
        {
            return this.BadRequest("No OTP data found for the provided email.");
        }
    }
}

