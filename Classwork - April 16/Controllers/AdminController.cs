using Classwork___April_16.data;
using Classwork___April_16.Models;
using Classwork___April_16.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Classwork___April_16.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(Event e)
        {
            EventManager eManager = new EventManager(Settings.Default.Constr);
            eManager.AddEvent(e);
            TempData["SuccessMessage"] = $"You have created a new game on {e.Date.ToLongDateString()}!";
            var esManager = new EmailSignupsManager(Settings.Default.Constr);
            IEnumerable<EmailSignup> signups = esManager.GetEmailSignups();
            foreach (EmailSignup signup in signups)
            {
                SendNotificationEmail(signup);
            }
            return Redirect("/Admin/Index");
        }

        public ActionResult ViewHistory()
        {
            EventManager eManager = new EventManager(Settings.Default.Constr);
            return View(eManager.GetEventsWithCount());
        }

        public ActionResult ViewPlayers(int id)
        {
            var pManager = new PlayerManager(Settings.Default.Constr);
            var eManager = new EventManager(Settings.Default.Constr);
            var vm = new ViewPlayersViewModel()
            {
                Event = eManager.GetEventById(id),
                Players = pManager.GetEventPlayers(id)
            };
            return View(vm);
        }

        #region Functions 
       

        private void SendNotificationEmail(EmailSignup signup)
        {

            var fromAddress = new MailAddress("hockeygamemania@gmail.com", "Hockey Game Mania");
            var toAddress = new MailAddress(signup.Email, $"{signup.FirstName} {signup.LastName}");

            string fromPassword = "litw05hgm";
            string subject = "New Game Posted!";
            string body = $"Hey {signup.FirstName}, The admin has posted a new game, sign up now to secure a spot!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        #endregion
    }
}