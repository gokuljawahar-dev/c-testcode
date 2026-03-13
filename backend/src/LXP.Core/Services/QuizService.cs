namespace LXP.Core.Services;

using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class QuizService(
    IQuizRepository quizRepository,
    IFeedbackResponseRepository feedbackResponseRepository,
    IQuizFeedbackService quizFeedbackService
) : IQuizService
{
    private readonly IQuizRepository _quizRepository = quizRepository;
    private readonly IFeedbackResponseRepository _feedbackResponseRepository =
        feedbackResponseRepository;
    private readonly IQuizFeedbackService _quizFeedbackService = quizFeedbackService;

    public void CreateQuiz(QuizViewModel quiz, Guid topicId)
    {
        var topic =
            this._quizRepository.GetTopicById(topicId)
            ?? throw new Exception($"Topic with id {topicId} not found.");

        var courseId = topic.CourseId;

        var existingQuiz = this._quizRepository.GetQuizByTopicId(topicId);
        if (existingQuiz != null)
        {
            throw new Exception($"A quiz already exists for the topic with id {topicId}.");
        }

        ValidateQuiz(quiz);

        var quizEntity = new Quiz
        {
            QuizId = quiz.QuizId,
            CourseId = courseId,
            TopicId = topicId,
            NameOfQuiz = quiz.NameOfQuiz,
            Duration = quiz.Duration,
            PassMark = quiz.PassMark,
            AttemptsAllowed = quiz.AttemptsAllowed,
            CreatedBy = "Admin", // Updated
            CreatedAt = DateTime.Now // Updated
        };

        this._quizRepository.AddQuiz(quizEntity);
    }

    public void UpdateQuiz(QuizViewModel quiz)
    {
        ValidateQuiz(quiz);

        var quizEntity = this._quizRepository.GetQuizById(quiz.QuizId);
        if (quizEntity != null)
        {
            quizEntity.NameOfQuiz = quiz.NameOfQuiz;
            quizEntity.Duration = quiz.Duration;
            quizEntity.PassMark = quiz.PassMark;
            quizEntity.AttemptsAllowed = quiz.AttemptsAllowed;

            this._quizRepository.UpdateQuiz(quizEntity);
        }
    }

    public void DeleteQuiz(Guid quizId)
    {
        var quizEntity = this._quizRepository.GetQuizById(quizId);
        if (quizEntity != null)
        {
            // Check if there are any learner attempts associated with the quiz
            var learnerAttempts = this._quizRepository.GetLearnerAttemptsByQuizId(quizId);
            if (learnerAttempts.Any())
            {
                // Delete learner attempts associated with the quiz
                foreach (var attempt in learnerAttempts)
                {
                    this._quizRepository.DeleteLearnerAttempt(attempt);
                }
            }

            // Delete related feedback questions and their options
            this._quizFeedbackService.DeleteFeedbackQuestionsByQuizId(quizId);

            foreach (var question in this._quizRepository.GetQuizFeedbackQuestionsByQuizId(quizId))
            {
                this._feedbackResponseRepository.DeleteFeedbackResponsesByQuizQuestionId(
                    question.QuizFeedbackQuestionId
                );
            }

            this._quizRepository.DeleteQuiz(quizEntity);
        }
    }

    public IEnumerable<QuizViewModel> GetAllQuizzes() =>
        this
            ._quizRepository.GetAllQuizzes()
            .Select(q => new QuizViewModel
            {
                QuizId = q.QuizId,
                CourseId = q.CourseId,
                TopicId = q.TopicId,
                NameOfQuiz = q.NameOfQuiz,
                Duration = q.Duration,
                PassMark = q.PassMark,
                AttemptsAllowed = q.AttemptsAllowed
            })
            .ToList();

    public QuizViewModel GetQuizById(Guid quizId)
    {
        var quiz = this._quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            return null;
        }

        return new QuizViewModel
        {
            QuizId = quiz.QuizId,
            CourseId = quiz.CourseId,
            TopicId = quiz.TopicId,
            NameOfQuiz = quiz.NameOfQuiz,
            Duration = quiz.Duration,
            PassMark = quiz.PassMark,
            AttemptsAllowed = quiz.AttemptsAllowed
        };
    }

    public Guid? GetQuizIdByTopicId(Guid topicId)
    {
        var quiz = this._quizRepository.GetQuizByTopicId(topicId);
        return quiz?.QuizId;
    }

    private static void ValidateQuiz(QuizViewModel quiz)
    {
        if (string.IsNullOrWhiteSpace(quiz.NameOfQuiz))
        {
            throw new Exception("NameOfQuiz cannot be null or empty.");
        }

        if (quiz.Duration <= 0)
        {
            throw new Exception("Duration must be a positive integer.");
        }

        if (quiz.PassMark is <= 0 or >= 100)
        {
            throw new Exception(
                "PassMark must be a positive integer and less than or equal to 100"
            );
        }

        if (quiz.AttemptsAllowed.HasValue && quiz.AttemptsAllowed <= 0)
        {
            throw new Exception("AttemptsAllowed must be null or a positive integer.");
        }
    }
}
