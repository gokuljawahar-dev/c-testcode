namespace LXP.Common.Constants;

public static class QuizServiceMessageConstants
{
    public static string TopicNotFound = "Topic with id {0} not found.";
    public static string QuizAlreadyExistsForTopic = "A quiz already exists for the topic with id {0}.";
    public static string NameOfQuizCannotBeNullOrEmpty = "NameOfQuiz cannot be null or empty.";
    public static string DurationMustBePositive = "Duration must be a positive integer.";
    public static string PassMarkMustBePositiveAndLessThanOrEqualTo100 = "PassMark must be a positive integer and less than or equal to 100.";
    public static string AttemptsAllowedMustBeNullOrPositive = "AttemptsAllowed must be null or a positive integer.";
}
