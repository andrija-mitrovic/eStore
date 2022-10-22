using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void SingleValidationFailureCreatesASingleElementErrorDictionary()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("QuantityInStock", "Quantity in stock cannot be less than zero")
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo(new string[] { "QuantityInStock" });
            actual["QuantityInStock"].Should().BeEquivalentTo(new string[] { "Quantity in stock cannot be less than zero" });
        }

        [Fact]
        public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("QuantityInStock", "Quantity in stock cannot be less than zero"),
                new ValidationFailure("QuantityInStock", "Quantity in stock must be greater than zero"),
                new ValidationFailure("Name", "Name cannot be empty")
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo(new string[] { "QuantityInStock", "Name" });
            actual["QuantityInStock"].Should().BeEquivalentTo(new string[]
            {
                "Quantity in stock cannot be less than zero",
                "Quantity in stock must be greater than zero"
            });
            actual["Name"].Should().BeEquivalentTo(new string[] { "Name cannot be empty" });
        }
    }
}
