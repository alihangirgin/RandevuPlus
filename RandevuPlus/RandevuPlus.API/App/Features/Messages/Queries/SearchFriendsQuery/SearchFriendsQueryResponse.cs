using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery
{
    public sealed record SearchFriendsQueryResponseItem(Guid Id, string Name)
    {
        public SearchFriendsQueryResponseItem() : this(Guid.Empty, string.Empty)
        {
        }
    }
}
