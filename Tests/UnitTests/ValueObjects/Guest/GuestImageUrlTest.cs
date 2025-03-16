using System;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Guest.Register
{
    public class GuestImageUrlTest
    {
        [Fact]
        public void Create_Valid_Url_Should_Succeed()
        {
            // Arrange
            var validUrl = new Uri("https://example.com/profile.jpg");

            // Act
            var result = GuestImageUrl.Create(validUrl);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validUrl, result.Unwrap().Value);
        }
        [Fact]
        public void Create_Valid_Url_Another_Domain_Should_Succeed()
        {
            // Arrange
            var validUrl = new Uri("https://otherdomain.com/profile.jpg");

            // Act
            var result = GuestImageUrl.Create(validUrl);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validUrl, result.Unwrap().Value);
        }

        [Fact]
        public void Create_Valid_Url_With_Query_Parameters_Should_Succeed()
        {
            // Arrange
            var validUrl = new Uri("https://example.com/profile.jpg?size=large&user=123");

            // Act
            var result = GuestImageUrl.Create(validUrl);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validUrl, result.Unwrap().Value);
        }
    }
}
