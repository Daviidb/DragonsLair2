﻿using System;
using System.Collections.Generic;
using System.Linq;
using TournamentLib;
using System.Reflection;
using System.Text;
using System.IO;


namespace DragonsLair
{
    public class Controller
    {
        private TournamentRepo tournamentRepository = new TournamentRepo();

        public TournamentRepo GetTournamentRepository()
        {
            return tournamentRepository;
        }


        public void ShowScore(string tournamentName)
        {
            using (StreamWriter writer = new StreamWriter("C:/Users/woopi/Desktop/kode/Dragons-Lair-master/DragonsLair/TurneringResultat.txt"))
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName); // Instancere et objekt kaldet tournament som referere til  metode i repo
                List<int> points = new int[tournament.GetTeams().Count].ToList<int>(); // Opretter en liste ud fra et tomt array som har længden af teams.
                List<Team> teams = tournament.GetTeams(); // Opretter en liste ved at kalde på metoden GetTeams fra tournament classen.
                
                int countedTeams = teams.Count; // countedTeams tæller hvor mange teams der er i vores liste "teams"
                int rounds = tournament.GetNumberOfRounds(); // Tæller runder ved at kalde på metoden GetNumberOfRounds.

                for (int i = 0; i < rounds; i++) 
                {
                    List<Team> winners = tournament.GetRound(i).GetWinningTeams(); // Liste med alle vinderne
                    if (winners[0] != null) // hvis den første plads er null, betyder det at der ikke er nogle teams.
                    {
                        foreach (Team winner in winners) 
                        {
                            for (int j = 0; j < tournament.GetTeams().Count; j++) // køre igennem så mange gange som der er hold.
                            {
                                if (winner.Name == tournament.GetTeams()[j].Name) // hvis holdet er vinderen, får de et point.
                                {
                                    points[j] = points[j] + 1;
                                }
                            }
                        }
                    }

                }

                Console.WriteLine("  #####                                        ");
                Console.WriteLine(" #     # ##### # #      #      # #    #  ####  ");
                Console.WriteLine(" #         #   # #      #      # ##   # #    # ");
                Console.WriteLine("  #####    #   # #      #      # # #  # #      ");
                Console.WriteLine("       #   #   # #      #      # #  # # #  ### ");
                Console.WriteLine(" #     #   #   # #      #      # #   ## #    # ");
                Console.WriteLine("  #####    #   # ###### ###### # #    #  ####  ");

                writer.WriteLine("  #####                                        ");
                writer.WriteLine(" #     # ##### # #      #      # #    #  ####  ");
                writer.WriteLine(" #         #   # #      #      # ##   # #    # ");
                writer.WriteLine("  #####    #   # #      #      # # #  # #      ");
                writer.WriteLine("       #   #   # #      #      # #  # # #  ### ");
                writer.WriteLine(" #     #   #   # #      #      # #   ## #    # ");
                writer.WriteLine("  #####    #   # ###### ###### # #    #  ####  ");

