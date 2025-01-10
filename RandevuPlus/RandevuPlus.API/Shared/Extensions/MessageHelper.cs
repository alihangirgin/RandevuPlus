namespace RandevuPlus.API.Shared.Extensions
{
    public static class MessageHelper
    {
        public static string ShortenMessage(string messageText, int maxLength)
        {
            if (string.IsNullOrEmpty(messageText))
                return messageText;

            if (messageText.Contains("\n"))
            {
                var indexOfNewLine = messageText.IndexOf("\n");
                return messageText.Substring(0, indexOfNewLine) + "...";
            }

            if (messageText.Length > maxLength)
            {
                return $"{messageText.Substring(0, maxLength)} ...";
            }

            return messageText;
        }
    }
}
