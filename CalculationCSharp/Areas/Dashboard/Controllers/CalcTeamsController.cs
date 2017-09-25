using CalculationCSharp.Areas.Dashboard.Models;
using CalculationCSharp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Dashboard.Controllers
{
    public class CalcTeamsController : ApiController
    {
        CalculationDBContext db = new CalculationDBContext();
        // GET: api/Teams
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var response = Request.CreateResponse();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            IQueryable<CalcTeams> calcTeam = db.CalcTeams.Where(s => s.TeamOwner == user.Id);
            List<CalcTeams> CalcTeam = calcTeam.ToList();
            List<CalcTeamViewModel> CalcTeamViewModels = new List<CalcTeamViewModel>();
            List<CalcTeamMembersViewModel> CalcTeamMembersViewModels = new List<CalcTeamMembersViewModel>();
            List<CalcTeamConfigurationViewModel> CalcTeamConfigurationsViewModels = new List<CalcTeamConfigurationViewModel>();

            foreach (var team in CalcTeam)
            {
                CalcTeamViewModel CalcTeamViewModel = new CalcTeamViewModel();
                CalcTeamViewModel.CalcTeamID = team.CalcTeamID;
                CalcTeamViewModel.TeamName = team.TeamName;
                CalcTeamViewModel.TeamOwner = team.TeamOwner;
                
               if (team.CalcTeamMembers != null)
                {
                    foreach (var teamMember in team.CalcTeamMembers)
                    {
                        teamMember.CalcTeams = null;
                        CalcTeamMembersViewModel CalcTeamMembersViewModel = new CalcTeamMembersViewModel();
                        ApplicationUser TeamMember = userManager.FindByIdAsync(teamMember.UserId).Result;
                        CalcTeamMembersViewModel.CalcTeams = team.CalcTeamID;
                        CalcTeamMembersViewModel.UserId = TeamMember.UserName;
                        CalcTeamViewModel.TeamMembers.Add(CalcTeamMembersViewModel);
                    }
                }
                if (team.CalcConfigurations != null)
                {
                    foreach (var teamCalc in team.CalcConfigurations)
                    {
                        CalcTeamConfigurationViewModel CalcTeamConfigurationsViewModel = new CalcTeamConfigurationViewModel();
                        CalcTeamConfigurationsViewModel.ID = teamCalc.ID;
                        CalcTeamConfigurationsViewModel.Scheme = teamCalc.Scheme;
                        CalcTeamConfigurationsViewModel.Name = teamCalc.Name;
                        CalcTeamConfigurationsViewModel.User = teamCalc.User;
                        CalcTeamConfigurationsViewModel.Configuration = teamCalc.Configuration;
                        CalcTeamConfigurationsViewModel.UpdateDate = teamCalc.UpdateDate;
                        CalcTeamConfigurationsViewModel.Version = teamCalc.Version;
                        CalcTeamConfigurationsViewModel.CalcOwner = teamCalc.CalcOwner;
                        CalcTeamConfigurationsViewModel.CalcTeams = teamCalc.CalcTeams.CalcTeamID;
                        CalcTeamViewModel.TeamCalcs.Add(CalcTeamConfigurationsViewModel);
                        teamCalc.CalcTeams = null;
                    }
                }

                CalcTeamViewModels.Add(CalcTeamViewModel);
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(CalcTeamViewModels));
            return response;
        }
        // POST: api/Teams
        [ResponseType(typeof(CalcTeams))]
        public IHttpActionResult Post(CalcTeams CalcTeam)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            CalcTeam.TeamOwner = HttpContext.Current.User.Identity.GetUserId();
            db.CalcTeams.Add(CalcTeam);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = CalcTeam.CalcTeamID }, CalcTeam);
        }

        // PUT: api/Teams/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Teams/5
        public void Delete(int id)
        {
        }
    }
}
