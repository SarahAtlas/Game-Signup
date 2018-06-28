using Classwork___April_16.data;
using Classwork___April_16.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Classwork___April_16.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult GameSignup()
        {
            EventManager eManager = new EventManager(Settings.Default.Constr);
            Event upcomingEvent = eManager.GetUpcomingEvent();

            var vm = new GameSignupViewModel
            {
                Event = upcomingEvent,
                EventStatus = eManager.GetEventStatus(upcomingEvent),
                Player = new Player()
            };
            if (Request.Cookies["firstName"] != null)
            {
                vm.Player.FirstName = Request.Cookies["firstName"].Value;
                vm.Player.LastName = Request.Cookies["lastName"].Value;
                vm.Player.Email = Request.Cookies["email"].Value;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Signup(Player player)
        {
            PlayerManager pManager = new PlayerManager(Settings.Default.Constr);            
            EventManager eManager = new EventManager(Settings.Default.Constr);

            HttpCookie firstNameCookie = new HttpCookie("firstName", player.FirstName);
            HttpCookie lastNameCookie = new HttpCookie("lastName", player.LastName);
            HttpCookie emailCookie = new HttpCookie("email", player.Email);
            Response.Cookies.Add(firstNameCookie);
            Response.Cookies.Add(lastNameCookie);
            Response.Cookies.Add(emailCookie);

            Event e = eManager.GetEventById(player.EventId);
            var eventStatus = eManager.GetEventStatus(e);
            if (eventStatus == EventStatus.Past)
            {
                TempData["ErrorMessage"] = "You cannot sign up to a game in the past!!";
                return Redirect("/Home/Index");
            }
            else if (eventStatus == EventStatus.Full)
            {
                TempData["ErrorMessage"] = "Nice try!";
                return Redirect("/Home/Index");
            }

            pManager.AddPlayer(player);

            

            TempData["SuccessMessage"] = $"You have signed up for the upcoming game on " +
                                          $"{e.Date.ToLongDateString()}." +
                                          $"We look foward to seeing you!";
            return Redirect("/");
        }

        public ActionResult NotificationEmail()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult NotificationEmail(string firstName, string lastName, string email)
        {
            var esManager = new EmailSignupsManager(Settings.Default.Constr);
            var ns = new EmailSignup
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
            esManager.AddEmailSignup(ns);
            TempData["SuccessMessage"] = $"You have signed up for email notifications. " +
                                         $"You will be notified whenever a new game is created.";
            return Redirect("/Home/Index");
        }
    }
}