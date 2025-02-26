namespace Application.ValidatorMessages;

public static class ValidatorMessage
{
    public static string PostIdRequired => "Post Id is required.";
    public static string UserIdRequired => "User Id is required.";
    public static string ContentRequired => "Content is required.";
    public static string ContentMaxLength => "Content must not exceed 1,000 characters.";
}