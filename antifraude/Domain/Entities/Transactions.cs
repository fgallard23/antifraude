using Domain.Exceptions;

namespace Domain.Entities
{
    public class Transactions
    {
        public Guid sourceAccountId { get; set; }
        public Guid targetAccountId { get; set; }
        public int transferTypeId { get; set; }
        public double value { get; set; }
        public TransactionsStatus TransactionsStatus { get; set; }

        private void ValidateState()
        {
            if (transferTypeId <= 0)
            {
                throw new InvalidTransferTypeIdException();
            }

            if (value <= 0)
            {
                throw new InvalidAmountException();
            }
        }

        public bool IsValid()
        {
            ValidateState();
            return true;
        }
    }
}
