using AutoMapper;
using Lemondo.ClientClass;
using Lemondo.DbClasses;
using Lemondo.Requestes;
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
        private readonly IMapper _mapper;

        public UserController(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _unitofWork.User.GetAll();
            var userResponseList = _mapper.Map<IEnumerable<UserResponse>>(users);
            return Ok(userResponseList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _unitofWork.User.GetById(id);
            if (user == null) return NotFound();

            var userResponse = _mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            var userlsit = _unitofWork.User.GetAll();
            if (userlsit.Result.Count() == 0) user.IsAdmin = true;
            var chekUserForregistered = await _unitofWork.User.Find(a => a.Email == userRequest.Email.ToUpper());
            if (chekUserForregistered.Count() != 0) return Content("Already Exists");
            await _unitofWork.User.Add(user);

            await _unitofWork.CompleteAsync();
            var userResponse = _mapper.Map<UserResponse>(user);

            return CreatedAtAction("GetUser", new { userResponse.FirstName }, userResponse);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitofWork.User.GetById(id);

            if (user == null) return BadRequest();

            await _unitofWork.User.Delete(id);
            await _unitofWork.CompleteAsync();
            var userResponse = _mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }

        [HttpGet("find/{user}")]
        public async Task<ActionResult> Find(string user)
        {
            user = user.ToUpper();
            var userDbList = await _unitofWork.User.Find(a => a.FirstName.Contains(user) || a.LastName.Contains(user) || a.Email.Contains(user));
            if (userDbList == null) return NotFound(user);
            var userResponseList = _mapper.Map<IEnumerable<UserResponse>>(userDbList);
            return Ok(userResponseList);
        }
    }
}
