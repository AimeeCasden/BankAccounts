using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using BankAccounts.Models;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            List<User> AllUsers = dbContext.Users.ToList();
            return View();
        }

        [Route("Login")]
        [HttpGet]

        public IActionResult Login()
        {
            return View("Login");
        }

        [Route("Bank/{id}")]
        [HttpGet]
        public IActionResult Bank(int id)
        {
            // taking a  list of all users and filtering out the one users that has the  one ID that is equal to the person in session. Then you can use that one person to get their first name
                // User person = dbContext.Users.SingleOrDefault(u => u.UserId == id);
                // HttpContext.Session.SetInt32("UserId", person.UserId);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return View("Index");
            }
            List<Transaction> AllTransactions = dbContext.Transactions.Include(m => m.Money).Where(u => u.UserId == id).ToList();
            ViewBag.list = AllTransactions;
            User userBalance = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            int Sum = 0;
            // foreach (var trans in AllTransactions)
            // {
            //     Sum += trans.Amount;
            // }
            ViewBag.Sum = Sum;
            ViewBag.User = dbContext.Users.FirstOrDefault(a => a.UserId == HttpContext.Session.GetInt32("UserId"));
            User person = dbContext.Users.SingleOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.User = person.FirstName;
            ViewBag.Email = HttpContext.Session.GetString("Email");

            return View("Bank");
        }

        [Route("Create")]
        [HttpPost]

        public IActionResult Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                User person = new User()
                {
                    FirstName = Request.Form["FirstName"],
                    LastName = Request.Form["LastName"],
                    Email = Request.Form["Email"],
                    Password = Request.Form["Password"]

                };
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetString("Email", newUser.Email);

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                return RedirectToAction("Bank", new { id = newUser.UserId });

            }
            else
            {
                return View("Index");
            }
        }

        [Route("CreateLogin")]
        [HttpPost]

        public IActionResult CreateLogin(LoginUser newUser)
        {
            if (ModelState.IsValid)
            {
                // Use the email to find the persons ID with that email
                System.Console.WriteLine("VALID");
                User person = dbContext.Users.SingleOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetInt32("UserId", person.UserId);
                HttpContext.Session.SetString("Email", newUser.Email);
                

                return RedirectToAction("Bank", new { id = person.UserId });
            }
            else
            {   
                System.Console.WriteLine("NOT VALID");
                return View("Login");
            }
        }
        [Route("Clear")]
        [HttpGet]

        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [Route("AddMoney")]
        [HttpPost]

        public IActionResult AddMoney(Transaction newTransaction)
        {
            newTransaction.UserId = (int)HttpContext.Session.GetInt32("UserId");
            newTransaction.Amount = Int32.Parse(Request.Form["Amount"]);
            System.Console.WriteLine(Request.Form["Amount"]);
            dbContext.Transactions.Add(newTransaction);
            dbContext.SaveChanges();

            return RedirectToAction("Bank",new{ id = (int)HttpContext.Session.GetInt32("UserId")});

            // List<Transaction> AllTransactions = dbContext.Transactions.Include(m => m.Money).Where(u => u.UserId == id).ToList();
            // ViewBag.list = AllTransactions;
        }


    }

}
