using System.Web.Mvc;
using UILayer.Controllers;
using UILayer.Models;
using System.Web;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using Moq;
namespace NUnitTesting
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void Email_Required()
        {
            // Arrange
            var model = new ForgotPasswordModel
            {
                Username = "kavya",
                NewPassword = "Kavya@123",
                ConfirmPassword = "Kavya@123"
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validate(model));
        }
        private void Validate(ForgotPasswordModel model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (validationResults.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException(errorMessage);
            }
        }

        [Test]
        public void Password_Required()
        {
            // Arrange
            var model = new ForgotPasswordModel
            {
                Username = "ramya",
                EmailAddress = "ramya@mail.com",
                ConfirmPassword = "Ramya@123"
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validate(model));
        }
        [Test]
        public void ConfirmPassword_Required()
        {
            // Arrange
            var model = new ForgotPasswordModel
            {
                Username = "ramya",
                EmailAddress = "ramya@mail.com",
                NewPassword = "Ramya@123"
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validate(model));
        }
        [Test]
        public void RoleName_Required()
        {
            var rolemodel = new RoleModel
            {
                RoleId = 1
            };
            Assert.Throws<ValidationException>(() => ValidateModel(rolemodel));
        }
        private void ValidateModel(RoleModel model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            if (validationResults.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException(errorMessage);
            }
        }

    }
}