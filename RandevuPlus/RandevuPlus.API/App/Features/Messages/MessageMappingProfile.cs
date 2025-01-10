using AutoMapper;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery;
using RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery;
using RandevuPlus.API.Shared.Domain;
using System.Globalization;

namespace RandevuPlus.API.App.Features.Messages
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, GetMessageQueryResponse>();
            CreateMap<Message, GetInboxQueryResponseItem>()
                .ForMember(dest => dest.ShortenedMessageText, opt => opt.MapFrom(src => ShortenMessage(src.MessageText, 10)))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.FullName))
                .ForMember(dest => dest.LastMessageDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("d MMM yyyy", new CultureInfo("tr-TR"))));
            CreateMap<AppUser, SearchFriendsQueryResponseItem>();
            CreateMap<Instructor, SearchFriendsQueryResponseItem>();
        }

        private string ShortenMessage(string messageText, int maxLength)
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
