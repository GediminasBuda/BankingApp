using AutoFixture.Xunit2;
using Domain.Services;
using FluentAssertions;
using Moq;
using Persistence.Models.ReadModels;
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
    public class TransactionService_Should
    {
        [Theory]
        [AutoMoqData]
        public async Task GetAllAsync_GivenUserIdandFirebaseId_ReturnsTransactionsResponse(
            [Frozen] Mock<IUserRepository> userRepositoryMock,
            [Frozen] Mock<ITransactionRepository> transactionRepositoryMock,
            IEnumerable<TransactionReadModel> transactionReadModels,
            Guid userId,
            string firebaseId,
            UserReadModel userReadModel,
            TransactionReadModel transactionReadModel,
            TransactionService sut)
        {
            //Arrange
            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetAsync(firebaseId))
                .ReturnsAsync(userReadModel);

            transactionRepositoryMock
                .Setup(transactionRepository => transactionRepository
                .GetAllAsync(userId))
                .ReturnsAsync(transactionReadModels);

            // Act
            var result = await sut.GetAllAsync(userId, firebaseId);

            //Assert
            result.Should().BeEquivalentTo(transactionReadModels, option => option
               .ExcludingMissingMembers());

            userRepositoryMock
               .Verify(userRepository => userRepository
               .GetAsync(It.IsAny<string>()), Times.Once);
            transactionRepositoryMock
                .Verify(transactionRepository => transactionRepository
                .GetAllAsync(It.IsAny<Guid>()), Times.Once);
        }
        [Theory]
        [AutoMoqData]
        public async Task TopUp_GivenUserIdandFirebaseId_ReturnsTransactionsResponse(
            [Frozen] Mock<IUserRepository> userRepositoryMock,
            [Frozen] Mock<ITransactionRepository> transactionRepositoryMock,
            IEnumerable<TransactionReadModel> transactionReadModels,
            Guid userId,
            string firebaseId,
            UserReadModel userReadModel,
            TransactionReadModel transactionReadModel,
            TransactionService sut)
        {

        }
    }
}
