using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PCR.Controllers
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PCR.Models;
    using PCR.Models.ViewModels;
    using SautinSoft.Document;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    [Authorize(Roles = "Doctor")]
    /// <summary>
    /// Defines the <see cref="DoctorController" />.
    /// </summary>
    public class DoctorController : Controller
    {
        /// <summary>
        /// Defines the _userManager which is provides the APIs for managing user in a persistence store...
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;


        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _repositoryEnv;

        /// <summary>
        /// Defines the idTest.
        /// </summary>
        private static string idTest = "";

        /// <summary>
        /// Defines the _repository.
        /// </summary>
        private readonly IPcr _repository;

        /// <summary>
        /// Defines the _roleManager; Provides the APIs for managing roles in a persistence store...
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Defines the _protector.
        /// </summary>
        private readonly IDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorController"/> class.
        /// </summary>
        /// <param name="userManager">The userManager<see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{IdentityRole}"/>.</param>
        /// <param name="pcr">The pcr<see cref="IPcr"/>.</param>
        /// <param name="provider">The provider<see cref="IDataProtectionProvider"/>.</param>
        public DoctorController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IPcr pcr, IDataProtectionProvider provider, IConfiguration config, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repositoryEnv = env;
            _config = config;
            _repository = pcr;
            _protector = provider.CreateProtector("PCR.Models.Report.secret");
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> Index()
        {
            //  var n = await _repository.SamplesPdfs.ToListAsync();

            IQueryable<SamplesPdf> li = from s in _repository.SamplesPdfs select s;
            ViewBag.all = li;
            return View();
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <param name="samples">The samples<see cref="SamplesPdf"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> Index(SamplesPdf samples)
        {

            var i = await DownloadFileFtp(samples.SampleName);
            byte[] myByteArray = i;
            string base64BinaryStr = Convert.ToBase64String(myByteArray);
            byte[] byteInfo = Convert.FromBase64String(base64BinaryStr);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteInfo, 0, byteInfo.Length);
            pdfStream.Position = 0;

            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// The AddResult.
        /// </summary>
        /// <param name="SampleName">The SampleName<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public IActionResult AddResult(string SampleName)
        {
            var n = _repository.SamplesPdfs;

            ViewBag.ResultInfo = n;

            SamplesPdf Product = new SamplesPdf()
            {
                SampleName = SampleName
            };

            return View(n);
        }


        /// <summary>
        /// The AllDoctors.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [AllowAnonymous]
        public IActionResult AllDoctors()
        {
            //   var t = _repository.GetAllDoctors;
            IQueryable<User> li = from s in _repository.GetAllDoctors select s;
            ViewBag.allDoctors = li;
            return View();
        }

        /// <summary>
        /// The AddResult.
        /// </summary>
        /// <param name="samples">The samples<see cref="SamplesPdf"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost]
        public IActionResult AddResult(SamplesPdf samples)
        {

            var test = samples.SampleName;
            var id = samples.Reportid;
            TempData["RID"] = id;
            idTest = id;

            TempData["Modi"] = test;
            return RedirectToAction("PatientInfo");
        }

        /// <summary>
        /// The PatientInfoAsync.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> PatientInfoAsync(int id)
        {
            var n = await _repository.SamplesPdfs.ToListAsync();
            var oo = idTest;
            var t = from ap in n select ap;
            t = t.Where(e => e.Reportid.Equals(id.ToString()));

            return View(t);
        }

        /// <summary>
        /// The _InvoiceAsync.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> _InvoiceAsync(int id)
        {

            var samplesPdf = _repository.SamplesPdfs.FirstOrDefault(i => i.Reportid.Equals(id.ToString()));



            var i = await DownloadFileFtp(samplesPdf.SampleName);

            byte[] myByteArray = i;
            string base64BinaryStr = Convert.ToBase64String(myByteArray);
            byte[] byteInfo = Convert.FromBase64String(base64BinaryStr);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteInfo, 0, byteInfo.Length);
            pdfStream.Position = 0;

            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// The Helper.
        /// </summary>
        /// <param name="result">The result<see cref="string"/>.</param>
        /// <param name="SampleName">The SampleName<see cref="string"/>.</param>
        /// <param name="Reportid">The Reportid<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> Helper(string result, string SampleName, string Reportid)
        {


            var i = await DownloadFileFtp(SampleName);
            var path1 = Path.Combine(_repositoryEnv.WebRootPath, "pdf", SampleName + ".pdf");
            System.IO.File.WriteAllBytes(path1, i);


            await TestMod(SampleName, result);
            _repository.MarkReportAsChecked(Reportid);

            System.IO.File.Delete(path1);
            return RedirectToAction("AddResult");
        }

        /// <summary>
        /// The TestMod.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="res">The res<see cref="string"/>.</param>
        public async Task TestMod(string name, string res)
        {

            var loadPath = Path.Combine(_repositoryEnv.WebRootPath, "pdf", name + ".pdf");
            DocumentCore dc = DocumentCore.Load(loadPath);

            Regex regex = new Regex(@"Result", RegexOptions.IgnoreCase);
            Regex rege1 = new Regex(@"Positive", RegexOptions.IgnoreCase);
            Regex rege2 = new Regex(@"Negative", RegexOptions.IgnoreCase);

            foreach (ContentRange item in dc.Content.Find(regex).Reverse())
            {
                item.Replace(res);
                System.Diagnostics.Debug.WriteLine(item.Find(regex).Any());


            }

            foreach (ContentRange item in dc.Content.Find(rege1).Reverse())
            {
                item.Replace(res);

            }
            foreach (ContentRange item in dc.Content.Find(rege2).Reverse())
            {
                item.Replace(res);

            }
            dc.Save(loadPath, new PdfSaveOptions());
            FTPhandler ftPhandler = new FTPhandler(_config);

            FileStream fs = new FileStream(loadPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(loadPath).Length;


            // Load input image into Byte Array
            var buff = br.ReadBytes((int)numBytes);
            fs.Close();
            br.Close();
            await ftPhandler.FTPUpload(name, buff);


        }



        /// <summary>
        /// The DownloadFileFtp.
        /// </summary>
        /// <param name="file">The file<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{byte[]}"/>.</returns>
        public async Task<byte[]> DownloadFileFtp(string file)
        {

            var ftp = _config.GetValue<string>("Ftp:ip");

            var userName = _config.GetValue<string>("Ftp:username");
            var password = _config.GetValue<string>("Ftp:Password");

            var n = file;


            var fullpathsFtp = $"" + ftp + $"{n}" + ".pdf";

            using var request = new WebClient { Credentials = new NetworkCredential(userName, password) };


            var getData = await request.DownloadDataTaskAsync(new Uri(fullpathsFtp));

            byte[] fileData = getData;
            if (fileData != null)
            {
                Response.ContentType = "application/pdf";
            }

            return fileData;
        }
    }
}
