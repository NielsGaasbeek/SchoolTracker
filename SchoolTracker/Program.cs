using System;
using System.IO;
using System.Windows.Forms;

namespace SchoolTracker
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

            string[] courses;

            using (StreamReader r = new StreamReader("../../data/courses.txt"))
            {
                courses = r.ReadToEnd().Split( new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            }


            Application.Run(new Form1(courses));
        }







    }
}
