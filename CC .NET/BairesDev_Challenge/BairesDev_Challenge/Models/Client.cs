using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BairesDev_Challenge.Models
{
    [Serializable]
    public class Client : ClientId
    {
        //public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentRole { get; set; }
        public string Country { get; set; }
        public string Industry { get; set; }
        public int? NumberOfRecommendations { get; set; }
        public int? NumberOfConnections { get; set; }

        public Client(long personId,string firstName,string lastName
            ,string currentRole,string country,string industry,int? numberOfRecommendations,int? numberOfConnections) : base(personId)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            CurrentRole = currentRole;
            Country = country;
            Industry = industry;
            NumberOfRecommendations = numberOfRecommendations;
            NumberOfConnections = numberOfConnections;
        }
    }

    [Serializable]
    public class ClientId
    {
        public long PersonId { get; set; }

        public ClientId(long personId)
        {
            PersonId = personId;
        }
    }
}
