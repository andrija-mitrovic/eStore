namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateTokenAsync(string userName, string email);
    }
}
