using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payment.Domain.Enums;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _adress;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentTests()
        {
            _name = new Name("Bruce","Waine");
            _document = new Document("09149787993", EDocumentType.CPF);
            _email = new Email("batman@dc.com");
            _adress = new Address("Rua 1", "1234", "Bairro legal", "Gothan", "SC", "BR", "88808000");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
            
        }

        //[TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {            
            var payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Wayne Corp", _adress, _email);
            _subscription.AddPayment(payment);            
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        //[TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenAddSubscription()
        {
            var payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Wayne Corp", _adress, _email);
            _subscription.AddPayment(payment);            
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }
    }
}