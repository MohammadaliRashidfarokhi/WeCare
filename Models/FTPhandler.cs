namespace PCR.Models
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FTPhandler" />.
    /// </summary>
    public class FTPhandler
    {
        /// <summary>
        /// Defines the _config.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="FTPhandler"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="IConfiguration"/>.</param>
        public FTPhandler(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// The UploadFileFtp.
        /// </summary>
        /// <param name="file">The file<see cref="string"/>.</param>
        /// <param name="b">The b<see cref="byte[]"/>.</param>
        public void UploadFileFtp(string file, byte[] b)
        {

            //        "ip": "ftp://ftp@192.168.1.4/pdf/",
            //"Password": "A12345",
            //"username": "pcr"

            var ftp = "ftp://ftp@192.168.1.4/pdf/";

            var userName = "pcr";
            var password= "A12345";

            var d = Path.GetFileName(file);
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftp + d + ".pdf");

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(userName, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
          //  request.EnableSsl = false;


            Stream reqStream = request.GetRequestStream();

            reqStream.Write(b, 0, b.Length);
            reqStream.Close();
        }

        /// <summary>
        /// The ListDirectory.
        /// </summary>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public List<string> GetAllFtpFiles()
        {
            try
            {
                var ftp = "ftp://wecare007-001@win8019.site4now.net/site1/pdf/";

                var userName = "wecare007-001";
                var password = "WeCareFTP12345";
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftp);
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var lineArr = line.Split('/');
                    line = lineArr[lineArr.Length - 1];
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public async Task FTPUpload(string file, byte[] b)
        {
            try
            {
                var d = Path.GetFileName(file);
                var ftp = "ftp://wecare007-001@win8019.site4now.net/site1/pdf/";

                var userName = "wecare007-001";
                var password = "WeCareFTP12345";
                //Create a FTP Request Object and Specfiy a Complete Path
                FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create(ftp + d + ".pdf");

                //Call A FileUpload Method of FTP Request Object
                reqObj.Method = WebRequestMethods.Ftp.UploadFile;

                //If you want to access Resourse Protected,give UserName and PWD
                reqObj.Credentials = new NetworkCredential(userName, password);

                // Copy the contents of the file to the byte array.

                reqObj.ContentLength = b.Length;

                //Upload File to FTPServer
                Stream requestStream = reqObj.GetRequestStream();
                requestStream.Write(b, 0, b.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)reqObj.GetResponse();
                response.Close();
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// The ToSecureString.
        /// </summary>
        /// <param name="source">The source<see cref="string"/>.</param>
        /// <returns>The <see cref="SecureString"/>.</returns>
        public SecureString ToSecureString(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }
            var result = new SecureString();
            foreach (var password in source)
            {
                result.AppendChar(password);
            }
            return result;
        }
    }
}
