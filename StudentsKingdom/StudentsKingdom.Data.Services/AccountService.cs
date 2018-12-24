﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudentsKingdom.Common.Enums;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class AccountService : IAccountService
    {
        private SignInManager<StudentsKingdomUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IMapper mapper;

        public AccountService(SignInManager<StudentsKingdomUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task Login(StudentsKingdomUser user, bool rememberMe)
        {
            await this.signInManager.SignInAsync(user, rememberMe);
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }

        public StudentsKingdomUser GetUserByNameAndPassword(string username, string password)
        {
            return  this.signInManager.UserManager.Users.FirstOrDefault(u =>
                u.UserName == username && signInManager.CheckPasswordSignInAsync(u, password, false).Result.Succeeded);

        }

        public async Task SeedAdmin()
        {
            var adminRoleName = StudentsKingdomUserRoles.Admin.ToString();

            if (!signInManager.UserManager.Users.Any(x => x.UserName == adminRoleName))
            {
                //тря да го оправя :)
                var user = new StudentsKingdomUser
                {
                    UserName = adminRoleName,
                    Email = "admin@adm.in",
                    Character = new Character(),
                    SecurityStamp = Guid.NewGuid().ToString()
                    
                };

                signInManager.UserManager.PasswordHasher.HashPassword(user, adminRoleName);

                await signInManager.UserManager.CreateAsync(user, adminRoleName);

                await signInManager.UserManager.AddToRoleAsync(user, adminRoleName);
            }

        }

        public async Task SeedRoles()
        {
            var roles = Enum.GetValues(typeof(StudentsKingdomUserRoles));
            foreach (var role in roles)
            {
                var roleName = role.ToString();

                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }
    }
}
