namespace PCR.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using PCR.Models;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="HomeController" />.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Defines the _iEnvironment.
        /// </summary>
        private IWebHostEnvironment _iEnvironment;

        /// <summary>
        /// Defines the _repository.
        /// </summary>
        private readonly IPcr _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="pcr">The pcr<see cref="IPcr"/>.</param>
        /// <param name="iEnvironment">The iEnvironment<see cref="IWebHostEnvironment"/>.</param>
        public HomeController(IPcr pcr, IWebHostEnvironment iEnvironment)
        {

            _repository = pcr;
            _iEnvironment = iEnvironment;
        }
        public IActionResult Back()
        {
            return Redirect(Request.Headers["Referer"].ToString());
        }
        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index()
        {
            IQueryable<User> li = from s in _repository.GetAllDoctors select s;
            ViewBag.allDoctors = li;
            return View();
        }

        /// <summary>
        /// The Privacy.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
