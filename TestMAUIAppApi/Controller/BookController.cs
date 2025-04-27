using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.DTO.InputDTO;
using SharedModels.Models.DTO.OutputDTO;
using TestMAUIAppApi.Data;

namespace TestMAUIAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        public async Task<ActionResult<List<BookOutputDTO>>> GetAllBooks()
        {
            var books = await (from book in _dataContext.Books.Include(b => b.Author).Include(b => b.Editorial)
                               select new BookOutputDTO
                               {
                                   Id = book.Id,
                                   Name = book.Name,
                                   PublishingYear = book.PublishingYear,
                                   AuthorId = book.Author.Id,
                                   Author = $"{book.Author.FirstName} {book.Author.LastName}",
                                   EditorialId = book.Editorial.Id,
                                   Editorial = book.Editorial.Name
                               }).ToListAsync();

            return books;

        }

        [HttpPost]
        public async Task<ActionResult<BookOutputDTO>> PostBook([FromBody] BookInputDTO book)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = await _dataContext.Authors.FindAsync(book.AuthorId);
            var editorial = await _dataContext.Editorials.FindAsync(book.EditorialId);

            Book newBook = new()
            {
                Name = book.Name,
                PublishingYear = book.PublishingYear,
                Author = author,
                Editorial = editorial
            };

            await dataContext.Books.AddAsync(newBook);
            await dataContext.SaveChangesAsync();

            BookOutputDTO newBookOutput = new()
            {
                Name = newBook.Name,
                PublishingYear = newBook.PublishingYear,
                Author = $"{newBook.Author.FirstName} {newBook.Author.LastName}",
                Editorial = newBook.Editorial.Name
            };

            return Ok(newBookOutput);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookOutputDTO>> Put(int id, [FromBody] BookInputDTO bookToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id != bookToUpdate.Id)
                return BadRequest();

            var book = await dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            var author = await _dataContext.Authors.FindAsync(bookToUpdate.AuthorId);
            var editorial = await _dataContext.Editorials.FindAsync(bookToUpdate.EditorialId);


            book.Id = bookToUpdate.Id;
            book.Name = bookToUpdate.Name;
            book.PublishingYear = bookToUpdate.PublishingYear;
            book.Author = author;
            book.Editorial = editorial;


            dataContext.Books.Update(book);
            await dataContext.SaveChangesAsync();

            BookOutputDTO newBookOutput = new()
            {
                Name = book.Name,
                PublishingYear = book.PublishingYear,
                Author = $"{book.Author.FirstName} {book.Author.LastName}",
                Editorial = book.Editorial.Name
            };

            return Ok(newBookOutput);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookOutputDTO>> Delete(int id)
        {
            var book = await dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            dataContext.Books.Remove(book);
            await dataContext.SaveChangesAsync();

            return Ok();
        }

    }
}
