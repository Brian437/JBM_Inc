/*
 * Coded by Brian Chaves
 * March 31,2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace JBM_IncDAL
{
    public static class DAL_Connection
    {
        private static SqlConnection connection;

        public static SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }
    }
}
