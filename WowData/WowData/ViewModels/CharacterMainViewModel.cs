using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WowData.Model;
using WowData.Helper;

namespace WowData.ViewModels
{
    class CharacterMainViewModel : INotifyPropertyChanged
    {
        private CharacterProfileData _loadedData;
        public string ToonName {
            get
            {
                return _loadedData.Name;
            }
            set
            {
                if(_loadedData.Name != value)
                {
                    _loadedData.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public WowClass ToonClass
        {
            //Class will only be set in code behind, and loaded once. Set should never be called
            get
            {
                return WowClassLoader.GetClassById(_loadedData.Class);
            }
            
        }
        /// <summary>
        /// Race should never be set beyond when the data is first loaded. Get should all be that is needed
        /// </summary>
        public WowRace Race
        {
            get
            {
                return WowRaceLoader.GetRaceById(_loadedData.Race);
            }
        }

        public int Level
        {
            get
            {
                return _loadedData.Level;
            }
            set
            {
                if (_loadedData.Level != value)
                {
                    _loadedData.Level = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ThumbnailUrl
        {
            get
            {
                //http://{locale}.battle.net/static-render/{locale}/{thumbnail}
                //http://us.battle.net/static-render/us/laughing-skull/168/109885352-avatar.jpg
                return $"http://us.battle.net/static-render/us/{ _loadedData.Thumbnail}";
            }
        }

        public string StaticRenderUrl
        {
            get
            {
                //http://render-api-<REGION>.worldofwarcraft.com/static-render/<REGION>/
                var renderImage = _loadedData.Thumbnail.Replace("avatar", "profilemain");
                return $"http://us.battle.net/static-render/us/{renderImage}";
            }
        }

        public CharacterMainViewModel(CharacterProfileData data)
        {
            _loadedData = data; //Never want this to be null
        }

        #region PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
