using System;
using System.Collections.Generic;
using System.Linq;
using BookCave.Models;
using BookCave.Models.InputModels;
using BookCave.Models.InputViewModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services 
{
    public class BookService : IBookService
    {
        private BookRepo _bookRepo;

        public BookService()
        {
            _bookRepo = new BookRepo();
        }

        public List<BookListViewModel> GetAllBooks()
        {
            var books = _bookRepo.GetAllBooks();
            return books; 
        }

        public HomeViewModel GetHomePage()
        {
            var books = new HomeViewModel { NewReleases = _bookRepo.GetNewReleases(), TopRated = _bookRepo.GetTopRated()};
            return books;
        }
        
        public List<BookListViewModel> Search(string str,List<BookListViewModel> books)
        {   
            var byname  = (from a in books
                        where a.Title.ToLower().Contains(str.ToLower())
                        select a);

            var byauthor = (from a in books
                        where a.Author.ToLower().Contains(str.ToLower())
                        select a);

            var result = byname.Concat(byauthor).ToList();  // Combine Lists   
              
            return result;
        }

        public BookDetailsViewModel GetBookById(int id)
        {
            var book = _bookRepo.GetBookById(id);
            return book; 
        }

        public List<ReviewViewModel> GetReviews(int id)
        {
            var reviews = _bookRepo.GetReviews(id);
            return(reviews);
        }
        public List<WishlistViewModel> GetWishlist(List<WishlistViewModel> WishlistId)
        {
            var Wishlist = new List<WishlistViewModel>();
            foreach(var id in WishlistId)
            {
                var book = _bookRepo.GetWishlistById(id.BookId);
                book.Id = id.Id;
                Wishlist.Add(book);
            }
            return Wishlist;
        }

        public void ChangeUserIdToName(List<ReviewViewModel> reviews, IQueryable<ApplicationUser> username)
        {
            foreach(var r in reviews)
            {
                foreach(var u in username)
                {
                    if(r.UserId == u.Id)
                    {
                        r.UserId = u.FirstName;
                    } 
                }
            }
        }

        public void AddReview(ReviewInputModel review)
        {
            _bookRepo.AddReview(review);
            UpdateBookRating(review.BookId);
        }

        public void UpdateBookRating(int BookId)
        {
            var reviews = _bookRepo.GetReviews(BookId);
            if(reviews.Count == 0)
            {
                _bookRepo.UpdateBookRating(BookId, 0.0);
            }
            else
            {
                var sumOfRatings = 0.0;
                foreach(var r in reviews)
                {
                    sumOfRatings += r.Rating;
                }
                var numberOfReviews = reviews.Count;
                var newRating = sumOfRatings / numberOfReviews;
                _bookRepo.UpdateBookRating(BookId, newRating);
            }

        }

        public void AddBook(BookInputModel newBook)
        {
            _bookRepo.AddBook(newBook);
        }

        public List<BookListViewModel> Filter(string str,List<BookListViewModel> Books)
        {
            var result = (from a in Books  
                        where a.Genre.ToLower() == str.ToLower()
                        select a).ToList();
            return result;
        }

        public List<BookListViewModel> SortByAz(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.Title ascending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByZa(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.Title descending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByRating(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.Rating descending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByPriceHigh(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.Price descending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByPriceLow(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.Price ascending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByReleaseNewest(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.ReleaseDate descending
                        select a).ToList();

            return sorted;
        }

        public List<BookListViewModel> SortByReleaseOldest(List<BookListViewModel> books)
        {
            var sorted = (from a in books
                        orderby a.ReleaseDate ascending
                        select a).ToList();

            return sorted;
        }

        public void DeleteBook(int id)
        {
            _bookRepo.DeleteBook(id);
        }

        public void UpdateBook(BookInputModel book)
        {
            _bookRepo.UpdateBook(book);
        }

        public void ProcessBook(BookInputModel book)
        {

            if(string.IsNullOrEmpty(book.Title))
            {
                throw new Exception("Title is missing!");
            }

            if(string.IsNullOrEmpty(book.Author))
            {
                throw new Exception("Author is missing!");
            }

            if(string.IsNullOrEmpty(book.Genre))
            {
                throw new Exception("Genre is missing!");
            }

            if(book.Price <= 0)
            {
                throw new Exception("Prixe is invalid!");
            }

            if(book.ReleaseDate.Year <= 0)
            {
                throw new Exception("Release date year is invalid");
            }

            if(book.ReleaseDate.Month <= 0)
            {
                throw new Exception("Release date month is invalid");
            }

            if(book.ReleaseDate.Day <= 0)
            {
                throw new Exception("Release date day is invalid");
            }

            if(string.IsNullOrEmpty(book.Image))
            {
                throw new Exception("Image is missing");
            }

            if(string.IsNullOrEmpty(book.Description))
            {
                throw new Exception("Description is missing!");
            }
        }
        public int GetHighestBookId()
        {
            return _bookRepo.GetHighestBookId();
        }

        public DetailsInputViewModel GoToRandomBook(IQueryable<ApplicationUser> username)
        {
            Random rnd = new Random();                      //Create a new instance of the random class
            var allBooks = GetAllBooks();                   //Retrieve all books from the database
            int randomId = rnd.Next(GetHighestBookId());    //Get a (pseudo)random Id in the range of Id's
            while(GetBookById(randomId) == null)            
            {                                               //Some Id's that are fetched don't belong to any book
                randomId = rnd.Next(GetHighestBookId());    //so a new Id has to be fetched until it is valid
            }                                               //to avoid a NullReferenceException
            var newbook = new DetailsInputViewModel();      
            newbook.Book = GetBookById(randomId);           //newbook is of type DetailsInputViewModel which will initally be empty
            newbook.Reviews = GetReviews(randomId);         //so we mmust populate it manually with the book and it's reviews
            ChangeUserIdToName(newbook.Reviews, username);  //Display the username instead of the userId on the reviews
            return newbook;
        }
    }
}
