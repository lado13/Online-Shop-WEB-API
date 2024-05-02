using Car_WEB_API.Data;
using Car_WEB_API.Helpers;
using Car_WEB_API.Interfaces;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Car_WEB_API.ViewModel.Login;
using Car_WEB_API.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly AppDBContext _appDBContext;
        private readonly IJwtService _jwtService;


        public UserController(IRepository<User> userRepository, AppDBContext appDBContext, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _appDBContext = appDBContext;
            _jwtService = jwtService;
        }




        //Shows all users of the website

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsers()
        {
            var item = await _userRepository.GetAll();
            return Ok(item);
        }



        //Shows a specific user with a specific ID

        [HttpGet("Get")]
        public async Task<IActionResult> GetUser(int id)
        {
            var item = await _userRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }




        //Removes a user from the database

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            await _userRepository.Delete(id);
            return NoContent();
        }


        //Registers a user on the site !!!
        //If you want to register with the admin role, change the role to admin in the method!!!

        [HttpPost("Register")]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (await CheckUserEmailExistAsync(user.Email))
                return BadRequest(new { Message = "User Email Already Exist!!!" });

            if (await CheckUserNameExistAsync(user.FirstName))
                return BadRequest(new { Message = "User Name Already Exist!!!" });

            user.Password = PasswordHasher.HashPassword(user.Password);
            user.Role = "User";
            user.Token = "";

            await _userRepository.Add(user);
            await _appDBContext.SaveChangesAsync();

            return Ok(new { Message = "User registered" });
        }


        //Checks if the user's email exists in the database

        private async Task<bool> CheckUserEmailExistAsync(string email)
             => await _appDBContext.Users.AnyAsync(u => u.Email == email);


        //Checks if the user's name exists in the database

        private async Task<bool> CheckUserNameExistAsync(string name)
            => await _appDBContext.Users.AnyAsync(u => u.FirstName == name);



        //Authenticates the user

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (user == null)
                return BadRequest();

            if (user == null)
                return Unauthorized();

            var existingUser = await _appDBContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
                return NotFound(new { Message = "User not found !!!" });

            if (!PasswordHasher.VerifyPassword(user.Password, existingUser.Password))
                return BadRequest(new { Message = "Password is incorrect !!!" });

            var token = _jwtService.CreateJwt(existingUser);

            return Ok(new { Token = token, Message = "Login Success" });
        }


        //Send a request "Token" to reset the user's password

        [HttpPost("request")]
        public IActionResult RequestPasswordReset(string email)
        {
            var user = _appDBContext.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound("User not found");

            var token = user.ResetPasswordToken = _jwtService.RequestJwt(user);
            var time = user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(1);

            _appDBContext.SaveChanges();
            return Ok(new { Token = token, ResetPasswordExpiry = time, Email = email, Message = "Reset token generated successfully" });
        }



        //Updates the user's password in the database

        [HttpPost("reset")]
        public IActionResult ResetPassword(string email, string token, string newPassword)
        {
            var user = _appDBContext.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound("User not found");

            if (user.ResetPasswordToken != token || user.ResetPasswordExpiry < DateTime.UtcNow)
                return BadRequest(new { Message = "Invalid or expired reset token !!!" });

            user.Password = PasswordHasher.HashPassword(newPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiry = DateTime.MinValue;

            _appDBContext.SaveChanges();
            return Ok(new { Message = "Password recovery successfully" });
        }






        //Updates user data such as name and picture

        [HttpPut("EditUserInfo")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userUpdate)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.FirstName = userUpdate.FirstName;
            user.Image = userUpdate.Image;

            await _userRepository.Update(user);
            return Ok(new { Message = "User info updated successfully" });
        }





    }

}









