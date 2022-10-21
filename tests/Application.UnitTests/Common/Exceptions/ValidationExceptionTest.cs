using FluentAssertions;
using System;
using Xunit;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.Common.Exceptions
{
    public sealed class ValidationExceptionTest
    {
        [Fact]
        public void DefaultConstructorCreatesAnEmptyErrorDictionary()
        {
            var actual = new ValidationException().Errors;

            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }


    }
}
