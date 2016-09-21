using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WowData.Model
{
    public class WowClass
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string PowerType { get; set; }
    }

    public class WowClassLoader
    {
        private static bool _isLoaded = false;
        public List<WowClass> Classes { get; set; }

        private static readonly WowClassLoader _instance = new WowClassLoader();
        private WowClassLoader() { }

        public static WowClass GetClassById(int id)
        {
            foreach(WowClass x in _instance.Classes)
            {
                if (x.ID == id)
                    return x;
            }

            //Should never reach this. Need to build a check in
            throw new Exception("Invalid Class ID Passed");

            return null;
        }

        public static async Task LoadAllClassData()
        {
            if (_isLoaded)
                return;
            try
            {
                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync($"https://us.api.battle.net/wow/data/character/classes?locale=en_US&apikey={Helper.Constants.APIKEY}");
                var res = JsonConvert.DeserializeObject<WowClassLoader>(json);
                _instance.Classes = res.Classes;
            }
            catch(Exception ex)
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
