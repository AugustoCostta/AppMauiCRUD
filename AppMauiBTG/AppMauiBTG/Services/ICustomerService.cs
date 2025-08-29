using AppMauiBTG.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMauiBTG.Services
{
    public interface ICustomerService
    {
        Task<List<Cliente>> GetCustomersAsync();
        Task<Cliente> GetCustomerAsync(int id);
        Task<int> SaveCustomerAsync(Cliente customer);
        Task<int> DeleteCustomerAsync(Cliente customer);
    }
}

