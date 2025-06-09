using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Others.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

namespace AIRMDataManager.Library.DataAccess
{
    public interface OLDIDataConnection
    {
        void CreatePrize(mst_prize model);
        void CreatePerson(tnm_person model);
        void CreateTeam(TeamModel model);
        void CreateTournament(TournamentModel model);

        void UpdateMatchup(MatchupModel model);

        void CompleteTournament(TournamentModel model);

        List<TeamModel> GetTeam_All();
        List<tnm_person> GetPerson_All();
        List<TournamentModel> GetTournament_All();


    }
}
