using auth_api.Models;

namespace auth_api.Services
{
    public static class UserStore
    {
        public static List<User> Users { get; set; } = new List<User>();
    }
}