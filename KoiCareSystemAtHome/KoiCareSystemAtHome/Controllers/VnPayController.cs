﻿//using Azure.Core;
//using KoiCareSystemAtHome.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace KoiCareSystemAtHome.Controllers
//{
//    public class VnPayController
//    {
//        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
//        {
//            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

//            return Redirect(url);
//        }

//        public IActionResult PaymentCallback()
//        {
//            var response = _vnPayService.PaymentExecute(Request.Query);

//            return Json(response);
//        }
//    }
//}
