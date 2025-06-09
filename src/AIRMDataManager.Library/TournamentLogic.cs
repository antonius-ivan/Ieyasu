using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.Tourney.Others.Models;
using AIRMDataManager.Library.Modules.Tourney.Person.Models;
using AIRMDataManager.Library.Modules.Tourney.Prize.Models;

namespace AIRMDataManager.Library
{
    public static class TournamentLogic
    {
        // Order our list randomly of teams
        // Check if it is big enough - if not, add in byes - 2*2*2*2 - 2^4
        // Create our first round of matchups
        // Create every round after that - 8 Matchups - 4 Matchups - 2 Matchups - 1 Matchup

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);

            UpdateTournamentResults(model);
        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            //Commented 2025-03-27 14:07
            //int startingRound = model.CheckCurrentRound();

            //List<MatchupModel> toScore = new List<MatchupModel>();

            //foreach (List<MatchupModel> round in model.Rounds)
            //{
            //    foreach (MatchupModel rm in round)
            //    {
            //        if (rm.Winner == null && ( rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
            //        {
            //            toScore.Add(rm);
            //        }
            //    }
            //}

            //MarkWinnerInMatchups(toScore);

            //AdvancedWinner(toScore, model);

            //toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));

            //int endingRound = model.CheckCurrentRound();

