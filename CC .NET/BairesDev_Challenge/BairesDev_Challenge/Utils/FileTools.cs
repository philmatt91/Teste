using BairesDev_Challenge.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BairesDev_Challenge.Utils
{
    public class FileTools
    {
        private readonly ConfigurationSettings _configSettings;
        private ILogger _logger;

        public FileTools(ConfigurationSettings configSettings,ILogger logger)
        {
            _configSettings = configSettings;
            _logger = logger;
        }

        public string ReadFile(string filePath = "")
        {
            string result = "";
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = _configSettings.JsonFilePath;
            }
            try
            {
                StreamReader file = File.OpenText(filePath);
                result = file.ReadToEnd();
                file.Close();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);                
            }
            return result;
        }        

        public void InsertText(string textWrite,string filePath = "")
        {
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = _configSettings.JsonFilePath;
            }
            string textRead = ReadFile(filePath).Trim();
            textRead = textRead.Substring(0,textRead.LastIndexOf("]"));
            textRead += ",";
            StringBuilder SB = new StringBuilder();
            SB.Append(textRead).Append(textWrite + "]");
            try
            {                
                using (StreamWriter sw = new StreamWriter(filePath))
                {                    
                    sw.Write(SB.ToString());
                    sw.Close();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception("Could not write on the file at the specified path: " + filePath);
            }
                       
        }
    }
}
