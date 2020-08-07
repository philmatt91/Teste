using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BairesDev_Challenge.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult ClientList()
        {
            return View();
        }
    }
}
