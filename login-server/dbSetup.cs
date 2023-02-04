using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class Database
{
    public static DataSet GetData(string connectionString, string str, string[] tblName = null)
    {
        DataSet dsResult = new DataSet();
        try
        {

            SqlDataReader myReader;


            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(str, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    myCommand.CommandTimeout = myCon.ConnectionTimeout;
                    while (!myReader.IsClosed)
                    {
                        dsResult.Tables.Add().Load(myReader);
                        if (tblName != null && tblName.Length > 0)
                        {
                            dsResult.Tables[dsResult.Tables.Count - 1].TableName = tblName[0];
                            tblName = tblName.Skip(1).ToArray();
                        }
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }
        }
        catch (Exception)
        {
            return null;
        }
        return dsResult;
    }

    public static DataSet GetDataEx(string connectionString, string str, string param)
    {
        return GetData(connectionString, str);
    }
    public static DataSet GetDataEx2(string connectionString, string str, params string[] param)
    {
        return GetData(connectionString, str);
    }

    public static int ExecuteData(string connectionString, string str, params IDataParameter[] sqlParams)
    {
        int rows = -1;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str, conn))
                {
                    cmd.CommandTimeout = conn.ConnectionTimeout;
                    if (sqlParams != null)
                    {
                        foreach (IDataParameter para in sqlParams)
                        {
                            cmd.Parameters.Add(para);
                        }
                        rows = cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
        return rows;
    }
}