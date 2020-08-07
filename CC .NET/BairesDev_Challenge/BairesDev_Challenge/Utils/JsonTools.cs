using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BairesDev_Challenge.Utils
{    
    public class JsonTools
    {
        private ILogger _logger;
        public JsonTools(ILogger logger)
        {
            _logger = logger;
        }

        public string SerializeObject<T>(T obj)
        {
            string result = "";
            try
            {
                result = JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return result;
        }
        
        public List<T> DeserializeObjectList<T>(string json)
        {
            List<T> result = new List<T>();
            try
            {
                result = JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<T2> ConvertLists<T1,T2>(List<T1> list1)
        {
            List<T2> result = new List<T2>();
            try
            {
                result = JsonConvert.DeserializeObject<List<T2>>(JsonConvert.SerializeObject(list1));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return result;
        }
    }
}
