namespace PCR.Models
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PCR.Models.ViewModels;
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EFPcr" />.
    /// </summary>
    public class EFPcr : IPcr
    {
        /// <summary>
        /// Defines the context.
        /// </summary>
        private ApplicationDBContext context;

        /// <summary>
        /// Defines the _random.
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Defines the _repositoryEnv.
        /// </summary>
        private readonly IHttpContextAccessor _repositoryEnv;

        /// <summary>
        /// Defines the _protector.
        /// </summary>
        private readonly IDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFPcr"/> class.
        /// </summary>
        /// <param name="ctx">The ctx<see cref="ApplicationDBContext"/>.</param>
        /// <param name="env">The env<see cref="IHttpContextAccessor"/>.</param>
        /// <param name="provider">The provider<see cref="IDataProtectionProvider"/>.</param>
        public EFPcr(ApplicationDBContext ctx, IHttpContextAccessor env, IDataProtectionProvider provider)
        {
            context = ctx;
            _repositoryEnv = env;
            _protector = provider.CreateProtector("PCR.Controllers.SymmetricEncryptDecrypt");
        }

        /// <summary>
        /// The RandomNumber.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public int RandomNumber()
        {


            return _random.Next(0, 1000);
        }

        /// <summary>
        /// Gets the Users.
        /// </summary>
        public IQueryable<User> Users => context.Users;

        /// <summary>
        /// Gets the Clicics.
        /// </summary>
        public IQueryable<Clinic> Clicics => context.Clinics;

        /// <summary>
        /// The ChangeUser.
        /// </summary>
        /// <param name="user">The user<see cref="string"/>.</param>
        /// <param name="boo">The boo<see cref="string"/>.</param>
        public void ChangeUser(string user, string boo)
        {
            var dbEntry = context.Users.FirstOrDefault(e => e.UserName == user);
            if (dbEntry != null)
            {

                dbEntry.Src = boo;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Gets the Samples.
        /// </summary>
        public IQueryable<Sample> Samples => context.Samples;

        /// <summary>
        /// Gets the Reports.
        /// </summary>
        public IQueryable<Report> Reports => context.Reports.Include(e => e.Samples);

        /// <summary>
        /// The SaveReport.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string SaveReport(Report report)
        {

            if (report != null)
            {
                report.PassportNumber = _protector.Protect(report.PassportNumber);
                report.PersonalIdentityNumber = _protector.Protect(report.PersonalIdentityNumber);
                report.UserName = GetName();
                context.Reports.Add(report);

            }
            context.SaveChanges();
            return "id";
        }

        /// <summary>
        /// The UploadSample.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <param name="sample">The sample<see cref="Sample"/>.</param>
        /// <param name="rpID">The rpID<see cref="string"/>.</param>
        public void UploadSample(Report report, Sample sample, string rpID)
        {
            var dbEntry = context.Reports.FirstOrDefault(e => e.Reportid == report.Reportid);
            if (dbEntry != null)
            {
                context.Samples.Add(new Sample { SampleName = sample.SampleName, Reportid = int.Parse(rpID) });
            }

            context.SaveChanges();
        }

        /// <summary>
        /// The GetSamplesPdfs.
        /// </summary>
        /// <returns>The <see cref="IQueryable{SamplesPdf}"/>.</returns>
        public IQueryable<SamplesPdf> GetSamplesPdfs()
        {
            var userName = GetName();
            var m = from clin in Clicics
                    join use in Users on clin.ClinicID equals use.Clicicid
                    join rep in Reports on clin.ClinicName equals rep.Clinique
                    join sample in Samples on rep.Reportid equals sample.Reportid.ToString()
                    orderby rep.Reportid
                    where use.UserName == userName
                    select new SamplesPdf
                    {
                        Reportid = rep.Reportid,
                        Patient = rep.Patient,
                        SampleName = sample.SampleName,
                        PassportNumber = _protector.Unprotect(rep.PassportNumber),
                        checkedBy = sample.checkedBy
                    };
            return m;
        }

        /// <summary>
        /// The GetUserSamplesPdfs.
        /// </summary>
        /// <returns>The <see cref="IQueryable{SamplesPdf}"/>.</returns>
        public IQueryable<SamplesPdf> GetUserSamplesPdfs()
        {
            var userName = GetName();
            var u = from samp in SamplesPdfs
                    join us in userName on samp.Patient equals userName
                    select samp;




            return u;
        }

        public void UpdateUserInfo(string email, string pass, string confPass)
        {
            User user = context.Users.FirstOrDefault(u => u.UserName.Equals(GetName()));
            if (user != null)
            {
                user.Email = email;

            }

            context.SaveChanges();
        }

   

        /// <summary>
        /// The NewUserId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string NewUserId()
        {
            var id = context.Users.OrderByDescending(id => id.Userid).Select(uID => uID.Userid).First();

            var value = int.Parse(id) + 1;



            return value.ToString();
        }

        /// <summary>
        /// The AddUser.
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <param name="userEmail">The userEmail<see cref="string"/>.</param>
        /// <param name="role">The role<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        public void AddUser(string userName, string userEmail, string role, string name)
        {
            User user = new User()
            {
                Userid = NewUserId(),
                UserName = userName,
                Email = userEmail,
                Name = name,
                Role = role,
                Src = "False"
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// The MarkReportAsChecked.
        /// </summary>
        /// <param name="reportid">The reportid<see cref="string"/>.</param>
        public void MarkReportAsChecked(string reportid)
        {
            var report = context.Reports.FirstOrDefault(e => e.Reportid == reportid);
            var doc = context.Users.FirstOrDefault(doc => doc.UserName == GetName());
            var samp = context.Samples.FirstOrDefault(sam => sam.Reportid.ToString() == report.Reportid);

            if (report != null)
            {
                var d = doc.Name;
                report.Checked = true;
                samp.checkedBy = d;
                report.checkedBy = d;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// The GetName.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetName()
        {
            return _repositoryEnv.HttpContext.User.Identity.Name;
        }

        /// <summary>
        /// The GettDoctors.
        /// </summary>
        /// <returns>The <see cref="IQueryable{User}"/>.</returns>
        public IQueryable<User> GettDoctors()
        {

            var m = from clin in Users
                    where clin.Role == "Doctor"
                    select new User
                    {
                        Clicicid = clin.Clicicid,
                        UserName = clin.UserName,
                        Name = clin.Name,
                        Src = "~/images/" + clin.UserName + ".jpg"
                    };

            return m;
        }

        /// <summary>
        /// Gets the SamplesPdfs.
        /// </summary>
        public IQueryable<SamplesPdf> SamplesPdfs => GetSamplesPdfs();

        /// <summary>
        /// Gets the SamplesPdfs2.
        /// </summary>
        public IQueryable<SamplesPdf> SamplesPdfs2 => GetUserSamplesPdfs();

        /// <summary>
        /// Gets the GetAllDoctors.
        /// </summary>
        public IQueryable<User> GetAllDoctors => GettDoctors();
    }
}
