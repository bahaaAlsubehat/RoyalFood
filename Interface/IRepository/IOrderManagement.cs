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
        Task<string> DeleteFromCart(int cartitml);
        Task<string> AddNewOrder(OrderDTO orderDTO);
        Task<List<OrderResponseDTO>> Allorders();
        Task<List<OrderResponseDTO>> SortOrders( int? OrderNumber, string? CustomerName, string? CustomerPhone,
            string? OrderStatus, DateTime? DellivaryDate, DateTime? OrderDate);
        Task<string> Editeonorderstatusinorder(int OrderNumber, int orderstatusid);
        Task<string> RemoveFromOrder(int id, RemoveItemsOrMealsFromOrderDTO removeIMDTO);


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
