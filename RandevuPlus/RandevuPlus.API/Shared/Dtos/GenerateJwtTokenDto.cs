namespace RandevuPlus.API.Shared.Dtos
{
    public sealed record GenerateJwtTokenDto(string AccessToken, DateTime ExpiresIn);   

}
