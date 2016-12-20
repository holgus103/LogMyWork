using System.Web.Mvc;
using LogMyWork.DTO.Issues;

namespace LogMyWork.Controllers
{
    public interface IIssuesController
    {
        ActionResult Create();
        ActionResult Create(IssueCreateDTO dto);
        ActionResult Delete(int? id);
        ActionResult DeleteConfirmed(int id);
        ActionResult Details(int? id);
        ActionResult Edit(int? id);
        ActionResult Index();
    }
}