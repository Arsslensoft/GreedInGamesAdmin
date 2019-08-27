using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Admins.Initialize();
            Application.Run(new Form1());
        }
    }
}
