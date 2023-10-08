using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer.Models
{
    class DbUtils
    {
        public static DB db;

        static DbUtils()
        {
            try
            {
                db = new DB();
            }
            catch (Exception) { MessageBox.Show("Error of connection to database", MessageBoxButton.OK.ToString()); }
        }
    }
}
