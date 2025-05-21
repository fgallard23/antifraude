using Domain;
using Domain.Entities;
using Domain.Exceptions;

namespace DomainTests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidAmountIsValid()
        {
            var trx = new Transactions()
            {
                sourceAccountId = Guid.NewGuid(),
                targetAccountId = Guid.NewGuid(),
                transferTypeId = 1,
                value = 1,
                TransactionsStatus = TransactionsStatus.Pending,
            };

            Assert.True(trx.IsValid());
        }

        [Fact]
        public void ShouldThrowErrorWhenTransferTypeIdIsZero()
        {
            var trx = new Transactions()
            {
                sourceAccountId = Guid.NewGuid(),
                targetAccountId = Guid.NewGuid(),
                transferTypeId = 0,
                value = 1,
                TransactionsStatus = TransactionsStatus.Pending,
            };

            Action act = () => trx.IsValid();
            var exception = Assert.Throws<InvalidTransferTypeIdException>(act);
        }

        [Fact]
        public void ShouldThrowErrorWhenValueIsZero()
        {
            var trx = new Transactions()
            {
                sourceAccountId = Guid.NewGuid(),
                targetAccountId = Guid.NewGuid(),
                transferTypeId = 1,
                value = 0,
                TransactionsStatus = TransactionsStatus.Pending,
            };

            Action act = () => trx.IsValid();
            var exception = Assert.Throws<InvalidAmountException>(act);
        }
    }
}