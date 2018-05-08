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
using BookCave.Models.InputModels;

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
            var books = _bookService.GetHomePage();

            return View(books);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult AddToCart(Book book)
        {
            return View("Index");
        }

        public IActionResult Browse(string sortOrder, string search, string genre)
        {
            ViewBag.SortParm = sortOrder;
            var books = _bookService.GetAllBooks();
            

            if(!String.IsNullOrEmpty(search))
            {
                books = _bookService.Search(search,books);
            }

            if(!String.IsNullOrEmpty(genre))
            {
                books =_bookService.Filter(genre,books);
            }

            switch (sortOrder)
            {
                case "Az":
                    books = _bookService.SortByAz(books);
                    break;
                case "Za":
                    books = _bookService.SortByZa(books);
                    break;
                case "Rating":
                    books = _bookService.SortByRating(books);
                    break;
                case "PriceHigh":
                    books = _bookService.SortByPriceHigh(books);
                    break;
                case "PriceLow":
                    books = _bookService.SortByPriceLow(books);
                    break;
                case "DateNew":
                    books = _bookService.SortByReleaseNewest(books);
                    break;
                case "DateOld":
                    books = _bookService.SortByReleaseOldest(books);
                    break;
                default:
                    break;
            }
            
            return View(books);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddBook(BookInputModel newBook)
        {
            if(ModelState.IsValid)
            {
                _bookService.AddBook(newBook);
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Add Movie";
            return View();
        }

        public IActionResult Details(int id)
        {
            var idbook = _bookService.GetBookById(id);
            return View(idbook);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var bookToEdit = _bookService.GetBookById(id);
            return View(bookToEdit);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(BookInputModel book)
        {
            _bookService.UpdateBook(book);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public void AddReview()
        {

        }
    }
}
