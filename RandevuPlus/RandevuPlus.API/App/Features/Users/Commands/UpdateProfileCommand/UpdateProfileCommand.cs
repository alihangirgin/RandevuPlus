using Ardalis.Result;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace RandevuPlus.API.App.Features.Users.Commands.UpdateProfileCommand
{
    public sealed record UpdateProfileCommand(string? FullName, string? OldPassword, string? NewPassword, IFormFile? ProfilePicture) : IRequest<Result>
    {
        [SwaggerSchema(ReadOnly = true)]
        public string? UploadUrl { get; private set; }
        public UpdateProfileCommand SetUrl(string scheme, string host)
        {
            return this with { UploadUrl = $"{scheme}://{host}/uploads/" };
        }
    }
}
