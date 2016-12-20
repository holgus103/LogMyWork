using System.Web.Mvc;
using LogMyWork.DTO.Projects;
using LogMyWork.Models;

namespace LogMyWork.Controllers
{
    public interface IProjectsController
    {
        ActionResult Archive();
        ActionResult Create();
        ActionResult Create(ProjectCreateDTO form);
        ActionResult Details(int? id);
        ActionResult Edit(int? id);
        ActionResult GetProjects(int taskID, string userID);
        ActionResult GetUsersForProject(int projectID);
        ActionResult Index();
        ActionResult UpdateStatus(int? id, ProjectStatus status);
        ActionResult Users(int? id);
    }
}