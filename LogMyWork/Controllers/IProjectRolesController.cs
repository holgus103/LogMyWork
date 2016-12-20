using System.Web.Mvc;
using LogMyWork.DTO.ProjectRoles;

namespace LogMyWork.Controllers
{
    public interface IProjectRolesController
    {
        ActionResult Create(ProjectRoleCreateDTO dto);
        ActionResult Create(int? id);
        ActionResult Delete(int? id);
        ActionResult DeleteConfirmed(int id);
    }
}