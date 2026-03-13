namespace LXP.Api.Controllers;

using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController(
    ILearnerService learnerServices,
    IProfileService profileService,
    IPasswordHistoryService passwordHistoryService
) : BaseController
{
    private readonly ILearnerService _learnerServices = learnerServices;
    private readonly IProfileService _profileService = profileService;
    private readonly IPasswordHistoryService _passwordHistoryService = passwordHistoryService;
    private static DateTime _currentTIme; //Raj
    public readonly Hashtable _otpTable = [];
    private static readonly ConcurrentDictionary<string, string> emailOtpMap = new(); //Raj

    ///<summary>
    ///Post the learner and profile details
    ///</summary>
    ///
    [HttpPost("/lxp/learner/registration")]
    public async Task<IActionResult> Registration([FromBody] RegisterUserViewModel learner)
    {
        var learnerservices = await this._learnerServices.LearnerRegistration(learner);
        if (learnerservices)
        {
            return this.Ok(
                this.CreateSuccessResponse(MessageConstants.MsgLearnerRegistrationSuccess)
            );
        }
        else
        {
            return this.Ok(
                this.CreateFailureResponse(MessageConstants.MsgLearnerAlreadyExists, 400)
            );
        }
    }

    ///<summary>
    ///Fetch all the learner details
    ///</summary>
    ///
    [HttpGet("/lxp/view/learner")]
    public async Task<IActionResult> GetAllCategory()
    {
        var categories = await this._learnerServices.GetAllLearner();
        return this.Ok(this.CreateSuccessResponse(categories));
    }

    ///<summary>
    ///Fetch all the learner profle details
    ///</summary>
    [HttpGet("/lxp/view/learnerProfile")]
    public async Task<IActionResult> GetAllLearnerProfile()
    {
        var LearnerProfileone = await this._profileService.GetAllLearnerProfile();
        return this.Ok(this.CreateSuccessResponse(LearnerProfileone));
    }

    ///<summary>
    ///Fetching particular Learner profile details using Id
    ///</summary>
    [HttpGet("/lxp/view/learnerProfile/{id}")]
    public async Task<IActionResult> GetLearnerProfileById(string id)
    {
        var LearnerProfileone = this._profileService.GetLearnerProfileById(id);
        return this.Ok(this.CreateSuccessResponse(LearnerProfileone));
    }

    ///<summary>
    ///Fetching particular Learner details using Learner Id
    ///</summary>
    [HttpGet("/lxp/view/learner/{id}")]
    public async Task<IActionResult> GetLearnerById(string id)
    {
        var LearnerProfileone = this._learnerServices.GetLearnerById(id);
        return this.Ok(this.CreateSuccessResponse(LearnerProfileone));
    }


    ///<summary>
    ///Generating email to the repected mail they entered
    ///</summary>
    [HttpPost("EmailVerification")]
    public IActionResult GenerateOTP([FromQuery] string email)
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

        var sender = "rajkumarprofo@gmail.com"; // Replace with your Gmail address
        var senderPass = "mdjc ubpu wnse bjno"; // Replace with your Gmail password
        var subject = "LXP Email Verification";
        var body = $"The OTP to Verify Your Email is: {sOTP}";

        using (var mail = new MailMessage(sender, email))
        {
            mail.Subject = subject;
            mail.Body = body;

            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(sender, senderPass);
                smtpClient.EnableSsl = true;

                try
                {
                    smtpClient.Send(mail);
                    _currentTIme = DateTime.Now;
                }
                catch (Exception e)
                {
                }
            }
        }

        return this.Ok(emailOtpMap);
    }

    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
    }

    ///<summary>
    ///verifying the OTP by entering the email Id
    ///</summary>
    [HttpGet("VerifyOTP")]
    public IActionResult VerifyOTP([FromQuery] string email, [FromQuery] string userOTP)
    {
        if (emailOtpMap.TryGetValue(email, out var value))
        {
            var otpData = value;
            var storedTimestamp = _currentTIme;
            var currentTimestamp = DateTime.Now;
            var timeDifference = currentTimestamp - storedTimestamp;

            var num = timeDifference.TotalMinutes;
            if (timeDifference.TotalMinutes < 2)
            {
                if (otpData == userOTP)
                {
                    emailOtpMap.Remove(email, out _);

                    return this.Ok("OTP verified successfully!");
                }
                else
                {
                    return this.BadRequest("Invalid OTP provided.");
                }
            }
            else
            {
                emailOtpMap.Remove(email, out _);
                return this.BadRequest("OTP has expired.");
            }
        }
        else
        {
            return this.BadRequest("No OTP data found for the provided email.");
        }
    }

    [HttpPut("/lxp/learner/updateProfile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileViewModel model)
    {
        await this._profileService.UpdateProfile(model);

        return this.Ok(this.CreateSuccessResponse(200));
    }

    [HttpPut("/lxp/learner/updatePassword")]
    public async Task<IActionResult> UpdatePassword(
        string learnerId,
        string oldPassword,
        string newPassword
    )
    {
        var result = await this._passwordHistoryService.UpdatePassword(
            learnerId,
            oldPassword,
            newPassword
        );

        if (!result)
        {
            return this.BadRequest("Old password is incorrect");
        }

        return this.Ok("Password updated successfully");
    }

    ///<summary>
    ///Fetching particular Learner details and Profile details using Learner Id
    ///</summary>
    [HttpGet("/lxp/view/getlearner/{id}")]
    public async Task<IActionResult> LearnerGetLearnerById(string id)
    {
        var learnerWithProfile = await this._learnerServices.LearnerGetLearnerById(id);
        return this.Ok(this.CreateSuccessResponse(learnerWithProfile));
    }

    ///<summary>
    ///Get profile id by learner id Ruban
    ///</summary>
    [HttpGet("GetProfileId/{learnerId}")]
    public Guid GetProfile(Guid learnerId) => this._profileService.GetprofileId(learnerId);
}
