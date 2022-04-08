﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppRestaurant.Repositories;
using WebAppRestaurant.Models;
using WebAppRestaurant.ViewModel;

namespace WebAppRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantDBEntities restaurantDBEntities;
        public HomeController()
        {
            restaurantDBEntities = new RestaurantDBEntities();
        }
        // GET: Home
        public ActionResult Index()
        {
            var objCustomerRepository = new CustomerRepository();
            var objitemRepository = new ItemRepository();
            var objpaymentTypeRepository = new PaymentTypeRepository();  

          var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objCustomerRepository.GetAllCustomers(), objitemRepository.GetAllItems(), objpaymentTypeRepository.GetAllPaymentType());
            return View(objMultipleModels);
            
        }

        [HttpGet]
        public JsonResult getItemUnitPrice(int itemId)
        {
            decimal UnitPrice = restaurantDBEntities.Items.Single(model => model.ItemId == itemId).ItemPrice;
            return Json(UnitPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Index(OrderViewModel objOrderViewModel)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            bool isStatus = objOrderRepository.AddOrder(objOrderViewModel);
            string SuccessMessage = String.Empty;

            if (isStatus)
            {
                SuccessMessage = "Your Order Has Been Successfully Placed.";
            }
            else
            {
                SuccessMessage = "There Is Some Issue While Placing Order.";
            }
            return Json(SuccessMessage, JsonRequestBehavior.AllowGet);
        }

    }
}