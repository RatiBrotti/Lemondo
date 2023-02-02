using Lemondo.UnitofWork.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lemondo.UnitofWork;
using Abp.Domain.Uow;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Repository;
using Lemondo.UnitofWork.Interface;

namespace Lemondo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;

        public BookController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _unitofWork.Book.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _unitofWork.Book.GetById(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                await _unitofWork.Book.Add(book);
                await _unitofWork.CompleteAsync();

                return CreatedAtAction("GetBook", new { book.Id }, book);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _unitofWork.Book.GetById(id);

            if (book == null)
                return BadRequest();

            await _unitofWork.Book.Delete(id);
            await _unitofWork.CompleteAsync();

            return Ok(book);
        }
    }
}
