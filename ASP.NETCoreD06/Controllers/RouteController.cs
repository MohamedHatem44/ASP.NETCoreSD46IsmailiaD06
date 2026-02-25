using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreD06.Controllers
{
    public class RouteController : Controller
    {
        /*------------------------------------------------------------------*/
        // GET: /Route/Index/1/John
        public IActionResult Index(string name, int id)
        {
            return Content(name + " " + id);
        }
        /*------------------------------------------------------------------*/
        [HttpGet("about/{deptId:int}")]
        public IActionResult About(int deptId)
        {
            return Content("About Us " + deptId);
        }
        /*------------------------------------------------------------------*/
        [HttpGet("about/{name:alpha}")]
        public IActionResult About(string name)
        {
            return Content("About Us " + name);
        }
        /*------------------------------------------------------------------*/
        //[HttpGet]
        //[Route("Test")]
        [HttpGet("Test")]
        public IActionResult About2()
        {
            return Content("About 2");
        }
        /*------------------------------------------------------------------*/
    }
}
