using MailSending.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MailSending.Models;
namespace MailSending.Controllers
{
    public class HomeController : Controller
    { 

        public IConfiguration Configuration { get; set; }

        public HomeController(IConfiguration _configuration)
        {
            this.Configuration = _configuration;
        }

        // GET: Home
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(EmailModel model)
        {
          //  string host = "smtp.gmail.com";//this.Configuration.GetValue<string>("Smtp:Server");
           // int port = 587;// this.Configuration.GetValue<int>("Smtp:Port");
           // bool enableSsl = true; // this.Configuration.GetValue<bool>("Smtp:EnableSsl");
          //  bool defaultCredentials = false; // this.Configuration.GetValue<bool>("Smtp:DefaultCredentials");

            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                if (model.Attachment != null)
                {
                    string fileName = Path.GetFileName(model.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
                }
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
