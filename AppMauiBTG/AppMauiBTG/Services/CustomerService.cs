using AppMauiBTG.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMauiBTG.Services
{
    public class CustomerService : ICustomerService
    {
        private SQLiteAsyncConnection _database;

        public CustomerService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Cliente>().Wait();
        }

        public Task<List<Cliente>> GetCustomersAsync()
        {
            return _database.Table<Cliente>().ToListAsync();
        }

        public Task<Cliente> GetCustomerAsync(int id)
        {
            return _database.Table<Cliente>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerAsync(Cliente customer)
        {
            if (customer.Id != 0)
            {
                return _database.UpdateAsync(customer);
            }
            else
            {
                return _database.InsertAsync(customer);
            }
        }

        public Task<int> DeleteCustomerAsync(Cliente customer)
        {
            return _database.DeleteAsync(customer);
        }
    }
}

