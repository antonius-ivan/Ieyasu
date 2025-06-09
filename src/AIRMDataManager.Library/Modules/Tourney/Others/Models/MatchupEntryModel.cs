using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRMDataManager.Library.Modules.Tourney.Others.Models
{
  public class MatchupEntryModel
  {
    /// <summary>
    /// the unique identifier for the prize
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique identifier for the team
    /// </summary>
    /// 
    public int MatchupId { get; set; }

    /// <summary>
    /// Represents one team in the matchup
    /// </summary>

    public int TeamCompetingId { get; set; }

    /// <summary>
    /// Represents one team in the matchup
    /// </summary>

    public TeamModel TeamCompeting { get; set; }

    /// <summary>
    /// Represents the score for this particular team
    /// </summary>
    public double Score { get; set; }
    /// <summary>
    /// The unique identifier for the parent matchup (team).
    /// </summary>
    public int ParentMatchupId { get; set; }
    /// <summary>
    /// Represents the matchup that this team came
    /// from as the winner
    /// </summary>
    public MatchupModel ParentMatchup { set; get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialScore">
    /// 
    /// </param>

  }
}
