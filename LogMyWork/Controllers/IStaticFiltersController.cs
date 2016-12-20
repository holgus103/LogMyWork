using System.Web.Mvc;
using LogMyWork.DTO.Filters;

namespace LogMyWork.Controllers
{
    public interface IStaticFiltersController
    {
        ActionResult Create();
        ActionResult Create(StaticFilterCreateDTO filter);
        ActionResult Delete(int? id);
        ActionResult DeleteConfirmed(int id);
        ActionResult Details(int? id);
        ActionResult Edit(int? id);
        ActionResult Index();
    }
}