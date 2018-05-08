﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Data.EntityModels;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;
using BookCave.Models.ViewModels;

namespace BookCave.Controllers
{
    public class HomeController : Controller
    {
        private BookService _bookService;

        public HomeController()
        {
            _bookService = new BookService();
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            return View(books);
        }

        public IActionResult AddToCart(Book book)
        {
            return View("Index");
        }

        public IActionResult Browse(string search)
        {
                if(search == null)
                {
                    var books = _bookService.GetAllBooks();
                    return View(books);
                }

                var result = _bookService.Search(search);
                return View(result);
        }

        public IActionResult BookDetails(int id)
        {
            //var book = new BookListViewModel() {Id=800, Title="book", Author="goggi", Image="img", Price=10.99, Rating=5.0 };
           
            var idbook = _bookService.GetBookById(id);
            return View(idbook);
        }
    }
}
