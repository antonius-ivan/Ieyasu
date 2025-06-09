using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

namespace AIRMDataManager.Library.Modules.Tourney.Others.Models
{
    public class TournamentModel
    {
        public event EventHandler<DateTime> OnTournamentComplete;
        public int Id { get; set; }
        public string TournamentName { get; set; }

        public decimal EntryFee { get; set; }

        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        public List<mst_prize> Prizes { get; set; } = new List<mst_prize>();

        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

        public void CompleteTournament()
        {
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }
    }
}
