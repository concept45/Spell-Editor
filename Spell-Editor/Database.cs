using System;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace Spell_Editor
{
    public class Database<Connection, StrBuilder, Parameter, Command, Transaction>
        where Connection : DbConnection
        where Command : DbCommand
        where Transaction : DbTransaction
    {
        public StrBuilder connectionString { get; set; }

        public bool CanConnectToDatabase(MySqlConnectionStringBuilder _connectionString)
        {
            bool exceptionOccurred = false;

            try
            {
                //! Close the connection again since this is just a try-connection function. We actually connect
                //! when the mainform is opened (this happens automatically because we use 'using').
                using (MySqlConnection connection = new MySqlConnection(_connectionString.ToString()))
                    connection.Open();
            }
            catch (MySqlException)
            {
                exceptionOccurred = true;
            }

            return !exceptionOccurred;
        }

        public async Task<bool> ExecuteNonQuery(string nonQuery, params Parameter[] parameters)
        {
            bool exceptionOccurred = false;

            await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), nonQuery, conn))
                    {
                        try
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            exceptionOccurred = true;
                        }
                    }

                    conn.Close();
                }
            });

            return !exceptionOccurred;
        }

        public Task<DataTable> ExecuteQuery(string query, params Parameter[] parameters)
        {
            return ExecuteQueryWithCancellation(new CancellationToken(), query, parameters);
        }

        public async Task<DataTable> ExecuteQueryWithCancellation(CancellationToken token, string query, params Parameter[] parameters)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                    {
                        conn.Open();

                        using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);

                            try
                            {
                                var reader = await cmd.ExecuteReaderAsync(token);

                                if (token.IsCancellationRequested)
                                    token.ThrowIfCancellationRequested();

                                var dt = new DataTable();
                                dt.Load(reader);
                                conn.Close();
                                return dt;
                            }
                            catch (Exception)
                            {
                                return new DataTable();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return new DataTable();
            }
        }

        public async Task<object> ExecuteScalar(string query, params Parameter[] parameters)
        {
            return await Task.Run(() =>
            {
                using (Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), connectionString.ToString()))
                {
                    conn.Open();

                    using (Command cmd = (Command)Activator.CreateInstance(typeof(Command), query, conn))
                    {
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                        try
                        {
                            object returnVal = cmd.ExecuteScalar();
                            conn.Close();
                            return returnVal;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            });
        }
    }
}
