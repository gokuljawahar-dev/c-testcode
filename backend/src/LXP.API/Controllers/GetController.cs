namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class GetController(IQuizService quizService, IQuizFeedbackService quizFeedbackService)
    : ControllerBase
{
    private readonly IQuizService _quizService = quizService;
    private readonly IQuizFeedbackService _quizFeedbackService = quizFeedbackService;

    // This controller is used by frontend to get quizrelateddetails


    [HttpGet("topic/{topicId}")]
    public ActionResult<Guid?> GetQuizIdByTopicId(Guid topicId)
    {
        var quizId = this._quizService.GetQuizIdByTopicId(topicId);

        if (quizId == null)
        {
            return this.Ok(null);
        }
        else
        {
            return this.Ok(quizId);
        }
    }
}


//using LXP.Core.IServices;

//using Microsoft.AspNetCore.Mvc;

//namespace LXP.Api.Controllers
//    [Route("api/[controller]")]
//    [ApiController]

//            _quizService = quizService;
//            _quizFeedbackService = quizFeedbackService;
//        // This controller is used by frontend to get quizrelateddetails


//            else


