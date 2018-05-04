using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Models.ViewModels;
using BookCave.Services;

namespace BookCave.Controllers
{
    public class CartController : Controller
    {
        private CartService _cartService;

        public CartController()
        {
            _cartService = new CartService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public void AddToCart(int id)
        {
            _cartService.AddToCart(id);
        }






        /*
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
