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
using Abp.Extensions;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

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

        /// <summary>
        /// Returns all books with details
        /// </summary>
        /// <returns>List Of Books</returns>
        /// <response code="200">Returns List Of Books</response>
        /// <response code="400">If the item is null</response>
        [ProducesResponseType(typeof(List<BookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> Get()
        {

            var books = (await _unitofWork.Book.All()).Include(x => x.Authors).ToList();
            if (books is null)
                return NotFound();

            var bookResponse = _mapper.Map<List<BookResponse>>(books);



            return Ok(bookResponse);
        }

        /// <summary>
        /// Returns Book conteining Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Book</returns>
        [ProducesResponseType(typeof(List<BookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(int id)
        {
            var book = (await _unitofWork.Book.All())
                .Include(x => x.Authors)
                .FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        /// <summary>
        /// Returs list of books, conteining searched word
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Books conteining searched word</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{search}/find")]
        public async Task<ActionResult<List<BookResponse>>> Find(string search)
        {
            var searchToUpper = search.ToUpper();

            var bookDbList = (await _unitofWork.Book.All())
                .Include(x=>x.Authors)
                .Where(a => a.Title.Contains(searchToUpper) || a.Authors.Any(x => x.FirstName.Contains(search)) || a.Authors.Any(x => x.LastName.Contains(search)))
                .ToList();

            if (bookDbList == null) 
                return NotFound();

            var bookrequestList = _mapper.Map<IEnumerable<BookResponse>>(bookDbList);
            return Ok(bookrequestList);
        }

        /// <summary>
        /// changes quantity of books borrow book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}/borrow")]
        public async Task<ActionResult> Borrow(int id)
        {
            var book = (await _unitofWork.Book.All())
                .Include(x => x.Authors)
                .FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            if (book.BooksQuantity <= 0)
                return BadRequest();

            book.BooksQuantity--;
            await _unitofWork.CompleteAsync();

            var bookResponse = _mapper.Map<BookResponse>(book);

            return NoContent();
        }

        /// <summary>
        /// changes quantity of books return book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}/return")]
        public async Task<ActionResult> Return(int id)
        {
            var book = (await _unitofWork.Book.All())
                .Include(x => x.Authors)
                .FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            book.BooksQuantity++;
            await _unitofWork.CompleteAsync();

            var bookResponse = _mapper.Map<BookResponse>(book);

            return NoContent();
        }


        /// <summary>
        /// Creates book entity and returns created book
        /// </summary>
        /// <param name="newbook"></param>
        /// <returns>Created book</returns>
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<BookResponse>> CreateBook(BookRequest newbook)
        {
            var book = _mapper.Map<Book>(newbook);

            book.Authors = (await _unitofWork.Author.All())
                .Where(x => newbook.AuthorIds.Contains(x.Id))
                .ToList();

            await _unitofWork.Book.Add(book);
            await _unitofWork.CompleteAsync();
            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);

        }


        /// <summary>
        /// Deletes Book enttity conteining Id
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {

            var book = await _unitofWork.Book.GetById(id);

            if (book == null) return BadRequest();

            await _unitofWork.Book.Delete(id);
            await _unitofWork.CompleteAsync();

            return Ok();
        }


    }
}
