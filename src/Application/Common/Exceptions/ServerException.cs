namespace Application.Common.Exceptions
{
    public sealed class ServerException : Exception
    {
        public ServerException() : base()
        {
        }

        public ServerException(string message)
            : base(message)
        {
        }
    }
}
