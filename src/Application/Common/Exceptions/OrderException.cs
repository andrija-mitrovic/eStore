namespace Application.Common.Exceptions
{
    public sealed class OrderException : Exception
    {
        public OrderException() : base()
        {
        }

        public OrderException(string message)
            : base(message)
        {
        }
    }
}
