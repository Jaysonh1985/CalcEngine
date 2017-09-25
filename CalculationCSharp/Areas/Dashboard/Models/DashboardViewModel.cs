using CalculationCSharp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Dashboard.Models
{
    public class CalcTeamViewModel
    {
        public int CalcTeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamOwner { get; set; }
        public List<CalcTeamMembersViewModel> TeamMembers { get; set; }
        public List<CalcTeamConfigurationViewModel> TeamCalcs { get; set; }
        public CalcTeamViewModel()
        {
            TeamMembers = new List<CalcTeamMembersViewModel>();
            TeamCalcs = new List<CalcTeamConfigurationViewModel>();
        }
    };

    public class CalcTeamMembersViewModel
    {
        public int CalcTeams { get; set; }
        public string UserId { get; set; }
    };

    public class CalcTeamConfigurationViewModel
    {
        public int ID { get; set; }
        public string Scheme { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Version { get; set; }
        public string CalcOwner { get; set; }
        public int CalcTeams { get; set; }
    };
}