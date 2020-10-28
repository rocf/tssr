using System;
using System.Collections.Generic;
using System.Text;

namespace SX.TSSR.Common
{
    public class Constants
    {
        public static string DefaultCorsPolicy = nameof(DefaultCorsPolicy);

        //public static string SqlServerConnectionString = "Data Source=127.0.0.1;Initial Catalog=iss_support;Persist Security Info=True;User ID=iss_support;Password=siss";
        public static string SqlServerConnectionString = "Data Source=127.0.0.1;Initial Catalog=iss_support;Persist Security Info=True;User ID=iss_support;Password=siss";

        public static IEnumerable<string> Trades = new List<string>() { "1", "2", "3", "6", "8", "H", "I", "C", "G", "L", "M" };

    }
}
