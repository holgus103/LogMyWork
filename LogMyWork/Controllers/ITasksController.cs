using System.Web.Mvc;
using LogMyWork.Consts;
using LogMyWork.ViewModels.Tasks;

namespace LogMyWork.Controllers
{
    public interface ITasksController
    {
        ActionResult Create();
        ActionResult Create(TaskCreateDTO form);
        ActionResult Delete(int? id);
        ActionResult DeleteConfirmed(int id);
        ActionResult Details(int? id);
        ActionResult Edit(int? id);
        ActionResult GetTasks(int projectID, string userID);
        ActionResult GetTasksForProject(int projectID);
        ActionResult Index();
        ActionResult UpdateStatus(int id, TaskStatus status);
    }
}