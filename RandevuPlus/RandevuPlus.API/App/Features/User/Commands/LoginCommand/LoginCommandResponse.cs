namespace RandevuPlus.API.App.Features.User.Commands.LoginCommand
{
    public record LoginCommandResponse(string AccessToken, DateTime ExpiresIn);
}
