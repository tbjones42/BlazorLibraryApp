using BlazorLibraryApp.Models;

namespace BlazorLibraryApp.Services
{
    public class LibraryService : ILibraryService
    {
        private List<Book> books = new();
        private List<User> users = new();
        private Dictionary<int, List<Book>> borrowedBooks = new();

        private readonly string booksFilePath;
        private readonly string usersFilePath;

        public LibraryService()
            : this(Path.Combine("Data", "Books.csv"), Path.Combine("Data", "Users.csv"))
        {
        }

        public LibraryService(string booksPath, string usersPath)
        {
            booksFilePath = booksPath;
            usersFilePath = usersPath;

            ReadBooks();
            ReadUsers();
        }

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;
        public Dictionary<int, List<Book>> GetBorrowedBooks() => borrowedBooks;

        public void ReadBooks()
        {
            books.Clear();

            if (!File.Exists(booksFilePath))
                return;

            foreach (var line in File.ReadLines(booksFilePath))
            {
                var fields = line.Split(',');

                if (fields.Length >= 4)
                {
                    books.Add(new Book
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Title = fields[1].Trim(),
                        Author = fields[2].Trim(),
                        ISBN = fields[3].Trim()
                    });
                }
            }
        }

        public void ReadUsers()
        {
            users.Clear();

            if (!File.Exists(usersFilePath))
                return;

            foreach (var line in File.ReadLines(usersFilePath))
            {
                var fields = line.Split(',');

                if (fields.Length >= 3)
                {
                    users.Add(new User
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Name = fields[1].Trim(),
                        Email = fields[2].Trim()
                    });
                }
            }
        }

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            WriteBooks();
        }

        public void EditBook(Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);

            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.ISBN = updatedBook.ISBN;
                WriteBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                books.Remove(book);
                WriteBooks();
            }
        }

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            WriteUsers();
        }

        public void EditUser(User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);

            if (user != null)
            {
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                WriteUsers();
            }
        }

        public void DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                users.Remove(user);
                borrowedBooks.Remove(id);
                WriteUsers();
            }
        }

        public bool BorrowBook(int bookId, int userId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            var user = users.FirstOrDefault(u => u.Id == userId);

            if (book == null || user == null)
                return false;

            if (!borrowedBooks.ContainsKey(userId))
            {
                borrowedBooks[userId] = new List<Book>();
            }

            borrowedBooks[userId].Add(book);
            books.Remove(book);
            WriteBooks();

            return true;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            if (!borrowedBooks.ContainsKey(userId))
                return false;

            var book = borrowedBooks[userId].FirstOrDefault(b => b.Id == bookId);

            if (book == null)
                return false;

            borrowedBooks[userId].Remove(book);
            books.Add(book);
            WriteBooks();

            return true;
        }

        private void WriteBooks()
        {
            var lines = books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
            File.WriteAllLines(booksFilePath, lines);
        }

        private void WriteUsers()
        {
            var lines = users.Select(u => $"{u.Id},{u.Name},{u.Email}");
            File.WriteAllLines(usersFilePath, lines);
        }
    }
}