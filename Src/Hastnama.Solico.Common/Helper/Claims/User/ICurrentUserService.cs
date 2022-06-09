namespace Hastnama.Solico.Common.Helper.Claims.User
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string FullName { get; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; }
    }
}