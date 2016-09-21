using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WowData.Model;

namespace WowData.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _realmInfo;
        private string _toonName;
        private Task _loadStatus;

        private bool _busy = false;
        public bool IsBusy
        {
            get
            {
                return _busy;
            }
            set
            {
                if (_busy == value)
                    return;
                _busy = value;
                OnPropertyChanged();
            }
        }

        public string RealmInfo
        {
            get
            {
                return _realmInfo;
            }
            set
            {
               if(_realmInfo != value)
                {
                    _realmInfo = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsDataEntered");
                }
            }
        }
        public string ToonName
        {
            get
            {
                return _toonName;
            }
            set
            {
                if(_toonName != value)
                {
                    _toonName = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsDataEntered");
                }
            }
        }
    
        public bool IsDataEntered
        {
            get
            {
                return !String.IsNullOrEmpty(_realmInfo) && !String.IsNullOrEmpty(_toonName);
            }
        }

        public LoginViewModel()
        {
            List<Task> allTasks = new List<Task>();
            allTasks.Add(WowClassLoader.LoadAllClassData());
            allTasks.Add(WowRaceLoader.LoadAllRaceData());
            _loadStatus = Task.WhenAll(allTasks);
        }

        internal async Task<CharacterProfileData> LoadMainCharacterDataAsync(string realm, string toonName)
        {
            //already loading and performing Task
            if (IsBusy)
                return null;

            CharacterProfileData _loadedData = null;
            try
            {
                IsBusy = true;
                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync($"https://us.api.battle.net/wow/character/{realm}/{toonName}?locale=en_US&apikey={Helper.Constants.APIKEY}");

                _loadedData = JsonConvert.DeserializeObject<CharacterProfileData>(json);

                //Wait out the loading of Race/Class data if still doing so
                await _loadStatus;
            }
            catch (HttpRequestException ex)
            {
                //This is fine, we display a message notice for bad data
            }
            catch (Exception ex)
            {
                //@TODO Need to handle other exceptions, maybe log them via HockeyApp?
                var str = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }

            return _loadedData;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
