using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class PayPalPayment : Payment
    {
        public PayPalPayment(
            string transactionCode,
            DateTime paidDate,
            DateTime expireDate, 
            decimal total, 
            decimal totalPaid, 
            Document document, 
            string payer, 
            Address adress, 
            Email email) : base(
                paidDate, 
                expireDate, 
                total, 
                totalPaid, 
                document, 
                payer, 
                adress, 
                email)
        {
            TransactionCode = transactionCode;
        }

        public string TransactionCode { get; private set; }
    }
}