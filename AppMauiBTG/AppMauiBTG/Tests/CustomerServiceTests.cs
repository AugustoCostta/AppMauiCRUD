using AppMauiBTG.Models;
using AppMauiBTG.Services;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace CustomerManagementApp.Tests
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private CustomerService _customerService;
        private string _dbPath;

        [SetUp]
        public void Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), "customers_test.db3");
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
            _customerService = new CustomerService(_dbPath);
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }

        [Test]
        public async Task SaveCustomerAsync_NewCustomer_AddsToDatabase()
        {
            var customer = new Cliente { Name = "Augusto", Lastname = "Costa", Age = 21, Address = "123 Paraná" };
            await _customerService.SaveCustomerAsync(customer);

            var customers = await _customerService.GetCustomersAsync();
            Assert.That(customers.Count, Is.EqualTo(1));
            Assert.That(customers[0].Name, Is.EqualTo("John"));
        }

        [Test]
        public async Task SaveCustomerAsync_ExistingCustomer_UpdatesDatabase()
        {
            var customer = new Cliente { Name = "Augusto", Lastname = "Costa", Age = 21, Address = "123 Paraná" };
            await _customerService.SaveCustomerAsync(customer);

            customer.Name = "Aaugusto";
            await _customerService.SaveCustomerAsync(customer);

            var customers = await _customerService.GetCustomersAsync();
            Assert.That(customers.Count, Is.EqualTo(1));
            Assert.That(customers[0].Name, Is.EqualTo("Aaugusto"));
        }

        [Test]
        public async Task DeleteCustomerAsync_RemovesFromDatabase()
        {
            var customer = new Cliente { Name = "Augusto", Lastname = "Costa", Age = 30, Address = "123 Paraná" };
            await _customerService.SaveCustomerAsync(customer);

            await _customerService.DeleteCustomerAsync(customer);

            var customers = await _customerService.GetCustomersAsync();
            Assert.That(customers.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetCustomerAsync_ReturnsCorrectCustomer()
        {
            var customer1 = new Cliente { Name = "Augusto", Lastname = "Costa", Age = 30, Address = "123 Paraná" };
            var customer2 = new Cliente { Name = "Aaugusto", Lastname = " Da Costa ", Age = 30, Address = "1234 Paraná" };
            await _customerService.SaveCustomerAsync(customer1);
            await _customerService.SaveCustomerAsync(customer2);

            var retrievedCustomer = await _customerService.GetCustomerAsync(customer1.Id);
            Assert.That(retrievedCustomer.Name, Is.EqualTo("Augusto"));
        }
    }
}

