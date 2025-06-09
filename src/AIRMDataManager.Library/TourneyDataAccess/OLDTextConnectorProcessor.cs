using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Others.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

// * Load the text file
// * Convert the text to list<PrizeModel>
// Find the ID
// Add the new record with the new ID (max + 1)
// Convert the prize to list<string>
// Save the list<string> to the text file

namespace AIRMDataManager.Library.DataAccess.TextHelpers
{
    public static class OLDTextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            // C:\data\TournamentTracker\PrizeModel.csv
            //return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName}";
            return "";
        }

        public static List<string> LoadFile(this string file)
        {
            
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<mst_prize> ConvertToPrizeModel(this List<string> lines)
        {
            List<mst_prize> output = new List<mst_prize>();

            foreach(string line in lines)
            {
                string[] cols = line.Split(',');
                mst_prize p = new mst_prize();
                p.id = int.Parse(cols[0]);
                p.prize_number = int.Parse(cols[1]);
                p.prize_nm = cols[2];
                p.prize_amt = decimal.Parse(cols[3]);
                p.prize_pctg = decimal.Parse(cols[4]);
                output.Add(p);

            }

            return output;
        }

        public static List<tnm_person> ConvertToPersonModels(this List<string> lines)
        {
            List<tnm_person> output = new List<tnm_person>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                tnm_person p = new tnm_person();
                p.id = int.Parse(cols[0]);
                p.person_firstname = cols[1];
                p.person_lastname = cols[2];
                p.person_email = cols[3];
                p.w_cellphone1 = cols[4];
                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines)
        {
            //id, team name, list of id separated by the pipe
            // 3, Tim's Team, 1|3|5

            List<TeamModel> output = new List<TeamModel>();
            List<tnm_person> people = new List<tnm_person>(); //GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
            // nikah further
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach(string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.id == int.Parse(id)).First());
                }
                output.Add(t);
            }
            return output;
        }
        public static List<TournamentModel> ConvertToTournamentModels (this List<string> lines)
        {
            // id = 0
            // TournamentName = 1
            // EntryFee = 2
            // EnteredTeams = 3
            // Prizes = 4
            // Rounds = 5
            //id, TournamentName, EntryFee,(id|id|id = Entered Teams), (id|id|id = Prizes), (Rounds  - id^id^id|id^id^id|id^id^id)
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = new List<TeamModel>();//GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
            List<mst_prize> prizes = new List<mst_prize>();//GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();
            List<MatchupModel> matchups = new List<MatchupModel>();// GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');

                foreach(string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                if (cols[4].Length > 0)
                {
                    string[] prizeIds = cols[4].Split('|');

                    foreach (string id in prizeIds)
                    {
                        tm.Prizes.Add(prizes.Where(x => x.id == int.Parse(id)).First());
                    } 
                }

                // Capture Rounds Information
                string[] rounds = cols[5].Split('|');

                foreach(string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<MatchupModel> ms = new List<MatchupModel>();

                    foreach (string MatchupModelTextId in msText)
                    {
                        ms.Add(matchups.Where(x => x.Id == int.Parse(MatchupModelTextId)).First());
                    }
                    tm.Rounds.Add(ms);
                }

                output.Add(tm);
            }
            return output;
        }
        public static void SaveToPrizeFile(this List<mst_prize> models)
        {
            List<string> lines = new List<string>();

            foreach (mst_prize p in models)
            {
                lines.Add($"{p.id},{p.prize_number }, {p.prize_nm},{p.prize_amt},{p.prize_pctg}");
            }

            //File.WriteAllLines(GlobalConfig.PrizesFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void SaveToPeopleFile(this List<tnm_person> models)
        {
            List<string> lines = new List<string>();

            foreach(tnm_person p in models)
            {
                lines.Add($"{p.id},{p.person_firstname},{p.person_lastname},{p.person_email},{p.w_cellphone1}");
            }

            //File.WriteAllLines(GlobalConfig.PeopleFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void SaveToTeamFiles(this List<TeamModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{  t.Id }.{ t.TeamName }.{ ConvertPeopleListToString(t.TeamMembers)}");
            }

            //File.WriteAllLines(GlobalConfig.TeamFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void SaveRoundsToFile(this TournamentModel model)
        {
            // Loop through each Round
            // Loop through each Matchup
            // Get the id for the new matchup and save the record
            // Loop through each Entry, get the id, and save it

            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel matchup in round)
                {
                    //Load all of the matchups from file
                    //Get the top id and add one
                    //Store the id
                    //Save the matchup record
                    matchup.SaveMatchupToFile();
                }
            }
        }
        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            // id = 0, TeamCompeting = 1, Score = 2, ParentMatchup = 3
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach(string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupEntryModel me = new MatchupEntryModel();
                me.Id = int.Parse(cols[0]);
                if(cols[1].Length == 0)
                {
                    me.TeamCompeting = null;
                }
                else
                {
                    me.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                }
                me.Score = double.Parse(cols[2]);

                int parentId = 0;
                if(int.TryParse(cols[3], out parentId))
                {
                    me.ParentMatchup = LookUpMatchupById(parentId);
                }
                else
                {
                    me.ParentMatchup = null;
                }
                output.Add(me);
            }

            return output;
        }
        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<string> entries = new List<string>(); //GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach (string entry in entries)
                {
                    string[] cols = entry.Split(',');

                    if(cols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
            }

            output = matchingEntries.ConvertToMatchupEntryModels();

            return output;
        }
        private static TeamModel LookupTeamById(int id)
        {
            List<string> teams = new List<string>();// GlobalConfig.TeamFile.FullFilePath().LoadFile();

            foreach (string team in teams)
            {
                //string[] cols = team.Split(',');
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();
                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeamModels().First();
                }
            }
            return null;
        }
        private static MatchupModel LookUpMatchupById(int id)
        {
            List<string> matchups = new List<string>();//GlobalConfig.MatchupFile.FullFilePath().LoadFile();

            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConvertToMatchupModels().First();
                }
            }

            return null;
            //return matchups.Where(x => x.Id == id).First();
        }
        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            //id = 0, entries=1 (pipe delimited by id), winner=2,  matchupRound=3
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.Entries = ConvertStringToMatchupEntryModels(cols[1]);
                if(cols[2].Length == 0)
                {
                    p.Winner = null;
                }
                else
                {
                    p.Winner = LookupTeamById(int.Parse(cols[2]));
                }
                p.MatchupRound = int.Parse(cols[3]);
                output.Add(p);

            }

            return output;
        }

        public static void SaveMatchupToFile(this MatchupModel matchup)
        {
            List<MatchupModel> matchups = new List<MatchupModel>();//GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            int currentID = 1;

            if (matchups.Count > 0)
            {
                currentID = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            matchup.Id = currentID;

            matchups.Add(matchup);

            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile();
            }
            //save to file
            List<string> lines = new List<string>();

            //id=0, entries =1 (pipe delimted by id), winner=2, matchupRound=3
            foreach (MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{m.Id},{ConvertMatchupEntryListToString(m.Entries)},{winner},{m.MatchupRound} ");
            }
            //File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void UpdateMatchupToFile(this MatchupModel matchup)
        {
            List<MatchupModel> matchups = new List<MatchupModel>();// GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            MatchupModel oldMatchup = new MatchupModel();

            foreach(MatchupModel m in matchups)
            {
                if(m.Id == matchup.Id)
                {
                    oldMatchup = m;
                }
            }
            matchups.Remove(oldMatchup);

            matchups.Add(matchup);

            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.UpdateEntryToFile();
            }
            //save to file
            List<string> lines = new List<string>();

            //id=0, entries =1 (pipe delimted by id), winner=2, matchupRound=3
            foreach (MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{m.Id},{ConvertMatchupEntryListToString(m.Entries)},{winner},{m.MatchupRound} ");
            }
            //File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }
        public static void SaveEntryToFile(this MatchupEntryModel entry)
        {
            List<MatchupEntryModel> entries = new List<MatchupEntryModel>();//GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();

            int currentId = 1;

            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            entry.Id = currentId;
            entries.Add(entry);

            List<string> lines = new List<string>();

            foreach (MatchupEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if(e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ e.Id},{ teamCompeting },{ e.Score },{ parent }");
            }

            //File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void UpdateEntryToFile(this MatchupEntryModel entry)
        {
            List<MatchupEntryModel> entries = new List<MatchupEntryModel>();// GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
            MatchupEntryModel oldEntry = new MatchupEntryModel();

            foreach(MatchupEntryModel e in entries)
            {
                if(e.Id == entry.Id)
                {
                    oldEntry = e;
                }
            }

            entries.Remove(oldEntry);

            entries.Add(entry);
            
            List<string> lines = new List<string>();

            foreach (MatchupEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ e.Id},{ teamCompeting },{ e.Score },{ parent }");
            }

            //File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models)
        {
            List<string> lines = new List<string>();

            foreach( TournamentModel tm in models)
            {
                lines.Add($@"{ tm.Id},{ tm.TournamentName },{ tm.EntryFee },{ ConvertTeamListToString(tm.EnteredTeams) },{ ConvertPrizeListToString(tm.Prizes) },{ ConvertRoundListToString(tm.Rounds) }");
            }

            //File.WriteAllLines(GlobalConfig.TournamentFile.FullFilePath(), lines);
            File.WriteAllLines("", lines);
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            // (Rounds  - id^id^id|id^id^id|id^id^id)
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }
            
            foreach (List < MatchupModel > r in rounds)
            {
                output += $"{ConvertMatchupListToString(r)}|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            //2|5|
            foreach (MatchupModel m in matchups)
            {
                output += $"{m.Id}^";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupEntryListToString(List<MatchupEntryModel> entries)
        {
            string output = "";

            if (entries.Count == 0)
            {
                return "";
            }

            //2|5|
            foreach (MatchupEntryModel e in entries)
            {
                output += $"{e.Id}|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
        private static string ConvertPrizeListToString(List<mst_prize> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            //2|5|
            foreach (mst_prize p in prizes)
            {
                output += $"{p.id}|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            //2|5|
            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPeopleListToString(List<tnm_person> people)
        {
            string output = "";

            if(people.Count == 0)
            {
                return "";
            }

            //2|5|
            foreach (tnm_person p in people)
            {
                output += $"{p.id}|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
