using Interface.DTO;
using Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IRepository
{
    public interface IOrderManagement
    {

        Task<string> CreateCart(int userid, int? itemid, int? mealid, int qnty);

        #region OrderStatus
        Task<string> CreateOrderStatus(string OrderStatus);
        Task<string> EditeOrderStatus(int id, string status);
        Task<List<OrderStatus>> AllOrderStatus();
        #endregion

        Task<string> CreatePaymentMethod(string paymentway);
        Task<string> EditePaymentMethod(int id,string paymentway);
        Task<List<Payment>> AllPayment();
    }
}
