using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace quanlythuvien
{
    public class qltt
    {
        protected static string _connectString =
            "server=.;database=QuanLyThuVien;integrated security=SSPI";

        // SELECT
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connect = new SqlConnection(_connectString))
            using (SqlCommand command = new SqlCommand(sql, connect))
            {
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    connect.Open();
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        // INSERT, UPDATE, DELETE (không tham số)
        public static void ExecuteNonQuery(string sql)
        {
            using (SqlConnection connect = new SqlConnection(_connectString))
            using (SqlCommand command = new SqlCommand(sql, connect))
            {
                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        // INSERT, UPDATE, DELETE (có tham số)
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connect = new SqlConnection(_connectString))
            using (SqlCommand command = new SqlCommand(sql, connect))
            {
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                connect.Open();
                return command.ExecuteNonQuery();
            }
        }
    }

}