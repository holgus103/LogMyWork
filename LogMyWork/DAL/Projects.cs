//using LogMyWork.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace LogMyWork.DAL
//{
//    public class DAL
//    {
//        private LogMyWorkContext parent;
//        public IQueryable<Project> GetAllProjectsForUser(string userID)
//        {
//            (from p in this.db.Projects
//             join r in this.db.ProjectRoles on p.ProjectID equals r.ProjectID
//             where r.UserID == userID
//             select p);
//        }
//    }
//}