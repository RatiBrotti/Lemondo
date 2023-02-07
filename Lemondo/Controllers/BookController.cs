using Lemondo.UnitofWork.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lemondo.UnitofWork;
using Abp.Domain.Uow;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Repository;
using Lemondo.Requestes;
using AutoMapper;
using Lemondo.ClientClass;
using Microsoft.EntityFrameworkCore;

namespace Lemondo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public BookController(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var books = (await _unitofWork.Book.GetAll())
                .Include(x => x.BookAuthors)
                .ThenInclude(x=>x.Author);

            var bookAuthors = await _unitofWork.BookAuthor.GetAll();
            var authors = await _unitofWork.Author.GetAll();

            var bookResponse = new List<BookResponse>();
            foreach (var book in books)
            {
                var bookAuthor = bookAuthors.Where(ba => ba.BookId == book.Id);
                var authorIds = bookAuthor.Select(ba => ba.AuthorId).ToList();
                var authorNames = authors.Where(a => authorIds.Contains(a.Id)).Select(a => a.FirstName + " " + a.LastName).ToList();

                bookResponse.Add(new BookResponse
                {
                    Title = book.Title,
                    Description = book.Description,
                    Image = book.Image,
                    Rating = book.Rating,
                    IsCheckedOut = book.IsCheckedOut,
                    PublicationDate = book.PublicationDate.Value,
                    AuthorId = authorIds,
                    AuthorName = authorNames,
                                        
                });
            }

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _unitofWork.Book.GetById(id);
            if (book == null) return NotFound();

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookRequest newbook)
        {
            var book = _mapper.Map<Book>(newbook);

            await _unitofWork.Book.Add(book);
            await _unitofWork.CompleteAsync();
            foreach (var item in newbook.AuthorId)
            {
                var bookAuthor = new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = item
                };
                await _unitofWork.BookAuthor.Add(bookAuthor);
            }
            await _unitofWork.CompleteAsync();
            var bookResponse = _mapper.Map<BookResponse>(newbook);

            return Ok(bookResponse);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {

            var book = await _unitofWork.Book.GetById(id);

            if (book == null) return BadRequest();

            await _unitofWork.Book.Delete(id);
            await _unitofWork.CompleteAsync();
            var bookresponse = _mapper.Map<BookResponse>(book);

            return Ok(bookresponse);
        }
        [HttpGet("find/{book}")]
        public async Task<ActionResult> Find(string book)
        {
            var bookDbList = await _unitofWork.Book.Find(a => a.Title.Contains(book));
            if (bookDbList == null) return NotFound(book);
            var bookrequestList = _mapper.Map<IEnumerable<BookResponse>>(bookDbList);
            return Ok(bookrequestList);
        }
    }
}
