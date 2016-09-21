using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowData.Model
{
    class CharacterProfileData
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public int Class { get; set; }
        public int Race { get; set; }
        public int Level { get; set; }
        public string Thumbnail { get; set; }
    }
}
