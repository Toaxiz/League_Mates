using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League_Mates.Models;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;

namespace League_Mates.Services
{
    public static class MatchServices
    {
        /// <summary>
        /// Function to get the participant allies of a certain summoner. 
        /// </summary>
        /// <param name="match">Match</param>
        /// <param name="summonerPuuid">Puuid of the summoner whos allies should be returned</param>
        /// <returns></returns>
        public static IEnumerable<Participant> GetAllies(this Match match, string summonerPuuid)
            => match.Info.Participants.Where(p => p.TeamId == match.Info.Participants.First(me => me.Puuid == summonerPuuid).TeamId && p.Puuid != summonerPuuid);

        /// <summary>
        /// Function to get the participant allies of a certain summoner. 
        /// </summary>
        /// <param name="match">Match</param>
        /// <param name="summoner">Summoner whos allies should be returned</param>
        /// <returns></returns>
        public static IEnumerable<Participant> GetAllies(this Match match, Summoner summoner)
            => match.GetAllies(summoner.Puuid);

        /// <summary>
        /// Function to get the allies of a certain summoner. 
        /// </summary>
        /// <param name="match">Match</param>
        /// <param name="summoner">Summoner whos allies should be returned</param>
        /// <returns>Summoner names of allies</returns>
        public static IEnumerable<string> GetAllieSummonerNames(this Match match, Summoner summoner)
            => match.GetAllies(summoner.Puuid).Select(ally => ally.SummonerName);

        /// <summary>
        /// Function to get the allies of a certain summoner. 
        /// </summary>
        /// <param name="match">Match</param>
        /// <param name="summonerPuuid">Puuid of summoner whos allies should be returned</param>
        /// <returns>Summoner names of allies</returns>
        public static IEnumerable<string> GetAllieSummonerNames(this Match match, string summonerPuuid)
            => match.GetAllies(summonerPuuid).Select(ally => ally.SummonerName);

        public static GameItem BuildGameItem(this Match match, string summonerPuuid)
        {
            var allies = match.GetAllieSummonerNames(summonerPuuid).ToArray();
            var summonerName = match.GetSummonerName(summonerPuuid);
            var time = $" {match.Info.GameCreation:dddd dd,MM,yyyy HH:mm:ss} ";
            return new GameItem() {
                Time = time,
                Searched = summonerName,
                Ally1 = allies[0],
                Ally2 = allies[1],
                Ally3 = allies[2],
                Ally4 = allies[3],
            };
        }

        public static string GetSummonerName(this Match match, string summonerPuuid)
            => match.Info.Participants.First(p => p.Puuid == summonerPuuid).SummonerName;
    }
}
