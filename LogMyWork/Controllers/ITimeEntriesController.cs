using System.Web.Mvc;
using LogMyWork.Models;

namespace LogMyWork.Controllers
{
    public interface ITimeEntriesController
    {
        ActionResult Create(int timeEntry);
        ActionResult GetFilteredValues(long? from, long? to, int? projectID, int? taskID, string user);
        ActionResult Index();
    }
}