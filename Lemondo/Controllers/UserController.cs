using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemondo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUnitofWork _unitofWork;

        public UserController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _unitofWork.User.GetAll();
            return Ok(User);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var book = await _unitofWork.User.GetById(id);

            if (book == null)
                return NotFound();

            return Ok(User);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(User user)
        {
            if (ModelState.IsValid)
            {
                await _unitofWork.User.Add(user);
                await _unitofWork.CompleteAsync();

                return CreatedAtAction("GetBook", new { user.Id }, user);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitofWork.User.GetById(id);

            if (user == null)
                return BadRequest();

            await _unitofWork.User.Delete(id);
            await _unitofWork.CompleteAsync();

            return Ok(User);
        }
    }
}
