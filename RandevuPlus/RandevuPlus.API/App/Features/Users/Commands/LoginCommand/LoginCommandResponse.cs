namespace RandevuPlus.API.App.Features.Users.Commands.LoginCommand
{
    public record LoginCommandResponse(string AccessToken, DateTime ExpiresIn);
}
