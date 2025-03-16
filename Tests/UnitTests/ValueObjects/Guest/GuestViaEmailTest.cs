using System;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Guest.Register
{
    public class GuestVIAEmailTest
    {
        [Theory]
        [InlineData("abc@via.dk")]
        [InlineData("abcd@via.dk")]
        [InlineData("123456@via.dk")]
        [InlineData("xyz@via.dk")]
        [InlineData("XYZ@via.dk")]
        public void Create_Valid_Email_Should_Succeed(string email)
        {
            // Act
            var result = GuestVIAEmail.Create(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(email.ToLower(), result.Unwrap().Value);
        }

        [Theory]
        [InlineData("abc@gmail.com")]
        [InlineData("test@hotmail.com")]
        [InlineData("user@yahoo.com")]
        public void Create_Invalid_Email_Should_Fail_Incorrect_Domain(string email)
        {
            // Act
            var result = GuestVIAEmail.Create(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Email must end with '@via.dk'");
        }

        [Theory]
        [InlineData("ab@via.dk")]        // Too short
        [InlineData("abcdefgh@via.dk")]  // Too long
        [InlineData("12345@via.dk")]     // Too short
        [InlineData("1234567@via.dk")]   // Too long
        public void Create_Invalid_Email_Should_Fail_Incorrect_Format(string email)
        {
            // Act
            var result = GuestVIAEmail.Create(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Incorrect email format");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_Invalid_Email_Should_Fail_Empty(string email)
        {
            // Act
            var result = GuestVIAEmail.Create(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Email is required");
        }

        [Theory]
        [InlineData("ab1@via.dk")]   // Invalid format
        [InlineData("abcdefg@via.dk")] // Invalid length
        [InlineData("abc12@via.dk")] // Invalid characters
        public void Create_Invalid_Email_Should_Fail_Invalid_Text1(string email)
        {
            // Act
            var result = GuestVIAEmail.Create(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message == "Incorrect email format");
        }
    }
}
