using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public bool? isCheckedOut { get; set; }
        public DateTime? yearPublished { get; set; }
        public DateTime? lastCheckedOutDate { get; set; }
        public DateTime? dueDate { get; set; }
    }

    public class LibraryCollection
    {
        public static List<Book> AllBooks { get; set; } = new List<Book>();
        public static List<Book> CheckedIn { get; set; } = new List<Book>();
        public static List<Book> CheckedOut { get; set; } = new List<Book>();
    }
}

