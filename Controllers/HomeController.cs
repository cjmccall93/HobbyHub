using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HobbyHub.Models;

namespace HobbyHub.Controllers
{

    public class HomeController : Controller
    {    private MyContext _context {get; set;}
        private PasswordHasher<Users> regHasher = new PasswordHasher<Users> ();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser> ();
        public HomeController (MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost ("register")]
        public IActionResult Register (Users u)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.FirstOrDefault (usr => usr.Username == u.Username) != null)
                {
                    ModelState.AddModelError ("Username", "Username is already in use, try logging in!");
                    return View ("Index");
                }
                string hash = regHasher.HashPassword (u, u.Password);
                u.Password = hash;

                _context.Users.Add (u);
                _context.SaveChanges ();
                HttpContext.Session.SetInt32 ("userId", u.UserId);
                return Redirect ("/hobby");
            }
            return View ("Index");
        }
                [HttpPost ("login")]
        public IActionResult Login (LoginUser lu,int userId)
        {
            if (ModelState.IsValid)
            {
                Users userInDB = _context.Users.FirstOrDefault (u => u.Username == lu.LoginUsername);
                if (userInDB == null)
                {
                    ModelState.AddModelError ("Username", "Invalid Username or Password");
                    return View ("Index");
                }
                var result = logHasher.VerifyHashedPassword (lu, userInDB.Password, lu.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError ("Password", "Invalid Username or Password!");
                    return View ("Index");
                }
                HttpContext.Session.SetInt32 ("userId", userInDB.UserId);
                int? usersId=HttpContext.Session.GetInt32("userId");
                return Redirect ("/hobby");
            }
            return View ("Index");
        }

        [HttpGet("hobby")]   
        public IActionResult Hobby()
        {
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Index");
            }

            List<Hobbies> AllHobbies= _context.Hobbies
            .Include(u => u.JoiningUser)
            .ThenInclude(u => u.JoiningUser)
            .Include(t => t.Creator)
            .ToList();
        return View(AllHobbies);
        }

        [HttpGet("newhobby")]
        public IActionResult NewHobby()
        {
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return View("Index");
            }
            return View("NewHobby");
        }
        [HttpPost("addhobby")]
        public IActionResult AddHobby(Hobbies hobby)
        {
            if(ModelState.IsValid)
            {
                if (_context.Hobbies.FirstOrDefault (usr => usr.Title == hobby.Title) != null)
                {
                    ModelState.AddModelError ("Title", "That hobby already exists go join the others or try a different hobby!");
                    return View ("NewHobby");
                }
                hobby.UserId =(int) HttpContext.Session.GetInt32("userId");
                HttpContext.Session.SetInt32 ("hobbyId", hobby.HobbyId);
                _context.Add(hobby);
                _context.SaveChanges();
                return Redirect("Hobby");
            }
                else
                {
                    return View("NewHobby");
                }
            }
            [HttpGet("hobby/{hId}")]
            public IActionResult Hobbies(int hId)
            {       
                if(HttpContext.Session.GetInt32("userId") == null)
            {
                return View("Index");
            }        
            int? userId = HttpContext.Session.GetInt32("userId");
            Users UserInDb = _context.Users
                .Include( o => o.FavoriteHobbies)
                .FirstOrDefault(o => o.UserId == (int) userId);
            ViewBag.User = UserInDb;
            List<Hobbies> theseHobbies= _context.Hobbies
            .Include(u => u.JoiningUser)
            .ThenInclude(u => u.JoiningUser)
            .Include(t => t.Creator)
            .Where(u => u.HobbyId == hId)
            .ToList();
            return View("Hobbies",theseHobbies);
            }
            [HttpGet("join/{hId}/{uId}")]
            public IActionResult JoinHobby(int hId,int uId)
            {
            HobbyHubAssoc joining = new HobbyHubAssoc();
            joining.HobbyId = hId;
            joining.UserId = uId;
            _context.HobbyHubAssoc.Add(joining);
            _context.SaveChanges();
            return Redirect($"/hobby/{hId}");
            
            }
            [HttpGet("cancel/{hId}/{uId}")]
        public IActionResult CancelActivity(int hId, int uId)
        {
            HobbyHubAssoc cancel =  _context.HobbyHubAssoc.FirstOrDefault( a => a.HobbyId == hId && a.UserId == uId );
            _context.HobbyHubAssoc.Remove(cancel);
            _context.SaveChanges();
            return Redirect($"/hobby/{hId}");
        }
        [HttpGet("edit/{hId}")]
        public IActionResult ChangeHobby(int hId)
        {
            Hobbies hobby = _context.Hobbies.FirstOrDefault(h => h.HobbyId == hId);
            return View(hobby);
        }
        [HttpPost("edit/{hId}/process")]
        public IActionResult Update(int hId, Hobbies d)
        {
            Hobbies hobby = _context.Hobbies.FirstOrDefault(h => h.HobbyId == hId);
            hobby.Title = d.Title;
            hobby.Description = d.Description;
            _context.SaveChanges();
            return Redirect($"/hobby/{hId}");
        }

    }
}
