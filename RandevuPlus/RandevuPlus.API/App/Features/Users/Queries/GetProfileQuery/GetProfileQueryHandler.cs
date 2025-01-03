using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;

namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<GetProfileQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public GetProfileQueryHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GetProfileQueryResponse>> Handle(GetProfileQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Result.Error("UserNotFound");
            return Result.Success(_mapper.Map<GetProfileQueryResponse>(user));
        }
    }
}
