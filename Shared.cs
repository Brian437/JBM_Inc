/*
 * Coded by Brian Chaves
 * March 31,2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using JBM_IncClasses;

namespace JBM_IncDAL
{
    internal static class Shared
    {
        //private static SqlConnection connection;

        //public static SqlConnection Connection
        //{
        //    get { return Shared.connection; }
        //    set { Shared.connection = value; }
        //}

        internal const string NOT_FOUND_STRING = "key values where not found";
    }
}
