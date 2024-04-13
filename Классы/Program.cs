using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace admission_commision
{ 
    enum RowState
{
    Existed,
    New,
    Modified,
    ModifiedNew,
    Deleted
}
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SQLClass.conn = new MySql.Data.MySqlClient.MySqlConnection();
            SQLClass.conn.ConnectionString = admission_commision.Properties.Settings.Default.MySQLConn;
            SQLClass.conn.Open();
            Application.Run(new Entry());
            SQLClass.conn.Close();
        }
    }
}
