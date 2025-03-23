/*
using EventAssociation.Core.Application.CommandDispatching.Commands;


namespace UnitTests.Features.Guest.Register
{
    public class RegisterANewGuestCommandTests
    {
        [Fact]
        public async Task Create_GuestCommand_With_Valid_Inputs_Should_Succeed()
        {/*#1#
            // Arrange
            string firstName = "Michael";
            string lastName = "Jackson";
            string email = "michael@jackson.com";
            string imageUrl = "https://example.com/michael.jpg";

            // Act
            var result = RegisterANewGuestCommand.Create(firstName, lastName, email, imageUrl);

            // Assert
            Assert.True(result.IsSuccess);
            var command = result.Unwrap();
            Assert.Equal(firstName, command.guestName.FirstName);
            Assert.Equal(lastName, command.guestName.LastName);
            Assert.Equal(email, command.email.Value);
            Assert.Equal(imageUrl, command.imageUrl.Value.ToString());
        }

        [Fact]
        public async Task Create_GuestCommand_With_Invalid_Email_Should_Fail()
        {
            // Arrange
            string firstName = "Michael";
            string lastName = "Jackson";
            string invalidEmail = "invalid-email";
            string imageUrl = "https://example.com/michael.jpg";

            // Act
            var result = RegisterANewGuestCommand.Create(firstName, lastName, invalidEmail, imageUrl);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message.Contains("Invalid email format"));
        }

        [Fact]
        public async Task Create_GuestCommand_With_Invalid_ImageUrl_Should_Fail()
        {
            // Arrange
            string firstName = "Michael";
            string lastName = "Jackson";
            string email = "abcd@via.dk";
            string invalidImageUrl = "not-a-valid-url"; // Invalid URL format

            // Act
            var result = RegisterANewGuestCommand.Create(firstName, lastName, email, invalidImageUrl);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message.Contains("Invalid image URL"));
        }

        [Fact]
        public async Task Create_GuestCommand_With_Empty_FirstName_Should_Fail()
        {
            // Arrange
            string emptyFirstName = "";
            string lastName = "Jackson";
            string email = "abcd@via.dk";
            string imageUrl = "https://example.com/michael.jpg";

            // Act
            var result = RegisterANewGuestCommand.Create(emptyFirstName, lastName, email, imageUrl);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.UnwrapErr(), e => e.Message.Contains("First name cannot be empty"));
        }

        [Fact]
        public async Task Create_GuestCommand_With_Multiple_Invalid_Fields_Should_Fail()
        {
            // Arrange
            string emptyFirstName = "";
            string lastName = "Jackson";
            string invalidEmail = "invalid-email";
            string invalidImageUrl = "not-a-valid-url";

            // Act
            var result = RegisterANewGuestCommand.Create(emptyFirstName, lastName, invalidEmail, invalidImageUrl);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.UnwrapErr().Count() > 1); // Ensure multiple errors are returned
        }
    }
}
*/
