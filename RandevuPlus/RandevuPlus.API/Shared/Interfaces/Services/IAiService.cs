namespace RandevuPlus.API.Shared.Interfaces.Services
{
    public interface IAiService
    {
        Task<string> AskQuestion(string question);
    }
}
