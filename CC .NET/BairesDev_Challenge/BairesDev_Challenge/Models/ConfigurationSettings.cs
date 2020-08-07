using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BairesDev_Challenge.Models
{
    public class ConfigurationSettings
    {
        public string JsonFilePath { get; set; }
        public string PreferredCountry { get; set; }
        public int StepNumberOfConnections { get; set; }
        public List<string> LatamCountries { get; set; }
    }
}
