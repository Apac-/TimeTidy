﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTidy.Models;

namespace TimeTidy.Persistence.Repositories
{
    public class WorkSiteRepository : IWorkSiteRepository
    {
        private readonly IApplicationDbContext _context;

        public WorkSiteRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public WorkSite Add(WorkSite workSite)
        {
            return _context.WorkSites.Add(workSite);
        }

        /// <summary>
        /// Get worksite by Id.
        /// </summary>
        /// <param name="id">Work Site Id</param>
        /// <returns>Found site or Null</returns>
        public WorkSite GetWorkSite(int id)
        {
            return _context.WorkSites.SingleOrDefault(w => w.Id == id);
        }

        public IEnumerable<WorkSite> GetWorkSites()
        {
            return _context.WorkSites.ToList();
        }

        public WorkSite Remove(WorkSite workSite)
        {
            return _context.WorkSites.Remove(workSite);
        }
    }
}