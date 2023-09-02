using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using BankSystemAssessment.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankSystemAssessment.Controllers
{
    [ApiController]
    [Route("api/BankSystem")]
    public class UserController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public UserController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetUser")]
        public IActionResult GetUsers()
        {
            if (_dbContext.Users == null) { 
                return NotFound();
            }
            return Ok(_dbContext.Users.ToList());
        }


        [HttpPost("AddUser")]
        public IActionResult AddUser(string UserName, string Password)
        {

            // Input validation
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Create a new user )
            var newUser = new User
            {
                Username = UserName,
                Password = Password // You should hash and salt the password here
            };

            // Add the user to the repository
            _dbContext.Add(newUser);
            _dbContext.SaveChanges();

            return Ok(newUser);

        }


        [HttpGet("Balance")]
        public IActionResult GetUserBalance(int UserID)
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            var user = _dbContext.Users.Find(UserID);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Balance);
        }

        [HttpPost("Withdraw")]
        public IActionResult Withdraw(int UserID, decimal Amount)
        {

            // Input validation
            if (UserID == 0 || Amount == 0)
            {
                return BadRequest("UserID and Amount is are required.");
            }

            // Validate user input and check if the user has sufficient balance
            var user = _dbContext.Users.Find(UserID);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Balance < Amount)
            {
                return BadRequest("Insufficient balance.");
            }

            // Deduct the amount from the user's balance
            user.Balance -= Amount;
            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return Ok(user.Balance);
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit(int UserID, decimal Amount)
        {

            // Input validation
            if (UserID == 0 || Amount == 0)
            {
                return BadRequest("UserID and Amount is are required.");
            }



            // Validate user input and add the amount to the user's balance
            var user = _dbContext.Users.Find(UserID);
            if (user == null)
            {
                return NotFound();
            }


            // Update the amount from the user's balance
            user.Balance += Amount;

            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return Ok(user.Balance);
        }

        [HttpPost("Transfer")]
        public IActionResult Transfer(int SenderUserID, int ReceiverUserID, decimal Amount)
        {

            // Input validation
            if (SenderUserID == 0 || ReceiverUserID == 0 || Amount == 0)
            {
                return BadRequest("Fill out required fields.");
            }

            // Validate user input, check sender's balance, and perform the transfer
            var sender = _dbContext.Users.Find(SenderUserID);
            var receiver = _dbContext.Users.Find(ReceiverUserID);

            if (sender == null || receiver == null)
            {
                return NotFound("User not found.");
            }

            if (sender.Balance < Amount)
            {
                return BadRequest("Insufficient balance for transfer.");
            }

            sender.Balance -= Amount;
            receiver.Balance += Amount;

            _dbContext.Update(sender.Balance);
            _dbContext.Update(receiver.Balance);
            _dbContext.SaveChanges();
            return Ok(receiver.Balance);

        }


    }
}
