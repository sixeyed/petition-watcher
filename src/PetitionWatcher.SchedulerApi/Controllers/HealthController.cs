using Microsoft.AspNetCore.Mvc;

namespace PetitionWatcher.SchedulerApi.Controllers
{
    [Route("health")]
    public class HealthController : Controller
    {
        [HttpGet]
        [Route("check")]
        public ActionResult GetCheck()
        {
            return Ok();
        }
    }
}