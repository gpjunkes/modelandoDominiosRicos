using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payment.Domain.Enums;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Tests.Handlers
{
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Guilherme";
            command.LastName = "Junkes";
            command.Document = "99999999999";
            command.Email = "guilherme.junkes@aaa.com";
            command.BarCode = "123123";
            command.BoletoNumber = "123123";
            command.PaymentNumber = "3123";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "JNKS CORP";
            command.PayerDocument = "123123123123";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "teste@123.com";
            command.Street = "asdasd";
            command.Number = "asdasda";
            command.Neighborhood = "asdasd";
            command.City = "asdasd";
            command.State = "as";
            command.Country = "as";
            command.ZipCode = "123123123";

            handler.Handle(command);

        }
    }
}
