using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RefactorThis.Application.Common.Interfaces;
using RefactorThis.Application.Services;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;

namespace RefactorThis.Domain.Tests
{
    [TestFixture]
	public class InvoicePaymentProcessorTests
	{
        private ServiceProvider _serviceProvider;
        private InvoiceService _service;
		private IInvoiceRepository _repository;


        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddPersistence();
            _serviceProvider = services.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<InvoiceService>();
			_repository = _serviceProvider.GetRequiredService<IInvoiceRepository>();
        }


        [Test]
		public async Task ProcessPayment_Should_ThrowException_When_NoInoiceFoundForPaymentReference( )
		{
			var payment = new Payment();
			var failureMessage = "";

			try
			{
				var result = await _service.ProcessPayment(payment);
			}
			catch (InvalidOperationException e)
			{
				failureMessage = e.Message;
			}

			Assert.AreEqual("There is no invoice matching this payment", failureMessage);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded( )
		{
			var invoice = new Invoice
			{
				Amount = 0,
				AmountPaid = 0,
				Payments = null
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
				InvoiceId = invoice.Id,
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("no payment needed", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 10,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 10
					}
				}
			};

            await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
				InvoiceId = invoice.Id
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("invoice was already fully paid", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
				InvoiceId = invoice.Id,
				Amount = 6
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("the payment is greater than the partial amount remaining", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount( )
		{
			var invoice = new Invoice
			{
				Amount = 5,
				AmountPaid = 0,
				Payments = new List<Payment>()
			};

            await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
				InvoiceId = invoice.Id,
				Amount = 6
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("the payment is greater than the invoice amount", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidAreEqualAmountDue( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
                InvoiceId = invoice.Id,
                Amount = 5
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("final partial payment received, invoice is now fully paid", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidAreEqualInvoiceAmount( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment> {
					new Payment {
						Amount = 10 
					} 
				}
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
                InvoiceId = invoice.Id,
                Amount = 10
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("invoice was already fully paid", result);
		}

		[Test]
        public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
                InvoiceId = invoice.Id,
                Amount = 1
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("another partial payment received, still not fully paid", result);
		}

		[Test]
		public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount( )
		{
			var invoice = new Invoice
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment>()
			};

			await _repository.SaveInvoice(invoice);

			var payment = new Payment
			{
				InvoiceId = invoice.Id,
				Amount = 1
			};

			var result = await _service.ProcessPayment(payment);

			Assert.AreEqual("invoice is now partially paid", result);
		}
	}
}