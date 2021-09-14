using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class CertificateHelper
    {
        private string fileName = "CertificateHelper.cs";

        #region "Singleton"

        private static readonly CertificateHelper instance = new CertificateHelper();

        private CertificateHelper() { }

        public static CertificateHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public bool InstallCertificate(string certificateDirectoryName, string cetificateFileName, string cetificatePassword, string powershellScriptFileName)
        {
            bool status = false;

            try
            {
                string certificateDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + certificateDirectoryName + @"\";

                string powerShellScriptFilePath = certificateDirectory + powershellScriptFileName;

                if (System.IO.File.Exists(powerShellScriptFilePath))
                {
                    string certificateFilePath = certificateDirectory + cetificateFileName;

                    if (System.IO.File.Exists(certificateFilePath))
                    {
                        string command = String.Format("/C powershell \"{0}\" -certificateFilePath \"{1}\" -certificatePassword '{2}' ", powerShellScriptFilePath, certificateFilePath, cetificatePassword);

                        System.Diagnostics.Process.Start("cmd", command);
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public X509Certificate2 GetCertificateByThumbPrint(string thumbPrint, StoreLocation location)
        {
            return this.GetCertificate(X509FindType.FindByThumbprint, thumbPrint, location);
        }

        public X509Certificate2 GetCertificateBySubjectName(string subjectName, StoreLocation location)
        {
            return this.GetCertificate(X509FindType.FindBySubjectName, subjectName, location);
        }

        public string CleanThumbPrint(string thumbPrint)
        {
            //replace spaces, non word chars and convert to uppercase
            return System.Text.RegularExpressions.Regex.Replace(thumbPrint, @"\s|\W", "").ToUpper();
        }

        private X509Certificate2 GetCertificate(X509FindType findType, string findValue, StoreLocation location)
        {

            DateTime furthestExpirey = DateTime.MinValue;
            X509Certificate2 certificate = null;
            X509Store certStore = new X509Store("My", location);

            try
            {
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509CertificateCollection certs = certStore.Certificates.Find(findType, findValue, false);

                foreach (X509Certificate2 cert in certs)
                {
                    if (cert != null)
                    {
                        if (cert.NotAfter >= furthestExpirey)
                        {
                            certificate = cert;
                            furthestExpirey = cert.NotAfter;
                        }
                    }
                }
            }
            finally
            {
                certStore.Close();
            }

            return certificate;
        }
    }
}
