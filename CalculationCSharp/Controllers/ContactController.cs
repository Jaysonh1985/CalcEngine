using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
namespace CalculationCSharp.Controllers
{
    public class ContactController : ApiController
    {
        /// <summary>Posts new configuration on the database.
        /// <para>calcConfiguration = Database Object entity </para>
        /// </summary>
        // POST: api/CalcConfigurations
        public async Task<IHttpActionResult> PostContactForm(JObject form)
        {
            IdentityMessage message = new IdentityMessage();
            message.Destination = "jaysonh1985@gmail.com";
            message.Body = form.ToString();
            message.Subject = "CalcSteps Query";
            EmailService EmailService = new EmailService();
            await EmailService.SendAsync(message);
            return CreatedAtRoute("DefaultApi", new { id = 1 }, form);
        }
    }
}
