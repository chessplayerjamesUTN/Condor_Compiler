using System.Collections.Generic;
using System.Data.SqlClient;
using Compilador.Clases;

namespace Compilador.Classes
{
    /// <summary>
    /// The class that manages custom connections to the data base stored on Azure.
    /// </summary>
    public class DataBaseConnection
    {
        private const string password =
            "AB*Kj9$gWf%SAtWwF#TXweArj!QMmlR$IN2ik$RVbC6MRn@k%kC%Cw79O$g&lngA6wHgkr&AW6#qGPee8aGs*feK3uPGU%EnXzB";
        private const string connectionString = "Server=tcp:compiladorcondorserver.database.windows.net,1433;"
                + "Initial Catalog=CompiladorCondorD;Persist Security Info=False;User ID=jwscarberry;Password=" + password
                + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=15;";

        //Variables required for SQL operations.
        private static SqlDataReader dataReader;
        private static SqlConnection cnn;
        private static SqlCommand command;

        /// <summary>
        /// Indicates whether user has been found yet or not.
        /// </summary>
        public static bool userFound = false;

        /// <summary>
        /// User's ID in data base.
        /// </summary>
        public static short userId = 0;

        /// <summary>
        /// User's username, per the name they entered at prompt.
        /// </summary>
        public static string userName;

        /// <summary>
        /// Code being compiled.
        /// </summary>
        public static string code;

        /// <summary>
        /// Indicates whether code was successfully compiled or not.
        /// </summary>
        public static byte worked;

        /// <summary>
        /// The list of errors generated with the compiled code.
        /// </summary>
        public static List<Error> errors;
        
        
        //public static void TestConnection()
        //{
        //    bool worked;
        //    SqlConnection cnn = new SqlConnection(connectionString);
        //    try
        //    {
        //        cnn.Open();
        //        worked = true;
        //    }
        //    catch
        //    {
        //        worked = false;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    MessageBox.Show(worked.ToString()); ;
        //}

        /// <summary>
        /// Attempts to obtain the user's ID, based off of their name.  If user isn't found in data base, they are added.
        /// </summary>
        public static void GetUserID()
        {
            cnn = new SqlConnection(connectionString);
            string sql = "SELECT COUNT(*) FROM user_compiler u WHERE u.name_user = '"
                + userName + "';"; //See if user exists or not.
            try
            {
                cnn.Open();
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int count = dataReader.GetInt32(0);
                dataReader.Close();
                if (count == 0) //If user hasn't been found, insert it.
                {
                    sql = "INSERT INTO user_compiler (name_user) VALUES ('" + userName + "');";
                    command = new SqlCommand(sql, cnn);
                    command.ExecuteNonQuery();
                }
                sql = "SELECT id_user FROM user_compiler u WHERE u.name_user = '" + userName + "';"; //Obtain user id.
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                userId = dataReader.GetInt16(0);
                userFound = true;
                cnn.Close();
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// Stores source code in data base, along with any errors that have been found.
        /// </summary>
        public static void SaveCompiledCode()
        {
            cnn = new SqlConnection(connectionString);
            string sql = "INSERT INTO compiled_code (id_user, text_code, compile_time, successful) VALUES (" + userId + 
                ",'" + code.Replace("'", "''") + "',GETDATE()," + worked + ");"; //Stores compiled source code and other
                //useful information into data base.
            try
            {
                cnn.Open();
                command = new SqlCommand(sql, cnn);
                command.ExecuteNonQuery();
                sql = "SELECT TOP 1 id_code FROM compiled_code WHERE id_user = " + userId + " AND text_code = '" 
                   + code.Replace("'", "''") + "' ORDER BY id_code DESC;"; //Obtain ID of newly inserted code into data base.
                int codeId;
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                codeId = dataReader.GetInt32(0);
                dataReader.Close();
                foreach (Error e in errors)
                {
                    sql = "INSERT INTO error (id_code, num_error) VALUES(" + codeId + "," + e.id + ");"; //Insert error IDs
                        //into data base, for compiled code.
                    command = new SqlCommand(sql, cnn);
                    command.ExecuteNonQuery();
                }
                cnn.Close();
            }
            catch
            {
                
            }
        }

    }
}
