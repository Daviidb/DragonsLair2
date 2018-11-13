using System.Collections.Generic;

namespace TournamentLib
{
    public class Round
    {
        private List<Match> matches = new List<Match>();
        public Team FreeRider { get; set; }

        public void AddMatch(Match m)
        {
            matches.Add(m);
        }

        public Match GetMatch(string teamName1, string teamName2)
        {
            Match getMatch = matches[0];
            for (int i = 0; i < matches.Count; i++)
            {
                if(teamName1 == matches[i].FirstOpponent.ToString() && 
                    teamName2 == matches[i].SecondOpponent.ToString())
                {
                    getMatch = matches[i];
                }
            }
            return getMatch;
        }

        public Match GetMatch(string team)
        {
            Match match = null;

            for (int i = 0; i < matches.Count; i++)
            {
                if (team == matches[i].FirstOpponent.ToString() ||
                    team == matches[i].SecondOpponent.ToString())
                {
                    match = matches[i];
                }
            }
            return match;
        }

        public bool IsMatchesFinished()
        {
            bool finished = false;

            for (int i = 0; i < matches.Count; i++)
            {
                if(matches[i].Winner != null)
                {
                    finished = true;
                }
            }

            return finished;
        }

        public List<Team> GetWinningTeams()
        {
            List<Team> winningTeams = new List<Team>();
            for (int i = 0; i < matches.Count; i++)
            {
                winningTeams.Add(matches[i].Winner);
            }
            return winningTeams;
        }

        public List<Team> GetLosingTeams()
        {
            List<Team> losingTeams = new List<Team>();
            for (int i = 0; i < matches.Count; i++)
            {
                if(matches[i].Winner == matches[i].FirstOpponent)
                {
                    losingTeams.Add(matches[i].SecondOpponent);
                }
                else if(matches[i].Winner == matches[i].SecondOpponent)
                {
                    losingTeams.Add(matches[i].FirstOpponent);
                }
            }
            return losingTeams;
        }

        public void SetFreeRider(Team newFreeRider)
        {
            FreeRider = newFreeRider;
        }
    }
}
