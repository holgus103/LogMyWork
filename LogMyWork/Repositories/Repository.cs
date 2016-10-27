using LogMyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Repositories
{
    public class Repository
    {
        protected LogMyWorkContext parent;

        public Repository(LogMyWorkContext ctx)
        {
            this.parent = ctx;
        } 

    }
}