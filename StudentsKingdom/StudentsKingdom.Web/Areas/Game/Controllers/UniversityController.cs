﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.User;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area("Game")]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class UniversityController : Controller
    {
        [Route("/Game/University")]
        public IActionResult University()
        {
            return this.View();
        }
    }
}