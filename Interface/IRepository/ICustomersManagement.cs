using Interface.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IRepository
{
    public interface ICustomersManagement
    {
        Task<List<CustomersResponseDTO>> AllCustomers();
        Task<List<CustomersResponseDTO>> SortCustomer(int? CustomerId, string? FirstName, string? Phone, string? lastname);
    }
}