            //if(endingRound > startingRound)
            //{
            //    // Alert users
            //    //EmailLogic.SendEmail();
            //    model.AlertUsersToNewRound();
            //}


        }
        
        public static void AlertUsersToNewRound(this TournamentModel model)
        {
            int currentRoundNumber = model.CheckCurrentRound();
            List<MatchupModel> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNumber).First();

            foreach(MatchupModel matchup in currentRound)
            {
                foreach(MatchupEntryModel me in matchup.Entries)
                {
                    foreach(tnm_person p in me.TeamCompeting.TeamMembers)
                    {
                        AlertPersonToNewRound(p, me.TeamCompeting.TeamName, matchup.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
                    }
                }
            }
        }

        private static void AlertPersonToNewRound(tnm_person p, string teamName, MatchupEntryModel competitor)
        {
            if(p.person_email.Length == 0)
            {
                return;
            }

            string to = "";
            string subject = "";
            StringBuilder body = new StringBuilder();

            if (competitor != null)
            {
                subject = $"You Have a new matchup with { competitor.TeamCompeting.TeamName }";

                body.AppendLine("<h1>You have a new matchup</h1>");
                body.AppendLine("<strong>Competitor: </strong>");
                body.AppendLine(competitor.TeamCompeting.TeamName);
                body.AppendLine();
                body.AppendLine();
                body.AppendLine("Have a great time!");
                body.AppendLine("~Tournament Tracker");
            }
            else
            {
                subject = $"You have a bye week this round";

                body.AppendLine("Enjoy your round off!");
                body.AppendLine("~Tournament Tracker");
            }
            to = p.person_email;

            EmailLogic.SendEmail(to, subject, body.ToString());
        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;
            
            foreach(List<MatchupModel> round in model.Rounds)
            {
                if(round.All(x => x.Winner != null))
                {
                    output += 1;
                }
                else
                {
                    return output;
                }
            }
            // Tournament is complete
            CompleteTournament(model);

            return output - 1;
        }

        private static void CompleteTournament(TournamentModel model)
        {
            //COMMENTED 2025-03-27 14:06
            //GlobalConfig.Connection.CompleteTournament(model);
            //TeamModel winners = model.Rounds.Last().First().Winner;
            //TeamModel runnerUp = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != winners).First().TeamCompeting;

            //decimal winnerPrize = 0;
            //decimal runnerUpPrize = 0;

            //if(model.Prizes.Count > 0)
            //{
            //    decimal totalIncome = model.EnteredTeams.Count * model.EntryFee;

            //    PrizeModel FirstPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 1).FirstOrDefault();
            //    PrizeModel SecondPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 2).FirstOrDefault();
            //    if (FirstPlacePrize != null)
            //    {
            //        winnerPrize = FirstPlacePrize.CalculatePrizePayout(totalIncome);
            //    }
            //    if (SecondPlacePrize != null)
            //    {
            //        runnerUpPrize = SecondPlacePrize.CalculatePrizePayout(totalIncome);
            //    }
            //}
            //// send Emailto all Tournament
            
            //string subject = "";
            //StringBuilder body = new StringBuilder();
            
            //subject = $"In  { model.TournamentName },{ winners.TeamName } has won!";

            //body.AppendLine("<h1>We have a WINNER!</h1>");
            //body.AppendLine("<p>Congratulations to our winner on a greate tournament.</p>");
            //body.AppendLine("br /");

            //if(winnerPrize > 0)
            //{
            //    body.AppendLine($"<p>{ winners.TeamName } will receive ${ winnerPrize }</p>");
            //}

            //if (runnerUpPrize > 0)
            //{
            //    body.AppendLine($"<p>{ runnerUp.TeamName } will receive ${ runnerUpPrize }</p>");
            //}

            //body.AppendLine("Thanks for a great tournament everyone");
            //body.AppendLine("~Tournament Tracker");


            //List<string> bcc = new List<string>();

            //foreach(TeamModel t in model.EnteredTeams)
            //{
            //    foreach(PersonModel p in t.TeamMembers)
            //    {
            //        if(p.EmailAddress.Length > 0)
            //        {
            //            bcc.Add(p.EmailAddress);
            //        }
            //    }
            //}

            //EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());

            //// Complete Tournament
            //model.CompleteTournament();
        }

        private static decimal CalculatePrizePayout(this mst_prize prize, decimal totalIncome)
        {
            decimal output = 0;

            if(prize.prize_amt > 0)
            {
                output = prize.prize_amt;
            }
            else
            {
                output = Decimal.Multiply(totalIncome , Convert.ToDecimal(prize.prize_pctg / 100));
            }

            return output;
        }

        private static void AdvancedWinner(List<MatchupModel> models, TournamentModel tournament)
        {

            //foreach (MatchupModel m in models)
            //{
            //    foreach (List<MatchupModel> round in tournament.Rounds)
            //    {
            //        foreach (MatchupModel rm in round)
            //        {
            //            foreach (MatchupEntryModel me in rm.Entries)
            //            {
            //                if (me.ParentMatchup != null)
            //                {
            //                    if (me.ParentMatchup.Id == m.Id)
            //                    {
            //                        me.TeamCompeting = m.Winner;
            //                        GlobalConfig.Connection.UpdateMatchup(rm);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private static void MarkWinnerInMatchups(List<MatchupModel> models)
        {
            //CoMMENTED on 2025-03-16 18:13
            string greaterWins = "";// ConfigurationManager.AppSettings["greaterWins"];

            // 0 means false, or low score wins
            foreach (MatchupModel m in models)
            {
                if(m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue;
                }
                if (greaterWins == "0")
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if(m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application");
                    }
                }
                else
                {
                    // i mean true, or high score wins
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application");
                    }
                } 
            }
        }

        private static void CreateOtherRounds (TournamentModel model, int rounds)
        {
            int round = rounds-1;
      List<MatchupModel> previousRound = model.Rounds[0];
      //List<MatchupModel> previousRound = 
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while(round <= rounds)
            {
                foreach(MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if(currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }

                model.Rounds.Add(currRound);
                previousRound = currRound;

                currRound = new List<MatchupModel>();
                round += 1;
            }
        }
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            foreach(TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if(byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            //Math.Pow(2, rounds);
            int output = 0;
            int totalTeams = 1;

            for(int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }
        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1; //int output = 0;
            int val = 2;

            while (val <= teamCount)
            {
                // output = output + 1;
                output += 1;

                // val = val * 2
                val *= 2;
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // cards.OrderBy(a => Guid.NewGuid()).ToList();

            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
