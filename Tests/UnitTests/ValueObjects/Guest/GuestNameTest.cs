using System;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Guest.Register
{
    public class GuestNameTest
    {
        [Theory]
        [InlineData("John", "Doe")]
        [InlineData("Alice", "Smith")]
        [InlineData("Michael", "Johnson")]
        [InlineData("Max", "Davis")]
        [InlineData("Oliver", "Clark")]
        public void Create_Valid_Names_Should_Succeed(string firstName, string lastName)
        {
            // Act
            var result = GuestName.Create(firstName, lastName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(firstName, result.Unwrap().firstName);
            Assert.Equal(lastName, result.Unwrap().lastName);
        }

        [Theory]
        [InlineData("")]
        public void Create_Invalid_FirstName_Empty_Should_Fail(string firstName)
        {
            // Act
            var result = GuestName.Create(firstName, "Doe");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Cannot have empty names!");
        }

        [Theory]
        [InlineData("")]
        public void Create_Invalid_LastName_Empty_Should_Fail(string lastName)
        {
            // Act
            var result = GuestName.Create("John", lastName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Cannot have empty names!");
        }

        [Theory]
        [InlineData("J", "Doe")]
        [InlineData("A", "Smith")]
        [InlineData("M", "Johnson")]
        public void Create_Invalid_Names_Too_Short_Should_Fail(string firstName, string lastName)
        {
            // Act
            var result = GuestName.Create(firstName, lastName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == $"{firstName} is too short, use at least 2 characters.");
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "Doe")]
        [InlineData("John", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void Create_Invalid_Names_Too_Long_Should_Fail(string firstName, string lastName)
        {
            // Act
            var result = GuestName.Create(firstName, lastName);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData("John123", "Doe")]
        [InlineData("Alice@", "Smith")]
        [InlineData("Max99", "Davis")]
        public void Create_Invalid_Names_With_Non_Letter_Characters_Should_Fail(string firstName, string lastName)
        {
            // Act
            var result = GuestName.Create(firstName, lastName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == $"{firstName} contains invalid characters. Only letters are allowed.");
        }

        [Fact]
        public void Create_Name_Should_Be_Capitalized_Correctly()
        {
            // Act
            var result = GuestName.Create("jOhn", "doe");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("John", result.Unwrap().firstName);
            Assert.Equal("Doe", result.Unwrap().lastName);
        }
    }
}
