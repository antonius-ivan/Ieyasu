using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Others.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;

//@PlaceNumber int,
//	@PlaceName nvarchar(50),
//	@PrizeAmount money,
//    @PrizePercentage float

namespace AIRMDataManager.Library.DataAccess
{
    public class OLDSqlConnector : IDataConnection
    {
        private const string db = "Tournaments";
        public void CreatePerson(tnm_person model)
        {
            //COMMENTED 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("@FirstName", model.FirstName);
            //    p.Add("@LastName", model.LastName);
            //    p.Add("@EmailAddress", model.EmailAddress);
            //    p.Add("@CellphoneNumber", model.CellphoneNumber);
            //    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            //    connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

            //    model.Id = p.Get<int>("@id");
            //}
        }

        // TODO - Make the CreatePrize method actually save to the database
        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">the prize information</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public void CreatePrize(mst_prize model)
        {
            //Commented 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("@PlaceNumber", model.PlaceNumber);
            //    p.Add("@PlaceName", model.PlaceName);
            //    p.Add("@PrizeAmount", model.PrizeAmount);
            //    p.Add("@PrizePercentage", model.PrizePercentage);
            //    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            //    connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

            //    model.Id = p.Get<int>("@id");

            //}


            //model.Id = 1;

            //return model;
        }

        public void CreateTeam(TeamModel model)
        {
            //COMMENTED 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("@TeamName", model.TeamName);
            //    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            //    connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

            //    model.Id = p.Get<int>("@id");

            //    foreach (PersonModel tm in model.TeamMembers)
            //    {
            //        p = new DynamicParameters();
            //        p.Add("@TeamId", model.Id);
            //        p.Add("@PersonId", tm.Id);


            //        connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);

            //    }

            //}
        }

        public void CreateTournament(TournamentModel model)
        {
            //COMMENTED 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    SaveTournament(connection, model);

            //    SaveTournamentPrizes(connection, model);

            //    SaveTournamentEntries(connection, model);

            //    SaveTournamentRounds(connection, model);

            //    TournamentLogic.UpdateTournamentResults(model);

            //    //return model;

            //}
        }

        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);

            model.Id = p.Get<int>("@id");
        }
        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (mst_prize pz in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pz.id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel tm in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            //List<List<MatchupModel>> Rounds
            //List<MatchupEntryModel> Entries

            // Loop through the rounds
            // Loop through the matchups
            // Save the matchup
            // Loop through the entries and save them

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    //p = p;
                    //matchup.Id = 1;
                    ////
                    //matchup.Id = p.Get<int>("@id");
                    matchup.Id = p.Get<dynamic>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();

                        p.Add("@MatchupId", matchup.Id);
                        if (entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }
                        if (entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }
        public List<tnm_person> GetPerson_All()
        {
            List<tnm_person> output;

            //COMMENTED 2025-03-16
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();

            //}

            output  = new List<tnm_person>();
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;

            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

            //    foreach (TeamModel Team in output)
            //    {
            //        var p = new DynamicParameters();
            //        p.Add("@TeamId", Team.Id);

            //        Team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
            //    }
            //}

            output = new List<TeamModel>();
            return output;
        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> output;

            //COMMENTED 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").ToList();
            //    var p = new DynamicParameters();

            //    foreach (TournamentModel t in output)
            //    {
            //        // Populate Prizes
            //        p = new DynamicParameters();
            //        p.Add("@TournamentId", t.Id);

            //        t.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

            //        // Populate Teams
            //        p = new DynamicParameters();
            //        p.Add("@TournamentId", t.Id);
            //        t.EnteredTeams = connection.Query<TeamModel>("dbo.spTeam_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

            //        foreach (TeamModel Team in t.EnteredTeams)
            //        {
            //            p = new DynamicParameters();
            //            p.Add("@TeamId", Team.Id);

            //            Team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
            //        }

            //        p = new DynamicParameters();
            //        p.Add("@TournamentId", t.Id);

            //        // Populate Rounds
            //        List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

            //        foreach (MatchupModel m in matchups)
            //        {
            //            p = new DynamicParameters();
            //            p.Add("@MatchupId", m.Id);

            //            // Populate Rounds
            //            m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntries_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();

            //            // populate each matchup (1 model)
            //            List<TeamModel> allTeams = GetTeam_All();

            //            if (m.Winnerid > 0)
            //            {
            //                m.Winner = allTeams.Where(x => x.Id == m.Winnerid).First();
            //            }

            //            foreach (var me in m.Entries)
            //            {
            //                if (me.TeamCompetingId > 0)
            //                {
            //                    me.TeamCompeting = allTeams.Where(x => x.Id == me.TeamCompetingId).First();
            //                }
            //                if (me.ParentMatchupId > 0)
            //                {
            //                    me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId).First();
            //                }
            //            }
            //        }
            //        // List<LIst<MatchupModel>>
            //        List<MatchupModel> currRow = new List<MatchupModel>();
            //        int currRound = 1;
            //        foreach (MatchupModel m in matchups)
            //        {
            //            if (m.MatchupRound > currRound)
            //            {
            //                t.Rounds.Add(currRow);
            //                currRow = new List<MatchupModel>();
            //                currRound += 1;
            //            }
            //            currRow.Add(m);
            //        }

            //        t.Rounds.Add(currRow);
            //    }

            //}

            output= new List<TournamentModel>();
            return output;
        }

        public void UpdateMatchup(MatchupModel model)
        {
            //COMMENTED 2025-03-16 18:14
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    var p = new DynamicParameters();
            //    if (model.Winner != null)
            //    {
            //        p.Add("@Id", model.Id);
            //        p.Add("@WinnerId", model.Winner.Id);

            //        connection.Execute("dbo.spMatchups_Update", p, commandType: CommandType.StoredProcedure);
            //    }

            //    //spMatchupEntries_Update @id @TeamCompetingId @Score

            //    foreach (MatchupEntryModel me in model.Entries)
            //    {
            //        if (me.TeamCompeting != null)
            //        {
            //            p = new DynamicParameters();
            //            p.Add("@Id", me.Id);
            //            p.Add("@TeamCompetingId", me.TeamCompeting.Id);
            //            p.Add("@Score", me.Score);

            //            connection.Execute("dbo.spMatchupEntries_Update", p, commandType: CommandType.StoredProcedure);
            //        }
            //    }
            //}


        }

        public void CompleteTournament(TournamentModel model)
        {
            //COMMENTED 2025-03-16 18:14
            //spTournaments_Complete
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("@Id", model.Id);

            //    connection.Execute("dbo.spTournaments_Complete", p, commandType: CommandType.StoredProcedure);
            //}
        }
    }
}
