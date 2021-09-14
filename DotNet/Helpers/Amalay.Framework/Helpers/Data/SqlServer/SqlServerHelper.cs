using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class SqlServerHelper : IDisposable
    {
        private string fileName = "SqlServerHelper.cs";
        private string message = string.Empty;
        private const int defaultSQLTimeout = 180;
        public Func<IDataReader, object> ReadData;

        //#region "Singleton"

        //private static readonly SqlServerHelper instance = new SqlServerHelper();

        //private SqlServerHelper() { }

        //public static SqlServerHelper Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        //#endregion

        #region "Properties"

        private string ConnectionString { get; set; }

        #endregion

        public SqlServerHelper(IDictionary<string, string> settings)
        {
            if (settings != null && settings.Count > 0)
            {
                this.ConnectionString = settings["SqlConnectionString"];
            }
        }

        private SqlConnection GetSqlConnection(bool enableAsync = false)
        {
            SqlConnection sqlConnection = null;

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new Exception("SqlConnectionString is not valid!");
            }

            if (enableAsync)
            {
                var builder = new SqlConnectionStringBuilder(this.ConnectionString);

                sqlConnection = new SqlConnection(builder.ToString());
            }
            else
            {
                sqlConnection = new SqlConnection(this.ConnectionString);
            }

            return sqlConnection;
        }

        public SqlParameter GetSqlParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        public SqlParameter GetSqlParameter(string parameterName, SqlDbType sqlDbType, object value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, sqlDbType);

            parameter.Value = (value == null) ? DBNull.Value : value;

            return parameter;
        }

        public SqlParameter GetSqlParameter(string parameterName, SqlDbType sqlDbType, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(parameterName, sqlDbType);
            parameter.Direction = direction;

            return parameter;
        }

        public SqlParameter GetSqlParameter(string parameterName, SqlDbType sqlDbType, int size, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(parameterName, sqlDbType);
            parameter.Direction = direction;

            if (size > 0)
            {
                parameter.Size = size;
            }

            return parameter;
        }

        public int ExecuteNonQuery(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection())
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            sqlCommand.Parameters.Add(parameter);
                        }
                    }

                    sqlConnection.Open();

                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection(true))
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            sqlCommand.Parameters.Add(param);
                        }
                    }

                    await sqlConnection.OpenAsync();

                    return await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public object ExecuteNonQueryWithReturnValue(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection())
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            sqlCommand.Parameters.Add(parameter);
                        }
                    }

                    SqlParameter returnValue = sqlCommand.Parameters.Add("@Return", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();

                    return returnValue.Value;
                }
            }
        }

        public object ExecuteNonQueryWithOutputParameter(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection())
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            sqlCommand.Parameters.Add(parameter);
                        }
                    }

                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteNonQuery();

                    return result;
                }
            }
        }

        public async Task<object> GetData(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection(true))
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            sqlCommand.Parameters.Add(parameter);
                        }
                    }

                    sqlConnection.Open();

                    try
                    {
                        using (var reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            return await Task.Run(() => ReadData(reader)).ConfigureAwait(false);
                        }

                    }                    
                    finally
                    {
                        sqlCommand.Dispose();
                        sqlConnection.Close();
                    }
                }
            }
        }

        public T ExecuteScalar<T>(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            var value = default(T);
            var defaultType = typeof(T);

            using (var sqlConnection = this.GetSqlConnection())
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            sqlCommand.Parameters.Add(parameter);
                        }
                    }

                    sqlConnection.Open();

                    var result = sqlCommand.ExecuteScalar();

                    if (!DBNull.Value.Equals(result))
                    {
                        if (defaultType.IsGenericType && defaultType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            defaultType = Nullable.GetUnderlyingType(defaultType);
                        }

                        value = (T)System.Convert.ChangeType(result, defaultType);
                    }

                    return value;
                }
            }
        }

        public async Task<object> ExecuteScalarAsync(string storedProcedureName, Collection<SqlParameter> parameters = null)
        {
            using (var sqlConnection = this.GetSqlConnection(true))
            {
                using (var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            sqlCommand.Parameters.Add(param);
                        }
                    }

                    SqlParameter returnValue = sqlCommand.Parameters.Add("@Return", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    sqlConnection.Open();

                    await sqlCommand.ExecuteScalarAsync();

                    return returnValue.Value;
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
