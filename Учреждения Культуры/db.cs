using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Учреждения_Культуры
{
    public class db
    {
        public string Names;
        public string Types;
        public string Adress;
        public string Representative;
        public string info;
        public string Person;
        public string Status = "Не рассмотрено";

        public string login = "";
        public string email = "";
        public string pass = "";
        static string serverName = @"DESKTOP-UCA27K4\SQLSERVER";
        static string dbName = "Auto_System";

        public SqlConnection con = new SqlConnection($@"Data Source={serverName}; Initial Catalog={dbName};Integrated Security = True");
    }
}
