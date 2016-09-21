using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WowData.Model
{
    public class WowRace
    {
        public int ID { get; set; }
        public string Side { get; set; }
        public string Name { get; set; }
    }

    public class WowRaceLoader
    {
        private static bool _isLoaded = false;
        public List<WowRace> Races { get; set; }

        private static readonly WowRaceLoader _instance = new WowRaceLoader();
        private WowRaceLoader() { }

        public static WowRace GetRaceById(int id)
        {
            foreach (WowRace x in _instance.Races)
            {
                if (x.ID == id)
                    return x;
            }

            //Should never reach this. Need to build a check in
            throw new Exception("Invalid Race ID Passed");

            return null;
        }

        public static async Task LoadAllRaceData()
        {
            if (_isLoaded)
                return;
            try
            {
                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync($"https://us.api.battle.net/wow/data/character/races?locale=en_US&apikey={Helper.Constants.APIKEY}");
                var res = JsonConvert.DeserializeObject<WowRaceLoader>(json);
                _instance.Races = res.Races;
            }
            catch (Exception ex)
            {
                //@TODO handle exceptions at some point
            }
            finally
            {
                _isLoaded = true;
            }
            return;
        }
    }
}
