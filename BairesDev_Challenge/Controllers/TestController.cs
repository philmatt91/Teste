using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace BairesDev_Challenge.Controllers
{
    public class TestController : Controller
    {
        public JsonResult MyTest(string parametro)
        {
            string query = "select FIRST 100 * from TAB_PACIENTE";
            string connectionstring = @"User=SYSDBA;Password=masterkey;Database=D:\Work\TFS\cma_app\Database\CMA.FDB; " + Environment.NewLine +
            "DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=0; " + Environment.NewLine +
            "Connection timeout=15;Pooling=True;Packet Size=8192;Server Type=0";
            FbConnection conn = new FbConnection(connectionstring);   
            FbDataAdapter adapter = new FbDataAdapter(query, conn);
            DataTable dt = new DataTable();                                  
            try
            {                
                adapter.Fill(dt);
            }
            catch (Exception e)
            {

            }            

            return Json(parametro);
        }
    }
}
