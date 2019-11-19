using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// DbHelper 的摘要说明
/// </summary>

public class DbHelperSQL
{

    protected static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

    public DbHelperSQL()
    {
    }

    public static int GetMaxID(string FieldName, string TableName)
    {
        string sQLString = "select max(" + FieldName + ")+1 from " + TableName;
        object single = DbHelperSQL.GetSingle(sQLString);
        int result;
        if (single == null)
        {
            result = 1;
        }
        else
        {
            result = int.Parse(single.ToString());
        }
        return result;
    }

    public static bool Exists(string strSql, params SqlParameter[] cmdParms)
    {
        object single = DbHelperSQL.GetSingle(strSql, cmdParms);
        int num;
        if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
        {
            num = 0;
        }
        else
        {
            num = int.Parse(single.ToString());
        }
        return num != 0;
    }

    public static int ExecuteSql(string SQLString)
    {
        int result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    int num = sqlCommand.ExecuteNonQuery();
                    result = num;
                }
                catch (SqlException ex)
                {
                    sqlConnection.Close();
                    throw new Exception(ex.Message);
                }
            }
        }
        return result;
    }

    public static void ExecuteSqlTran(ArrayList SQLStringList)
    {
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand
            {
                Connection = sqlConnection
            })
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string text = SQLStringList[i].ToString();
                        if (text.Trim().Length > 1)
                        {
                            sqlCommand.CommandText = text;
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    sqlTransaction.Commit();
                }
                catch (SqlException ex)
                {
                    sqlTransaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }

    public static int ExecuteSql(string SQLString, string content)
    {
        int result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
            SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText)
            {
                Value = content
            };
            sqlCommand.Parameters.Add(sqlParameter);
            try
            {
                sqlConnection.Open();
                int num = sqlCommand.ExecuteNonQuery();
                result = num;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
            }
        }
        return result;
    }

    public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
    {
        int result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
            SqlParameter sqlParameter = new SqlParameter("@fs", SqlDbType.Image)
            {
                Value = fs
            };
            sqlCommand.Parameters.Add(sqlParameter);
            try
            {
                sqlConnection.Open();
                int num = sqlCommand.ExecuteNonQuery();
                result = num;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
            }
        }
        return result;
    }

    public static object GetSingle(string SQLString)
    {
        object result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    object obj = sqlCommand.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        result = null;
                    }
                    else
                    {
                        result = obj;
                    }
                }
                catch (SqlException ex)
                {
                    sqlConnection.Close();
                    throw new Exception(ex.Message);
                }
            }
        }
        return result;
    }

    public static SqlDataReader ExecuteReader(string strSQL)
    {
        SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString);
        SqlDataReader result;
        using (SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection))
        {
           
            try
            {
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                result = sqlDataReader;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return result;
    }

    public static DataSet Query(string SQLString)
    {
        DataSet result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            DataSet dataSet = new DataSet();
            try
            {
                sqlConnection.Open();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SQLString, sqlConnection))
                {
                    sqlDataAdapter.Fill(dataSet, "ds");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            result = dataSet;
        }
        return result;
    }

    public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
    {
        int result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                try
                {
                    DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                    int num = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                    result = num;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        return result;
    }

    public static void ExecuteSqlTran(Hashtable SQLStringList)
    {
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            sqlConnection.Open();
            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        foreach (DictionaryEntry dictionaryEntry in SQLStringList)
                        {
                            string cmdText = dictionaryEntry.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])dictionaryEntry.Value;
                            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, cmdText, cmdParms);
                            int num = sqlCommand.ExecuteNonQuery();
                            sqlCommand.Parameters.Clear();
                            sqlTransaction.Commit();
                        }
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
    {
        object result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                try
                {
                    DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                    object obj = sqlCommand.ExecuteScalar();
                    sqlCommand.Parameters.Clear();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        result = null;
                    }
                    else
                    {
                        result = obj;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        return result;
    }

    public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
    {
        SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString);
        SqlDataReader result;
        using (SqlCommand sqlCommand = new SqlCommand())
        {
            
            try
            {
                DbHelperSQL.PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Parameters.Clear();
                result = sqlDataReader;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return result;
    }

    public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
    {
        DataSet result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand();
            DbHelperSQL.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlDataAdapter.Fill(dataSet, "ds");
                    sqlCommand.Parameters.Clear();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                result = dataSet;
            }
        }
        return result;
    }

    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
        {
            conn.Open();
        }
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
        {
            cmd.Transaction = trans;
        }
        cmd.CommandType = CommandType.Text;
        if (cmdParms != null)
        {
            for (int i = 0; i < cmdParms.Length; i++)
            {
                SqlParameter value = cmdParms[i];
                cmd.Parameters.Add(value);
            }
        }
    }

    public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
    {
        SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString);
        sqlConnection.Open();
        using (SqlCommand sqlCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand.ExecuteReader();
        }
    }

    public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
    {
        DataSet result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            DataSet dataSet = new DataSet();
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
            {
                SelectCommand = DbHelperSQL.BuildQueryCommand(sqlConnection, storedProcName, parameters)
            };
            sqlDataAdapter.Fill(dataSet, tableName);
            sqlConnection.Close();
            result = dataSet;
        }
        return result;
    }

    private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand sqlCommand = new SqlCommand(storedProcName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        for (int i = 0; i < parameters.Length; i++)
        {
            SqlParameter value = (SqlParameter)parameters[i];
            sqlCommand.Parameters.Add(value);
        }
        return sqlCommand;
    }

    public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
    {
        int result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = BuildIntCommand(sqlConnection, storedProcName, parameters))
            {
                rowsAffected = sqlCommand.ExecuteNonQuery();
                int num = (int)sqlCommand.Parameters["ReturnValue"].Value;
                result = num;
            }
            
        }
        return result;
    }

    public static object RunProcedureGetSingle(string storedProcName, IDataParameter[] parameters)
    {
        object result;
        using (SqlConnection sqlConnection = new SqlConnection(DbHelperSQL.connectionString))
        {
            using (SqlCommand sqlCommand = BuildIntCommand(sqlConnection, storedProcName, parameters))
            {
                sqlConnection.Open();
                object obj = sqlCommand.ExecuteScalar().ToString();
                result = obj;
            }
            
        }
        return result;
    }

    private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand sqlCommand = DbHelperSQL.BuildQueryCommand(connection, storedProcName, parameters);
        sqlCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
        return sqlCommand;
    }

}