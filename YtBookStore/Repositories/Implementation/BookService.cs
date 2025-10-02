using YtBookStore.Models.Domain;
using YtBookStore.Repositories.Abstract;

namespace YtBookStore.Repositories.Implementation
{
    public class BookService : IBookService
    {
        private readonly DatabaseContext context;
        public BookService(DatabaseContext context)
        {
            this.context = context;
        }
        public bool Add(Book model)
        {
            try
            {
                context.Book.Add(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.FindById(id);
                if (data == null)
                    return false;
                context.Book.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Book FindById(int id)
        {
            return context.Book.Find(id);
        }

        public IEnumerable<Book> GetAll()
        {
            var data = (from book in context.Book
                        join author in context.Author
                        on book.AuthorId equals author.Id
                        join publisher in context.Publisher on book.PubhlisherId equals publisher.Id
                        join genre in context.Genre on book.GenreId equals genre.Id
                        select new Book
                        {
                            Id = book.Id,
                            AuthorId = book.AuthorId,
                            GenreId = book.GenreId,
                            Isbn = book.Isbn,
                            PubhlisherId = book.PubhlisherId,
                            Title = book.Title,
                            TotalPages = book.TotalPages,
                            GenreName = genre.Name,
                            AuthorName = author.AuthorName,
                            PublisherName = publisher.PublisherName
                        }
                        ).ToList();

            // You can also achieve the same with this LINQ syntax. Which is shorter and human readable. But make sure match your Genre,Publisher,Author and Book models with mine before doing so.
            // 
            //var data = context.Book
            //                 .Include(book => book.Publisher)
            //                 .Include(book => book.Author)
            //                 .Include(book => book.Genre)
            //                 .Select(book => new Book
            //                 {
            //                     Id = book.Id,
            //                     AuthorId = book.AuthorId,
            //                     GenreId = book.GenreId,
            //                     Isbn = book.Isbn,
            //                     PubhlisherId = book.PubhlisherId,
            //                     Title = book.Title,
            //                     TotalPages = book.TotalPages,
            //                     GenreName = book.Genre.Name,
            //                     AuthorName = book.Author.AuthorName,
            //                     PublisherName = book.Publisher.PublisherName
            //                 });

            return data;
        }

        public bool Update(Book model)
        {
            try
            {
                context.Book.Update(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
