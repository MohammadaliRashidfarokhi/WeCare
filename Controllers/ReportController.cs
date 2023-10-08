using Microsoft.AspNetCore.Authorization;

namespace PCR.Controllers
{
    using DinkToPdf.Contracts;
    using MessagingToolkit.QRCode.Codec;
    using MessagingToolkit.QRCode.Codec.Data;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using PCR.Models;
    using PCR.Models.ViewModels;
    using SelectPdf;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    [Authorize(Roles = "User")]
    /// <summary>
    /// Defines the <see cref="ReportController" />.
    /// </summary>
    public class ReportController : Controller
    {
        /// <summary>
        /// Defines the name.
        /// </summary>
        private string name = "";

        /// <summary>
        /// Defines the _converter.
        /// </summary>
        private IConverter _converter;

        /// <summary>
        /// Defines the _userManager which is provides the APIs for managing user in a persistence store...
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _repositoryEnv;
        /// <summary>
        /// Defines the idTest.
        /// </summary>
        private static string idTest = "";

        /// <summary>
        /// Defines the _roleManager; Provides the APIs for managing roles in a persistence store...
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Defines the _protector.
        /// </summary>
        private readonly IDataProtector _protector;

        /// <summary>
        /// Defines the _repository.
        /// </summary>
        private readonly IPcr _repository;

        /// <summary>
        /// Defines the _config.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Defines the _compositeViewEngine.
        /// </summary>
        protected readonly ICompositeViewEngine _compositeViewEngine;

        /// <summary>
        /// Defines the _appEnvironment.
        /// </summary>
        private readonly IHostingEnvironment _appEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="userManager">The userManager<see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{IdentityRole}"/>.</param>
        /// <param name="converter">The converter<see cref="IConverter"/>.</param>
        /// <param name="repo">The repo<see cref="IPcr"/>.</param>
        /// <param name="provider">The provider<see cref="IDataProtectionProvider"/>.</param>
        /// <param name="config">The config<see cref="IConfiguration"/>.</param>
        /// <param name="appEnvironment">The appEnvironment<see cref="IHostingEnvironment"/>.</param>
        /// <param name="compositeViewEngine">The compositeViewEngine<see cref="ICompositeViewEngine"/>.</param>
        public ReportController(UserManager<IdentityUser> userManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, IConverter converter, IPcr repo, IDataProtectionProvider provider, IConfiguration config, IHostingEnvironment appEnvironment, ICompositeViewEngine compositeViewEngine)
        {
            _converter = converter;
            _userManager = userManager;
            _roleManager = roleManager;
            _protector = provider.CreateProtector("PCR.Controllers.SymmetricEncryptDecrypt");
            _repository = repo;
            _config = config;
            _compositeViewEngine = compositeViewEngine;
            _appEnvironment = appEnvironment;
            _repositoryEnv = env;
        }

        /// <summary>
        /// The Appointment.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public IActionResult Appointment(Report report)
        {


            return View(report);



        }

        /// <summary>
        /// The Appointment.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Appointment()
        {


            return View();
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //  var n = await _repository.SamplesPdfs.ToListAsync();
            var test = _repository.Reports;
            var d = _repository.GetName();
            test = test.Where(s => s.UserName.Equals(_repository.GetName()));


            ViewBag.all = test;
            return View("Index");
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
        /// The _Invoice2.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> _Invoice2(int id)
        {
            var test = _repository.Reports;
            var list = test
                .Select(s => new { Reportid = s.Reportid })
                .ToList();
            //    test = test.Where(s => s.Reportid.Equals(id));

            var test2 = _repository.Samples;
            var list2 = test2
                .Select(s => new { Reportid = s.Reportid, SampleName = s.SampleName })
                .ToList();
            list2.RemoveAll(s => s.Reportid != id);








            var samplesPdf = _repository.SamplesPdfs.FirstOrDefault(i => i.Reportid.Equals(id.ToString()));





            var i = await DownloadFileFtp(list2.ElementAt(0).SampleName);
            byte[] myByteArray = i;
            string base64BinaryStr = Convert.ToBase64String(myByteArray);
            byte[] byteInfo = Convert.FromBase64String(base64BinaryStr);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteInfo, 0, byteInfo.Length);
            pdfStream.Position = 0;

            return new FileStreamResult(pdfStream, "application/pdf");
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

        /// <summary>
        /// The GenerateCode.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string GenerateCode()
        {
            var ftp = _config.GetValue<string>("Ftp:ip");



            var userName = _config.GetValue<string>("NetworkCredential:UserName");
            var password = _config.GetValue<string>("NetworkCredential:Password");




            var pa = _appEnvironment.WebRootPath + "\\" + "QR";
            string domain = "https://localhost:44356/";
            //string domain = "http://saadou00-001-site1.btempurl.com/";
            string qrCodeImgFileName = new Random().Next().ToString();
            string qrCodeInformation = domain + "Report/Checker?fil=";
            QRCodeEncoder qe = new QRCodeEncoder();
            QRCodeDecoder decoder = new QRCodeDecoder();
            string d = "QR" + qrCodeImgFileName + ".jpg";
            TempData["iName"] = d;





            var encryptedText = _protector.Protect(qrCodeImgFileName);


            var fullPath = qrCodeInformation + encryptedText;
            System.Drawing.Bitmap bm = qe.Encode(fullPath);



            var memStream = new MemoryStream();
            bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);



            var test = memStream.ToArray();

            string imreBase64Data = Convert.ToBase64String(test);

            ViewBag.Image = $"data:image/jpg;base64,{imreBase64Data}";



            MessagingToolkit.QRCode.Codec.Data.QRCodeBitmapImage image = new QRCodeBitmapImage(bm);



            return encryptedText;
        }

