using System.Collections.Concurrent;

namespace RandevuPlus.API.Shared.Models
{
    public static class OnlineUsers
    {
        public static ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();
    }
}
