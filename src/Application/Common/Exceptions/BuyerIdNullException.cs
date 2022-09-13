namespace Application.Common.Exceptions
{
    public class BuyerIdNullException : Exception
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
