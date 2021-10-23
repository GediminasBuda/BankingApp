using AutoFixture.Xunit2;
using Contracts.Models.RequestModels;
using Domain.Clients.Firebase.Models;
using Domain.Clients.Firebase.Models.ResponseModels;
using Domain.Services;
using FluentAssertions;
using Moq;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelper.Attributes;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        [Theory]
        [AutoMoqData]
        public async Task SignUpAsync_With_SignUpRequest_ReturnsSignUpResponse(
            SignUpRequest signUpRequest,
            FirebaseSignUpResponse firebaseSignUpResponse,
            UserWriteModel userWriteModel,
            [Frozen] Mock<IFirebaseClient> firebaseClientMock,
            [Frozen] Mock<IUserRepository> userRepositoryMock,
            AuthService sut)
        {
            //Arrange
           
            firebaseSignUpResponse.Email = signUpRequest.Email;

            firebaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignUpAsync(signUpRequest.Email, signUpRequest.Password))
                .ReturnsAsync(firebaseSignUpResponse);

            // Act

            var result = await sut.SignUpAsync(signUpRequest);

            //Assert
            result.IdToken.Should().BeEquivalentTo(firebaseSignUpResponse.IdToken);
            result.Email.Should().BeEquivalentTo(firebaseSignUpResponse.Email);
            result.Email.Should().BeEquivalentTo(signUpRequest.Email);
            result.Username.Should().BeEquivalentTo(signUpRequest.Username);
            result.DateCreated.GetType().Should().Be<DateTime>();
            result.UserId.GetType().Should().Be<Guid>();

            firebaseClientMock
               .Verify(firebaseClient => firebaseClient
               .SignUpAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .SaveAsync(It.Is<UserWriteModel>(model =>
                model.FirebaseId.Equals(firebaseSignUpResponse.FirebaseId) &&
                model.Email.Equals(firebaseSignUpResponse.Email))), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task SignInAsync_WithSignInRequest_ReturnsSignInResponse(
           [Frozen] Mock<IFirebaseClient> firebaseClientMock,
           [Frozen] Mock<IUserRepository> userRepositoryMock,
           SignInRequest signInRequest,
           FirebaseSignInResponse firebaseSignInResponse,
           UserReadModel userReadModel,
           AuthService sut)
        {
            // Arrange
            firebaseSignInResponse.Email = signInRequest.Email;
            userReadModel.FirebaseId = firebaseSignInResponse.FirebaseId;
            userReadModel.Email = firebaseSignInResponse.Email;


            firebaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(firebaseSignInResponse);

            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetAsync(firebaseSignInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);

            // Act
            var result = await sut.SignInAsync(signInRequest);

            // Assert
            result.Email.Should().BeEquivalentTo(signInRequest.Email);
            result.IdToken.Should().BeEquivalentTo(firebaseSignInResponse.IdToken);
            result.Username.Should().BeEquivalentTo(userReadModel.Username);

            firebaseClientMock
                .Verify(firebaseClient => firebaseClient
                .SignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .GetAsync(firebaseSignInResponse.FirebaseId), Times.Once);
        }

    }
}
