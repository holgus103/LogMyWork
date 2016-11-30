using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Repositories
{
    public static class RatesRepository
    {
        public static IEnumerable<KeyValuePair<object, string>> GetUserRatesAsKeyValuePair(this LogMyWorkContext context, string userID)
        {
            return context.Rates
        .Where(r => r.UserID == userID)
                    .ToList()
                    .Select(r => new KeyValuePair<object, string>(r.RateID, r.RateValue.ToString()));
        }

        public static Rate GetRateForProjectForUser(this LogMyWorkContext context, int projectID, string userID)
        {
            return context.Rates.Where(r => r.UserID == userID && r.Projects.Any(p => p.ProjectID == projectID)).FirstOrDefault();
        }
    }
}