namespace LXP.Common.Constants;
public static class QuizEngineMessageConstants
{
    public static string QuizNotFound = "Quiz with ID {0} not found.";
    public static string LearnerAttemptNotFound = "Learner attempt with ID {0} not found.";
    public static string TimeLimitExpired = "Time limit for submitting the quiz has expired.";
    public static string OnlyOneOptionAllowed = "Only one option is allowed for this question type.";
    public static string SelectBetween2And3Options = "You must select between 2 and 3 options for this question type.";
    public static string InvalidOption = "The selected option '{0}' is not a valid option for the given question.";
    public static string AlreadyPassed = "You have already passed this quiz in a previous attempt.";
    public static string MaxAttemptsExceeded = "You have exceeded the maximum number of attempts for this quiz.";
    public static string AlreadyPassedCannotRetake = "You have already passed this quiz in a previous attempt and cannot retake it.";
}
