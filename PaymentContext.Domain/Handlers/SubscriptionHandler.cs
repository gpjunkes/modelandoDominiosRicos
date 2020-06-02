using Flunt.Notifications;
using Flunt.Validations;
using Payment.Domain.Enums;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;
using System;
using System.Xml.Linq;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
        Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePaypalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(
            IStudentRepository repository,
            IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }
            // verificar se documento já está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            // verificar se e-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Document", "Este e-mail já está em uso");
            }

            // gerar os VOs 
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var adress = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                command.Payer, 
                adress, 
                email
            );

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // agrupar as validações
            AddNotifications(name, document, email, adress, student, subscription, payment);

            // salvar as informações
            _repository.CreateSubstription(student);

            // enviar e-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Adress, "Bem vindo!", "Sua assinatura foi criada.");

            // retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePaypalSubscriptionCommand command)
        {
            // verificar se documento já está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            // verificar se e-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Document", "Este e-mail já está em uso");
            }

            // gerar os VOs 
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var adress = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                new Document(command.PayerDocument, command.PayerDocumentType),
                command.Payer,
                adress,
                email
            );

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // agrupar as validações
            AddNotifications(name, document, email, adress, student, subscription, payment);

            // checar as notificações
            if (Invalid)
            {
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            // salvar as informações
            _repository.CreateSubstription(student);

            // enviar e-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Adress, "Bem vindo!", "Sua assinatura foi criada.");

            // retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}