                Console.WriteLine("0-------------------------------------------0");
                writer.WriteLine("0-------------------------------------------0");
                Console.WriteLine("Turnering: " + tournamentName);
                writer.WriteLine("Turnering: " + tournamentName);
                Console.WriteLine("Spillede runder: " + rounds);
                writer.WriteLine("Spillede runder: " + rounds);
                writer.WriteLine("Vinder af turnering: ");
                Console.WriteLine("|----------------------------| VUNDNE KAMPE |");
                writer.WriteLine("|----------------------------| VUNDNE KAMPE |");
                for (int i = 0; i < countedTeams; i++)
                {
                    int index = points.IndexOf(points.Max());
                    Console.WriteLine(PaddedText(teams[index].ToString(), 27) + " - " + PaddedText(points[index].ToString(), 13));
                    writer.WriteLine(PaddedText(teams[index].ToString(), 27) + " - " + PaddedText(points[index].ToString(), 13));

                    points.RemoveAt(index);
                    teams.RemoveAt(index);
                }
                Console.WriteLine("0-------------------------------------------0");
                writer.WriteLine("0-------------------------------------------0");
                Console.ReadLine();
            }
        }

        public void ScheduleNewRound(string tournamentName, bool printNewMatches = true)
        {
            Tournament tournament = tournamentRepository.GetTournament(tournamentName);
            tournament.SetupTestTeams(); // Bruges til at teste menuen, udkommenter ved test
            Round newRound = new Round();
            Match newMatch;
            
            List<Team> tempTeams;
            List<Team> newTeams = new List<Team>();

            int numberOfRound = tournament.GetNumberOfRounds();
            Round lastRound = null;
            Random random = new Random();
            bool isRoundFinished = true;
            Team freeRider = null;

            if (numberOfRound != 0)
            {
                numberOfRound--;
                lastRound = tournament.GetRound(numberOfRound);
                isRoundFinished = tournament.GetRound(numberOfRound).IsMatchesFinished();
            }

            if(isRoundFinished)
            {
                if (lastRound != null)
                {
                    tempTeams = new List<Team>(tournament.GetRound(numberOfRound).GetWinningTeams());
                    if(tournament.GetRound(numberOfRound).FreeRider != null)
                    {
                        tempTeams.Add(tournament.GetRound(numberOfRound).FreeRider);
                    }
                } 
                else
                {
                    tempTeams = new List<Team>(tournament.GetTeams());
                }
                
                while(tempTeams.Count >= 1)
                {
                    if(tempTeams.Count == 1)
                    {
                        freeRider = tempTeams[0];
                        tempTeams.RemoveAt(0);
                    } 
                    else
                    {
                        newMatch = new Match();

                        Team team1 = tempTeams[0];
                        tempTeams.RemoveAt(0);

                        Team team2 = tempTeams[0];
                        tempTeams.RemoveAt(0);

                        newMatch.FirstOpponent = team1;
                        newMatch.SecondOpponent = team2;
                        newTeams.Add(team1);
                        newTeams.Add(team2);
                        newRound.AddMatch(newMatch);
                    }
                }
                tournament.AddRound(newRound);
                tournament.GetRound(numberOfRound).SetFreeRider(freeRider);
            }

            if(printNewMatches == true)
            { 
                Console.WriteLine("0-------------------------------------------0");
                PrintLine("Turnering: " + tournamentName);
                PrintLine("Runde: " + numberOfRound);
                PrintLine(newTeams.Count / 2 + " kampe");
                Console.WriteLine("0-------------------------------------------0");
                for (int i = 0; i < newTeams.Count; i++)
                {
                    PrintLine(PaddedText(newTeams[i].Name, 20) + " - " + PaddedText(newTeams[i + 1].Name, 20));
                    i++;
                }
                Console.WriteLine("0-------------------------------------------0");
                Console.ReadLine();
            }
        }

        public void SaveMatch(string tournamentName, int roundNumber, string winningTeam)
        {
            Tournament tournament = tournamentRepository.GetTournament(tournamentName);
            Round r = tournament.GetRound(roundNumber);
            Match m = r.GetMatch(winningTeam);

            if (m != null)
            {
                Team w = tournament.GetTeam(winningTeam);
                m.Winner = w;
                Console.WriteLine($@"Kampen mellem '{m.FirstOpponent.ToString()}' og '{m.SecondOpponent.ToString()}' i runde {roundNumber} i turneringen '{tournamentName}' er nu afviklet. Vinderen blev '{m.Winner.ToString()}'.");
            }
            else
            {
                Console.WriteLine($@"Holdet '{winningTeam}' kan ikke være vinder i runde {roundNumber}, da holdet enten ikke deltager i runde {roundNumber} eller kampen allerede er registreret med en vinder.");
            }
        }

        public string PaddedText(string text, int length)
        {
            int runs = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < (length - text.Length) / 2; i++)
            {
                sb.Append(" ");
                runs++;
            }

            if (length > (runs * 2 + text.Length))
            {
                return sb + " " + text + sb;
            } 
            else
            {
                return sb + text + sb;
            }
        }

        public void PrintLine(string text)
        {
            Console.WriteLine("|" + PaddedText(text, 43) + "|");
        }
    }
}
