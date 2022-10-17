using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    internal sealed class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
