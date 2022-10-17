namespace Application.Common.Exceptions
{
    public sealed class BuyerIdNullException : Exception
    {
        public BuyerIdNullException() : base()
        {
        }

        public BuyerIdNullException(string message)
            : base(message)
        {
        }
    }
}
