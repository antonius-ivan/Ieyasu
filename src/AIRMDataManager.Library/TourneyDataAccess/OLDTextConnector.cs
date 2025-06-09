using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.DataAccess.TextHelpers;
using AIRMDataManager.Library;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Others.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;

namespace AIRMDataManager.Library.DataAccess
{
    public class OLDTextConnector : IDataConnection
    {

        public void CreatePerson(tnm_person model)
        {
            List<tnm_person> people = new List<tnm_person>();//GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if(people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.id).First().id + 1;
            }

            model.id = currentId;

            people.Add(model);

            people.SaveToPeopleFile();
            
        }


        // TODO - Wire up the CreatePrize for text files.
        public void CreatePrize(mst_prize model)
        {
            // Load the text file
            // Convert the text to list<PrizeModel>
            List<mst_prize> prizes = new List<mst_prize>();//GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            // Find the ID
            int currentID = 1;
            if(prizes.Count > 0)
            {
                currentID = prizes.OrderByDescending(x => x.id).First().id + 1;
            }
                
            model.id = currentID;

            // Add the new record with the new ID (max + 1)
            prizes.Add(model);


            // Convert the prize to list<string>
            // Save the list<string> to the text file
            prizes.SaveToPrizeFile();
            

        }

        public List<tnm_person> GetPerson_All()
        {
            return new List<tnm_person>(); //GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public void CreateTeam(TeamModel model)
        {
            // Load the text file
            // Convert the text to list<PrizeModel>
            //List<PrizeModel> teams = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            List<TeamModel> teams = new List<TeamModel>();// GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

            // Find the ID
            int currentID = 1;
            if (teams.Count > 0)
            {
                currentID = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            teams.Add(model);

            teams.SaveToTeamFiles();
            
        }

        public List<TeamModel> GetTeam_All()
        {
            return new List<TeamModel>();// GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> Tournaments = new List<TournamentModel>();
            //List<TournamentModel> Tournaments = GlobalConfig.TournamentFile
            //    .FullFilePath()
            //    .LoadFile()
            //    .ConvertToTournamentModels();

            int currentID = 1;

            if (Tournaments.Count > 0)
            {
                currentID = Tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentID;

            model.SaveRoundsToFile();

            Tournaments.Add(model);

            Tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model);
        }

        public List<TournamentModel> GetTournament_All()
        {
            return new List<TournamentModel>();
            //return GlobalConfig.TournamentFile
            //    .FullFilePath()
            //    .LoadFile()
            //    .ConvertToTournamentModels();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }

        public void CompleteTournament(TournamentModel model)
        {
            List < TournamentModel > Tournaments = new List<TournamentModel>();
            //List<TournamentModel> Tournaments = GlobalConfig.TournamentFile
            //        .FullFilePath()
            //        .LoadFile()
            //        .ConvertToTournamentModels();

            Tournaments.Remove(model);

            Tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model);
        }
    }
}
