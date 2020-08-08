using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BairesDev_Challenge.Utils;
using BairesDev_Challenge.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BairesDev_Challenge.Controllers
{
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly FileTools _fileTools;
        private readonly JsonTools _jsonTools;
        private ILogger _logger;
        private readonly ConfigurationSettings _configSettings;

        public ClientsController(ConfigurationSettings configSettings, ILogger logger)
        {
            _fileTools = new FileTools(configSettings, logger);
            _logger = logger;
            _configSettings = configSettings;
            _jsonTools = new JsonTools(logger);
        }

        private string GetPosition(int id)
        {
            List<Client> clients = SortClients();
            int index = clients.FindIndex(item => item.PersonId == id);
            if (index >= 0)
            {

                return "{\"Position\":" + (index + 1).ToString() + "}";
            }
            else
            {
                throw new Exception("The provided PersonId could not be found.");
            }
        }

        private List<Client> SortClients()
        {
            string filePath = _configSettings.JsonFilePath;
            string json = _fileTools.ReadFile(filePath);

            if (String.IsNullOrEmpty(json))
            {
                throw new Exception("Could not read the file specified in " + filePath);
            }
            else
            {
                List<Client> clients = _jsonTools.DeserializeObjectList<Client>(json);
                if (clients.Count > 0)
                {
                    /*Priorities
                     * 1)Number of Connections 
                     * (Divided by a configurable(appsettings.json file) number, so other priorities can apply)
                     * Example if divided by 50: 32 and 54 would have the same priority, but 81 would have a higher priority
                     * 
                     * 2)Number of Recommendations
                     * 3)Client being in the United States
                     * 4)Client being in Latin America
                     * 5)Number of Connections (Last Tie Breaker)             
                    */

                    try
                    {
                        return clients.OrderByDescending(item => Math.Round(((
                        item.NumberOfConnections.HasValue ? (double)item.NumberOfConnections : 0
                        ) / _configSettings.StepNumberOfConnections), MidpointRounding.ToEven))
                                .ThenByDescending(item => item.NumberOfRecommendations)
                                .ThenBy(item => item.Country.Equals(_configSettings.PreferredCountry) ? 0 : 1)
                                .ThenBy(item => _configSettings.LatamCountries.Contains(item.Country) ? 0 : 1)
                                .ThenByDescending(item => item.NumberOfConnections)
                                .ToList();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        throw new Exception("There was an error sorting possible clients");
                    }
                }
                else
                {
                    throw new Exception("Could not parse any clients on the specified file: " + filePath);
                }
            }
        }

        [Route("api/[controller]/[action]/{personId}/{firstName}/{lastName}/{currentRole}/{country}/{industry}/{numberOfRecommendations}/{numberOfConnections}")]
        public string ClientInsert(int personId, string firstName, string lastName, string currentRole,
            string country, string industry, int? numberOfRecommendations, int? numberOfConnections)
        {
            Client client;
            try
            {
                client = new Client(personId, firstName, lastName, currentRole, country, industry, numberOfRecommendations, numberOfConnections);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception("Could not create a new Client, please verify if all parameters are provided.");
            }
            string json = _jsonTools.SerializeObject<Client>(client);
            _fileTools.InsertText(json);
            return GetPosition(personId);
        }

        [Route("api/[controller]/[action]/{id?}")]
        public string ClientPosition(int? id)
        {
            if (id.HasValue)
            {
                return GetPosition((int)id);
            }
            else
            {
                throw new Exception("Please provide the client's ID");
            }
        }

        [Route("api/[controller]/[action]/{top?}")]
        public JsonResult TopClients(int? top, bool? fullData)
        {
            List<Client> clients = SortClients();
            if (top.HasValue && top > 0)
            {
                clients = clients.Take(top.Value).ToList();
                if (!(fullData.HasValue && fullData.Value))
                {
                    List<ClientId> clientIds = _jsonTools.ConvertLists<Client, ClientId>(clients);
                    return Json(clientIds);
                }
            }
            return Json(clients);
        }

        [Route("[controller]/[action]")]
        public ActionResult List()
        {
            return View();
        }
    }
}
