﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;

namespace TimeTidy.Services
{
    public class DbContextService : IDbContextService
    {
        private ApplicationDbContext _context;

        public DbContextService()
        {
            _context = new ApplicationDbContext();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser FindUser(string id)
        {
            return _context.Users.Single(u => u.Id == id);
        }

        public ApplicationUser FindUserOrDefault(string id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public WorkSite FindWorkSiteOrDefault(int id)
        {
            return _context.WorkSites.SingleOrDefault(w => w.Id == id);
        }

        public List<IdentityRole> Roles()
        {
            return _context.Roles.ToList();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public List<ApplicationUser> Users()
        {
            return _context.Users.ToList();
        }
    }
}