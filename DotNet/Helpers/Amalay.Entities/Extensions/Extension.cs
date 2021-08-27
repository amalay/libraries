using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public static partial class Extension
    {
        public static T ConvertTo<T>(this object result)
        {
            var value = default(T);

            if (result != null)
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                value = (T)(converter.ConvertFromInvariantString(result.ToString()));
            }

            return value;
        }

        private static bool HasKey(this System.Collections.IDictionary dictionary, string key)
        {
            return dictionary.Contains(key);
        }

        private static bool HasKey(this System.Collections.Generic.IDictionary<string, string> dictionary, string key)
        {
            return dictionary.Keys.Contains(key);
        }

        private static bool HasKey(this System.Collections.Generic.IDictionary<string, object> dictionary, string key)
        {
            return dictionary.Keys.Contains(key);
        }

        public static T GetValue<T>(this System.Collections.IDictionary dictionary, string key)
        {
            var value = default(T);

            if (dictionary.HasKey(key))
            {
                if (dictionary[key] != null)
                {
                    value = dictionary[key].ConvertTo<T>();
                }
            }

            return value;
        }

        public static T GetValue<T>(this System.Collections.Generic.IDictionary<string, string> dictionary, string key)
        {
            var value = default(T);

            if (dictionary.HasKey(key))
            {
                if (dictionary[key] != null)
                {
                    value = dictionary[key].ConvertTo<T>();
                }
            }

            return value;
        }

        public static T GetValue<T>(this System.Collections.Generic.IDictionary<string, object> dictionary, string key)
        {
            var value = default(T);

            if (dictionary.HasKey(key))
            {
                if (dictionary[key] != null)
                {
                    value = dictionary[key].ConvertTo<T>();
                }
            }

            return value;
        }

        public static T GetValue<T>(this System.Collections.Generic.IDictionary<string, string> dictionary, string prefix, string secretName)
        {
            var value = default(T);

            if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(secretName))
            {
                var key = $"{prefix}-{secretName}";

                if (dictionary.HasKey(key))
                {
                    if (dictionary[key] != null)
                    {
                        value = dictionary[key].ConvertTo<T>();
                    }
                }
            }

            return value;
        }
                
        public static T GetValue<T>(this System.Collections.Specialized.NameValueCollection appSettings, string key)
        {
            var value = default(T);

            if (appSettings != null && appSettings.Count > 0)
            {
                var appSetting = appSettings[key];

                if (!string.IsNullOrWhiteSpace(appSetting))
                {
                    value = appSetting.ConvertTo<T>();
                }
            }

            return value;
        }

        public static T GetValue<T>(this JObject jobject, string key)
        {
            T value = default(T);

            if (jobject != null)
            {
                var jtoken = jobject.SelectToken(key);

                //if (jtoken != null && jtoken.GetType() != typeof(Newtonsoft.Json.Linq.JObject) || jtoken.GetType() != typeof(Newtonsoft.Json.Linq.JArray) || jtoken.GetType() == typeof(Newtonsoft.Json.Linq.JValue))
                //{

                //}

                if (jtoken != null)
                {
                    if (jtoken.GetType() == typeof(Newtonsoft.Json.Linq.JValue))
                    {
                        value = jtoken.ToString().ConvertTo<T>();
                    }
                    else if (jtoken.GetType() == typeof(Newtonsoft.Json.Linq.JObject) || jtoken.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                    {
                        value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jtoken.ToString());
                    }
                }
            }

            return value;
        }

        public static T GetValue<T>(this IDataReader reader, string columnName)
        {
            var value = default(T);
            var t = typeof(T);

            if (reader.HasColumn(columnName))
            {
                if (!DBNull.Value.Equals(reader[columnName]))
                {
                    if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        t = Nullable.GetUnderlyingType(t);
                    }
                    value = (T)Convert.ChangeType(reader[columnName], t);
                }
            }

            return value;
        }

        public static bool IsValidDataTable(this System.Data.DataTable dataTable, int rowCount = 0)
        {
            var isValid = false;

            if (dataTable != null && dataTable.Rows.Count > rowCount)
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool IsValidDataSet(this System.Data.DataSet ds, int tableCount = 0, int rowCount = 0)
        {
            var isValid = false;

            if (ds != null && ds.Tables != null && ds.Tables.Count > tableCount && ds.Tables[tableCount].Rows != null && ds.Tables[tableCount].Rows.Count > rowCount)
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool HasColumn(this IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> GetEnumerator<T>(this IDataReader dataReader, Func<IDataRecord, T> generator)
        {
            while (dataReader.Read())
            {
                yield return generator(dataReader);
            }
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string sortExpression)
        {
            string[] sortParts = sortExpression.Split(' ');
            var param = Expression.Parameter(typeof(T), string.Empty);

            try
            {
                var property = Expression.Property(param, sortParts[0]);
                var sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

                if (sortParts.Length > 1 && sortParts[1].Equals("decending", StringComparison.OrdinalIgnoreCase))
                {
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);
                }

                return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
            }
            catch (ArgumentException)
            {
                return source;
            }
        }        

        public static string ToHexString(this byte[] hex)
        {
            if (hex == null)
                return null;

            if (hex.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (byte b in hex)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static async Task ForEachAsyncConcurrent<T>(this IEnumerable<T> enumerable, Func<T, Task> action, int? maxDegreeOfParallelism = null)
        {
            if (maxDegreeOfParallelism.HasValue)
            {
                using (var semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism.Value, maxDegreeOfParallelism.Value))
                {
                    var tasksWithThrottler = new List<Task>();

                    foreach (var item in enumerable)
                    {
                        // Increment the number of currently running tasks and wait if they are more than limit.
                        await semaphoreSlim.WaitAsync();

                        tasksWithThrottler.Add(Task.Run(async () =>
                        {
                            await action(item);

                            // action is completed, so decrement the number of currently running tasks
                            semaphoreSlim.Release();
                        }));
                    }

                    // Wait for all tasks to complete.
                    await Task.WhenAll(tasksWithThrottler.ToArray());
                }
            }
            else
            {
                await Task.WhenAll(enumerable.Select(item => action(item)));
            }

            //await enumerable.ForEachAsyncConcurrent(async item => { await SomeAsyncMethod(item); }, 5);
        }

    }
}
