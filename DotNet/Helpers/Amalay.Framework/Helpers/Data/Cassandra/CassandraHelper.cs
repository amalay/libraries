using Amalay.Entities;
using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class CassandraHelper : IDisposable
    {
        private ISession cassandraSession = null;
        private IMapper cassandraMapper = null;

        #region "Properties"

        private string CassandraConnectionString { get; set; }

        private string CassandraContactPoint { get; set; }

        private string CassandraKeyspace { get; set; }

        private string CassandraUserName { get; set; }

        private string CassandraPassword { get; set; }

        private int CassandraPortNumber { get; set; }

        public ISession CassandraSession
        {
            get
            {
                if (this.cassandraSession == null)
                {
                    this.cassandraSession = this.GetCassandraSessionUsingCredential();
                }

                return this.cassandraSession;
            }
        }

        public IMapper CassandraMapper
        {
            get
            {
                if (this.cassandraMapper == null)
                {
                    if (this.CassandraSession != null)
                    {
                        this.cassandraMapper = new Mapper(this.CassandraSession);
                    }
                }

                return this.cassandraMapper;
            }
        }

        #endregion

        public CassandraHelper(IDictionary<string, string> settings)
        {
            if (settings != null && settings.Count > 0)
            {
                //this.CassandraConnectionString = setting.Settings["CassandraConnectionString"];

                this.CassandraContactPoint = settings["CassandraContactPoint"];
                this.CassandraKeyspace = settings["CassandraKeyspace"];
                this.CassandraUserName = settings["CassandraUserName"];
                this.CassandraPassword = settings["CassandraPassword"];

                var port = settings.GetValue<int>("CassandraPortNumber"); //10350
                this.CassandraPortNumber = (port > 0) ? port : 10350;
            }
        }

        public ISession GetCassandraSessionUsingCredential()
        {
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => this.CassandraContactPoint);

            Cluster cluster = Cluster.Builder()
                .WithCredentials(this.CassandraUserName, this.CassandraPassword)
                .WithPort(this.CassandraPortNumber)
                .AddContactPoint(this.CassandraContactPoint)
                .WithSSL(options)
                .Build();

            return cluster.Connect(this.CassandraKeyspace);
        }

        public ISession GetCassandraSessionUsingConnectionString()
        {
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);

            Cluster cluster = Cluster.Builder()
                .WithConnectionString(this.CassandraConnectionString)
                .WithSSL(options)
                .Build();

            return cluster.Connect(this.CassandraKeyspace);
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
