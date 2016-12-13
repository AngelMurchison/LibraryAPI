using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using LibraryAPI.Models;
using System.Net;
using System.Net.Http;

namespace LibraryAPI.Services
{
    public class LibraryServices
    {
        private static string connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=APILibraryDatabase;Trusted_Connection=True;";

        public static List<Book> getAllBooks()
        {
            
            var rv = new List<Book>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, author, title, genre, yearPublished, lastCheckedOutDate, dueDate, isCheckedOut FROM Books";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var author = reader[1];
                        var title = reader[2];
                        var genre = reader[3];
                        var yearPublished = reader[4];
                        var lastCheckedOutDate = reader[5];
                        var dueDate = reader[6];
                        var isCheckedOut = reader[7];

                        var book = new Book
                        {
                            Id = (int)id,
                            author = author as string,
                            title = title as string,
                            genre = genre as string,
                            yearPublished = yearPublished as DateTime?,
                            lastCheckedOutDate = lastCheckedOutDate as DateTime?,
                            dueDate = dueDate as DateTime?,
                            isCheckedOut = isCheckedOut as bool?,
                        };
                        rv.Add(book);
                    }
                    connection.Close();
                }
                LibraryCollection.AllBooks.Equals(rv);
                return rv;
            }
        }

        public static List<Book> getCheckedOut()
        {
            var rv = new List<Book>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, author, title, lastCheckedOutDate, dueDate FROM Books WHERE isCheckedOut = 1";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var author = reader[1];
                        var title = reader[2];
                        var lastCheckedOutDate = reader[3];
                        var dueDate = reader[4];
                        var isCheckedOut = true;

                        var book = new Book
                        {
                            Id = (int)id,
                            author = author as string,
                            title = title as string,
                            lastCheckedOutDate = lastCheckedOutDate as DateTime?,
                            dueDate = dueDate as DateTime?,
                            isCheckedOut = isCheckedOut as bool?,
                        };
                        rv.Add(book);
                    }
                    connection.Close();
                }
                LibraryCollection.CheckedOut.Equals(rv);
                return rv;
            }
        }

        public static List<Book> getCheckedIn()
        {
            var rv = new List<Book>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, author, title, lastCheckedOutDate, dueDate FROM Books WHERE isCheckedOut = 0";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var author = reader[1];
                        var title = reader[2];
                        var lastCheckedOutDate = reader[3];
                        var dueDate = reader[4];
                        var isCheckedOut = false;

                        var book = new Book
                        {
                            Id = (int)id,
                            author = author as string,
                            title = title as string,
                            lastCheckedOutDate = lastCheckedOutDate as DateTime?,
                            dueDate = dueDate as DateTime?,
                            isCheckedOut = isCheckedOut as bool?,
                        };
                        rv.Add(book);
                    }
                    connection.Close();
                }
                LibraryCollection.CheckedIn.Equals(rv);
                return rv;

            }
        }

        public static List<Book> addABook(string author, string title, string genre, DateTime yearPublished)
        {
            Book booktoadd = new Book
            {
                author = author,
                title = title,
                genre = genre,
                isCheckedOut = false,
                yearPublished = yearPublished.Date,
                lastCheckedOutDate = DateTime.Today,
                dueDate = DateTime.Today,
            };
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Books VALUES (@author, @title, @genre, @yearPublished, @lastCheckedOutDate, @dueDate,  @isCheckedOut)";
                    cmd.Parameters.AddWithValue("@author", booktoadd.author);
                    cmd.Parameters.AddWithValue("@title", booktoadd.title);
                    cmd.Parameters.AddWithValue("@genre", booktoadd.genre);
                    cmd.Parameters.AddWithValue("@yearPublished", booktoadd.yearPublished);
                    cmd.Parameters.AddWithValue("@lastCheckedOutDate", booktoadd.lastCheckedOutDate);
                    cmd.Parameters.AddWithValue("@dueDate", booktoadd.dueDate);
                    cmd.Parameters.AddWithValue("@isCheckedOut", booktoadd.isCheckedOut);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
                List<Book> books = getAllBooks();
                return books;
            }
        }

        public static List<Book> removeBook(int id)
        {
            var ids = new List<int>();
            List<Book> books = getAllBooks();
            foreach (var book in books) { ids.Add(book.Id); }

            if (ids.Contains(id))
            {
                using (var connection = new SqlConnection(connectionStrings))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = $@"DELETE FROM Books WHERE Id={id}";


                        connection.Open();
                        var reader = cmd.ExecuteReader();
                        connection.Close();
                    }
                }
            }
            books = getAllBooks();
            return books;
        }

        public static List<Book> updateBook(string author, string title, string genre, int id)
        {
            //var authors = new List<string>();
            //var titles = new List<string>();
            //var genres = new List<string>();
            var ids = new List<int>();

            List<Book> books = LibraryCollection.AllBooks;
            foreach (var book in books)
            {
                //authors.Add(book.author);
                //titles.Add(book.title);
                //genres.Add(book.genre);
                ids.Add(book.Id);
            }

            if (ids.Contains(id) /*&& authors.Contains(author) != true*/)
            {
                using (var connection = new SqlConnection(connectionStrings))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = $@"UPDATE Books SET author='{author}', title='{title}', genre='{genre}' WHERE Id={id};";

                        connection.Open();
                        var reader = cmd.ExecuteReader();
                        connection.Close();
                    }
                }
            }
            LibraryCollection.AllBooks.Equals(books);
            return books;
        }

        public static List<Book> checkOutBook(int id)
        {
            bool? ischeckedout = null;
            var idandcheckedstatus = new Dictionary<int, bool?>();
            List<Book> books = getAllBooks();
            foreach (var book in books) { idandcheckedstatus.Add(book.Id, book.isCheckedOut); }

            if (idandcheckedstatus.Keys.Contains(id))
            {
                idandcheckedstatus.TryGetValue(id, out ischeckedout);
            }
            else
            {
                
            }

            if (ischeckedout != null && ischeckedout.Value.Equals(true))
            {

            }
            else
            {
                var dueDate = DateTime.Today.AddDays(10).Date;
                using (var connection = new SqlConnection(connectionStrings))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = $@"UPDATE Books SET isCheckedOut='1', dueDate='{dueDate.Year}-{dueDate.Month}-{dueDate.Day}', lastCheckedOutDate='{DateTime.Today}' WHERE Id={id};";

                        connection.Open();
                        var reader = cmd.ExecuteReader();
                        connection.Close();
                    }
                }
            }
            books = getAllBooks();
            return books;
        }
       
        public static List<Book> checkInBook(int id)
        {
            bool? ischeckedin = null;
            var idandcheckedstatus = new Dictionary<int, bool?>();
            List<Book> books = getAllBooks();
            foreach (var book in books) { idandcheckedstatus.Add(book.Id, book.isCheckedOut); }

            if (idandcheckedstatus.Keys.Contains(id))
            {
                idandcheckedstatus.TryGetValue(id, out ischeckedin);
            }
            if (ischeckedin != null && ischeckedin.Value.Equals(true))
            {
            using (var connection = new SqlConnection(connectionStrings))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = $@"UPDATE Books SET isCheckedOut='0', dueDate=NULL WHERE Id={id};";

                        connection.Open();
                        var reader = cmd.ExecuteReader();
                        connection.Close();
                        books = getAllBooks();
                    }
                }
            }
            return books;
        }
    }
}


