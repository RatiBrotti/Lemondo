using AutoMapper;
using Lemondo.ClientClass;
using Lemondo.DbClasses;
using Lemondo.Requestes;
using Lemondo.UnitofWork.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemondo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public AuthorController(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var authors = await _unitofWork.Author.GetAll();
            var authorResponseList = _mapper.Map<IEnumerable<AuthorResponse>>(authors).AsQueryable();
            return Ok(authorResponseList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _unitofWork.Author.GetById(id);
            if (author == null) return NotFound();

            var authorResponse= _mapper.Map<AuthorResponse>(author);

            return Ok(authorResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuhtor(AuthorRequest authorRequest)
        {
            var author = _mapper.Map<Author>(authorRequest);

            await _unitofWork.Author.Add(author);

            await _unitofWork.CompleteAsync();
            var authorResponse = _mapper.Map<AuthorResponse>(author);

            return CreatedAtAction("GetAuthor", new { authorResponse.FirstName }, authorResponse);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _unitofWork.Author.GetById(id);

            if (author == null) return BadRequest();

            await _unitofWork.Author.Delete(id);
            await _unitofWork.CompleteAsync();
            var authorrequest = _mapper.Map<AuthorResponse>(author);

            return Ok(authorrequest);
        }

        [HttpGet("find/{author}")]
        public async Task<ActionResult> Find(string author)
        {
            author= author.ToUpper();
            var authorDbList = await _unitofWork.Author.Find(a => a.FirstName.Contains(author) || a.LastName.Contains(author));
            if (authorDbList == null) return NotFound(author);
            var authorrequestList = _mapper.Map<IEnumerable<AuthorResponse>>(authorDbList);
            return Ok(authorrequestList);
        }

    }
}
