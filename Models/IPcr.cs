namespace PCR.Models
{
    using PCR.Models.ViewModels;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="IPcr" />.
    /// </summary>
    public interface IPcr
    {
        /// <summary>
        /// Gets the Users.
        /// </summary>
        IQueryable<User> Users { get; }

        /// <summary>
        /// Gets the Clicics.
        /// </summary>
        IQueryable<Clinic> Clicics { get; }

        /// <summary>
        /// The SaveReport.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string SaveReport(Report report);

        /// <summary>
        /// The ChangeUser.
        /// </summary>
        /// <param name="user">The user<see cref="string"/>.</param>
        /// <param name="boo">The boo<see cref="string"/>.</param>
        void ChangeUser(string user, string boo);

        /// <summary>
        /// Gets the Samples.
        /// </summary>
        IQueryable<Sample> Samples { get; }

        /// <summary>
        /// The UploadSample.
        /// </summary>
        /// <param name="report">The report<see cref="Report"/>.</param>
        /// <param name="sample">The sample<see cref="Sample"/>.</param>
        /// <param name="rpID">The rpID<see cref="string"/>.</param>
        void UploadSample(Report report, Sample sample, string rpID);

        /// <summary>
        /// The AddUser.
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <param name="userEmail">The userEmail<see cref="string"/>.</param>
        /// <param name="role">The role<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        public void AddUser(string userName, string userEmail, string role, string name);

        /// <summary>
        /// Gets the Reports.
        /// </summary>
        IQueryable<Report> Reports { get; }

        /// <summary>
        /// The MarkReportAsChecked.
        /// </summary>
        /// <param name="reportid">The reportid<see cref="string"/>.</param>
        void MarkReportAsChecked(string reportid);

        /// <summary>
        /// The GetName.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetName();

        public void UpdateUserInfo(string email, string pass, string confPass);
        /// <summary>
        /// Gets the SamplesPdfs.
        /// </summary>
        IQueryable<SamplesPdf> SamplesPdfs { get; }

        /// <summary>
        /// Gets the SamplesPdfs2.
        /// </summary>
        IQueryable<SamplesPdf> SamplesPdfs2 { get; }

        /// <summary>
        /// Gets the GetAllDoctors.
        /// </summary>
        IQueryable<User> GetAllDoctors { get; }
    }
}
