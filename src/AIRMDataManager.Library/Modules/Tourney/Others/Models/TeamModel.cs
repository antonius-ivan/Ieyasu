using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;

namespace AIRMDataManager.Library.Modules.Tourney.Others.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<tnm_person> TeamMembers { get; set; } = new List<tnm_person>();

    }
}
