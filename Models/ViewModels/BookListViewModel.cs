using System;

namespace BookCave.Models.ViewModels
{
    public class BookListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public DateTime ReleaseDate { get; internal set; }
        public string Genre { get; internal set; }
    }
}