        /// <summary>
        /// The Checker.
        /// </summary>
        /// <param name="fil">The fil<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> Checker(string fil)
        {
            FTPhandler ftPhandler = new FTPhandler(_config);
            List<string> list = ftPhandler.GetAllFtpFiles();


            var path = Request.GetDisplayUrl();
            string[] array = path.Split('?');
            var check = array[1].Substring(4);


            var encryptedText = _protector.Unprotect(check);
            foreach (var VARIABLE in list)
            {
                var text = _protector.Unprotect(VARIABLE.Substring(0, VARIABLE.Length - 4));
                if (text.Equals(encryptedText))
                {
                    var i = await DownloadFileFtp(VARIABLE.Substring(0, VARIABLE.Length - 4));
                    byte[] myByteArray = i;
                    string base64BinaryStr = Convert.ToBase64String(myByteArray);
                    byte[] byteInfo = Convert.FromBase64String(base64BinaryStr);
                    MemoryStream pdfStream = new MemoryStream();
                    pdfStream.Write(byteInfo, 0, byteInfo.Length);
                    pdfStream.Position = 0;

                    return new FileStreamResult(pdfStream, "application/pdf");

                }

            }



            return View();
        }



        /// <summary>
        /// The RandomNumber.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string RandomNumber()
        {
            var characters = "0123456789";
            var Charsarr = new char[6];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var resultString = new String(Charsarr);

            return resultString;
        }

        /// <summary>
        /// The _Invoice.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> _Invoice(Report report)
        {
            //DateTime to date only


            if (!ModelState.IsValid)
            {
                return View("Appointment", report);
                //  return RedirectToAction("Appointment");
            }
            //            change Datetime to date

            Sample sample = new Sample();
            var saveRP = new Report();
            report.Reportid = RandomNumber();
            report.Checked = false;






            RegionInfo myRI1 = new RegionInfo(report.Citizenship);
            report.Citizenship = myRI1.DisplayName;

            var userName = _config.GetValue<string>("NetworkCredential:UserName");
            var password = _config.GetValue<string>("NetworkCredential:Password");
            FTPhandler ftPhandler = new FTPhandler(_config);
            name = GenerateCode();

            sample.SampleName = name;


            using (var stringWriter = new StringWriter())
            {
                var viewResult = _compositeViewEngine.FindView(ControllerContext, "_Invoice", false);


                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                var i = ViewBag.Image;
                var htmlDoc = new StringBuilder();
                htmlDoc.AppendLine($"<img style=\" padding: 0px 42px 0px 42px \" src=\"{i}\" />");

                var htmlToPdf = new HtmlToPdf(1000, 1414);
                htmlToPdf.Options.DrawBackground = true;

                var pdf = htmlToPdf.ConvertHtmlString(TemplateGenerator.GetHTMLString(report) + htmlDoc);
                var pdfBytes = pdf.Save();
                TempData["name"] = name;

                var stream = new MemoryStream(pdfBytes);
                // ftPhandler.UploadFileFtp(name, pdfBytes);
                await ftPhandler.FTPUpload(name, pdfBytes);

                _repository.SaveReport(report);

                _repository.UploadSample(report, sample, report.Reportid);
                return new FileStreamResult(stream, "application/pdf");

            }




        }

    }
}
