using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemondo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly IUnitofWork _unitofWork;

        public AuthorController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _unitofWork.Author.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var author = await _unitofWork.Author.GetById(id);

            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Author author)
        {
            if (ModelState.IsValid)
            {
                await _unitofWork.Author.Add(author);
                await _unitofWork.CompleteAsync();

                return CreatedAtAction("GetBook", new {author.Id }, author);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _unitofWork.Author.GetById(id);

            if (author == null)
                return BadRequest();

            await _unitofWork.Author.Delete(id);
            await _unitofWork.CompleteAsync();

            return Ok(author);
        }
    }
}
