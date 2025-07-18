﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRMDataManager.Library.Modules.Tourney.Others.Models
{
  public class MatchupModel
  {
    /// <summary>
    /// the unique identifier for the prize
    /// </summary>
    public int Id { get; set; }
    public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
    /// <summary>
    /// The ID from the database that will be used to identify the winner.
    /// </summary>
    public int Winnerid { get; set; }
    public int? TournamentId { get; set; }//1
    public TeamModel Winner { get; set; }
    public int MatchupRound { get; set; }

    public string DisplayName
    {
      get
      {
        var output = "";
        foreach (var me in Entries)
        {
          if (me.TeamCompeting != null)
          {
            if (output.Length == 0)
            {
              output = me.TeamCompeting.TeamName;
            }
            else
            {
              output += $" vs. {me.TeamCompeting.TeamName}";
            }
          }
          else
          {
            output = "Matchup Not Yet Determined";
            break;
          }
        }

        return output;
      }
    }
  }
}
