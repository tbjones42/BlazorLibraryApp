using BlazorLibraryApp.Models;
using BlazorLibraryApp.Services;
using Xunit;

namespace BlazorLibraryApp.Tests
{
    public class LibraryServiceTests
    {
        [Fact]
        public void AddBook_ShouldAddBookToList()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            var service = new LibraryService(booksFile, usersFile);

            var newBook = new Book
            {
                Title = "1984",
                Author = "George Orwell",
                ISBN = "11111"
            };

            // Act
            service.AddBook(newBook);
            var books = service.GetBooks();

            // Assert
            Assert.Single(books);
            Assert.Equal("1984", books[0].Title);
            Assert.Equal("George Orwell", books[0].Author);
            Assert.Equal("11111", books[0].ISBN);
        }

        [Fact]
        public void EditBook_ShouldUpdateExistingBook()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(booksFile, new[]
            {
                "1,Old Title,Old Author,12345"
            });

            var service = new LibraryService(booksFile, usersFile);

            var updatedBook = new Book
            {
                Id = 1,
                Title = "New Title",
                Author = "New Author",
                ISBN = "99999"
            };

            // Act
            service.EditBook(updatedBook);
            var books = service.GetBooks();

            // Assert
            Assert.Single(books);
            Assert.Equal("New Title", books[0].Title);
            Assert.Equal("New Author", books[0].Author);
            Assert.Equal("99999", books[0].ISBN);
        }

        [Fact]
        public void DeleteBook_ShouldRemoveBookFromList()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(booksFile, new[]
            {
                "1,Book One,Author One,11111",
                "2,Book Two,Author Two,22222"
            });

            var service = new LibraryService(booksFile, usersFile);

            // Act
            service.DeleteBook(1);
            var books = service.GetBooks();

            // Assert
            Assert.Single(books);
            Assert.Equal(2, books[0].Id);
            Assert.Equal("Book Two", books[0].Title);
        }

        [Fact]
        public void ReadBooks_ShouldLoadBooksFromCsv()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(booksFile, new[]
            {
                "1,The Hobbit,J.R.R. Tolkien,12345",
                "2,Dune,Frank Herbert,67890"
            });

            // Act
            var service = new LibraryService(booksFile, usersFile);
            var books = service.GetBooks();

            // Assert
            Assert.Equal(2, books.Count);
            Assert.Equal("The Hobbit", books[0].Title);
            Assert.Equal("Dune", books[1].Title);
        }

        [Fact]
        public void AddUser_ShouldAddUserToList()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            var service = new LibraryService(booksFile, usersFile);

            var newUser = new User
            {
                Name = "Trevor",
                Email = "trevor@email.com"
            };

            // Act
            service.AddUser(newUser);
            var users = service.GetUsers();

            // Assert
            Assert.Single(users);
            Assert.Equal("Trevor", users[0].Name);
            Assert.Equal("trevor@email.com", users[0].Email);
        }

        [Fact]
        public void EditUser_ShouldUpdateExistingUser()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(usersFile, new[]
            {
                "1,Old Name,old@email.com"
            });

            var service = new LibraryService(booksFile, usersFile);

            var updatedUser = new User
            {
                Id = 1,
                Name = "New Name",
                Email = "new@email.com"
            };

            // Act
            service.EditUser(updatedUser);
            var users = service.GetUsers();

            // Assert
            Assert.Single(users);
            Assert.Equal("New Name", users[0].Name);
            Assert.Equal("new@email.com", users[0].Email);
        }

        [Fact]
        public void DeleteUser_ShouldRemoveUserFromList()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(usersFile, new[]
            {
                "1,Trevor,trevor@email.com",
                "2,Alice,alice@email.com"
            });

            var service = new LibraryService(booksFile, usersFile);

            // Act
            service.DeleteUser(1);
            var users = service.GetUsers();

            // Assert
            Assert.Single(users);
            Assert.Equal("Alice", users[0].Name);
        }

        [Fact]
        public void ReadUsers_ShouldLoadUsersFromCsv()
        {
            // Arrange
            string booksFile = Path.GetTempFileName();
            string usersFile = Path.GetTempFileName();

            File.WriteAllLines(usersFile, new[]
            {
                "1,Trevor,trevor@email.com",
                "2,Alice,alice@email.com"
            });

            // Act
            var service = new LibraryService(booksFile, usersFile);
            var users = service.GetUsers();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Equal("Trevor", users[0].Name);
            Assert.Equal("Alice", users[1].Name);
        }
    }
}