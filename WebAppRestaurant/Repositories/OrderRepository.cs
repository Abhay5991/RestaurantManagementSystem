using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppRestaurant.Models;
using WebAppRestaurant.ViewModel;
using System.Web.Mvc;

namespace WebAppRestaurant.Repositories
{
    public class OrderRepository
    {
        private RestaurantDBEntities objRestaurantDBEntities;

        public  OrderRepository()
        {
            objRestaurantDBEntities = new RestaurantDBEntities();
        }

        public bool AddOrder(OrderViewModel objOrderViewModel)
        {
            try
            {
                Order objOrder = new Order();


                objOrder.CustomerId = objOrderViewModel.CustomerId;
                objOrder.FinalTotal = objOrderViewModel.FinalTotal;
                objOrder.OrderDate = DateTime.Now;
                objOrder.OrderNumber = String.Format("{0:ddmmyyyyhhmmss}", DateTime.Now);
                objOrder.PaymentTypeId = objOrderViewModel.PaymentTypeId;
                objRestaurantDBEntities.Orders.Add(objOrder);
                objRestaurantDBEntities.SaveChanges();

                int OrderId = objOrder.OrderId;


                foreach (var item in objOrderViewModel.ListOfOrderDetailViewModel)
                {
                    OrderDetail objOrderDetail = new OrderDetail();
                    objOrderDetail.Discount = item.Discount;
                    objOrderDetail.ItemId = item.ItemId;
                    objOrderDetail.Quantity = item.Quantity;
                    objOrderDetail.OrderId = objOrder.OrderId;
                    objOrderDetail.Total = item.Total;
                    objOrderDetail.UnitPrice = item.UnitPrice;

                    objRestaurantDBEntities.OrderDetails.Add(objOrderDetail);
                    objRestaurantDBEntities.SaveChanges();

                    Transaction objTransaction = new Transaction()
                    {
                        ItemId = item.ItemId,
                        Quantity = (-1) * item.Quantity,
                        TransactionDate = DateTime.Now,
                        TypeId = 2
                    };
                    objRestaurantDBEntities.Transactions.Add(objTransaction);
                    objRestaurantDBEntities.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}