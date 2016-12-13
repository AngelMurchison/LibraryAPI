using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LibraryAPI.Models;
using LibraryAPI.Services;


namespace LibraryAPI.Controllers
{
    public class BookController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var books = LibraryServices.getAllBooks();
            return Ok(books);
        }

        [HttpGet]
        public IHttpActionResult GetCheckedIn()
        {
            var books = LibraryServices.getCheckedIn();
            return Ok(books);
        
        }

        [HttpGet]
        public IHttpActionResult GetCheckedOut()
        {
            var books = LibraryServices.getCheckedOut();
            return Ok(books);
        }

        [HttpPost]
        public IHttpActionResult Add(string author, string title, string genre, DateTime yearPublished)
        {
            var books = LibraryServices.addABook(author, title, genre, yearPublished);
            return Ok(books);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var books = LibraryServices.removeBook(id);
            return Ok(books);
        }

        [HttpPut]
        public IHttpActionResult Update(string author, string title, string genre, int id)
        {
            var books = LibraryServices.updateBook(author, title, genre, id);
            return Ok(books);
        }

        [HttpPut]
        public IHttpActionResult checkOut(int id)
        {
            var books = LibraryServices.checkOutBook(id);
            return Ok(books);
        }

        [HttpPut]
        public IHttpActionResult checkIn(int id)
        {
            var books = LibraryServices.checkInBook(id);
            return Ok(books);
        }
    }
}