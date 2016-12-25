using LogMyWork.DTO.Rates;
using System.Web.Mvc;

namespace LogMyWork.Controllers
{
    public interface IRatesController
    {
        ActionResult Create();
        ActionResult Create(RateCreateDTO rateValue);
        ActionResult Delete(int? id);
        ActionResult DeleteConfirmed(int id);
        ActionResult Index();
    }
}