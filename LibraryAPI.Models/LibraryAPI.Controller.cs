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

namespace LibraryAPI.Controller
{
    public class LibraryController
    {
        // add a Book to the collection
        static void bookToDataBase(Book booktoadd)
        {
            var connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=APILibraryDatabase;Trusted_Connection=True;";
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    // may need a foreach or for statement here.
                    cmd.CommandText = @"INSERT INTO Books VALUES (@author, @title, @genre, @isCheckedOut, @yearPublished, @lastCheckedOutDate, @dueDate)";
                    cmd.Parameters.AddWithValue("@author", booktoadd.author);
                    cmd.Parameters.AddWithValue("@title", booktoadd.title);
                    cmd.Parameters.AddWithValue("@genre", booktoadd.genre);
                    cmd.Parameters.AddWithValue("@isCheckedOut", booktoadd.isCheckedOut);
                    cmd.Parameters.AddWithValue("@yearPublished", booktoadd.yearPublished);
                    cmd.Parameters.AddWithValue("@lastCheckedOutDate", booktoadd.lastCheckedOutDate);
                    cmd.Parameters.AddWithValue("@dueDate", booktoadd.dueDate);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        // to get a list of all books with their status and data
        static List<Book> displayAllBooks(SqlConnection conn)
        {
            var rv = new List<Book>();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT author, title, genre, yearPublished, lastCheckedOutDate, dueDate, isCheckedOut FROM Books";

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader["Id"];
                    var author = reader[1];
                    var title = reader[2];
                    var genre = reader[3];
                    var isCheckedOut = reader[4];
                    var yearPublished = reader[5];
                    var lastCheckedOutDate = reader[6];
                    var dueDate = reader[7];

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
                conn.Close();
            }
            return rv;
        }

        // to get all the books that are checked out, and when the book is expected to come back
        static List<Book> displayCheckedOut(SqlConnection conn)
        {
            var rv = new List<Book>();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT author, title, lastCheckedOutDate, dueDate FROM Books WHERE isCheckedOut = 1";

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader["Id"];
                    var author = reader[1];
                    var title = reader[2];
                    var lastCheckedOutDate = reader[3];
                    var dueDate = reader[4];
                    var isCheckedOut = reader[5];

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
                conn.Close();
            }
            return rv;
        }

        // to get all the books that are available
        static List<Book> displayAvailable(SqlConnection conn)
        {
            var rv = new List<Book>();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT author, title, lastCheckedOutDate, dueDate FROM Books WHERE isCheckedOut = 0";

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader["Id"];
                    var author = reader[1];
                    var title = reader[2];
                    var lastCheckedOutDate = reader[3];
                    var dueDate = reader[4];
                    var isCheckedOut = reader[5];

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
                conn.Close();
            }
            return rv;

        }

        // Remove a Book to the collection
        // Update a Book
        // Check out a Book
    }
}
