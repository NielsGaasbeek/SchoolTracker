using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace SchoolTracker
{
    public partial class Form1 : Form
    {
        // variable used to determine the text on the start/stop-button
        bool clicked;


        public Form1(string[] dropdown)
        {
            InitializeComponent();

            clicked = false;

            // Contains all the courses specified in the courses.txt-file
            foreach (string s in dropdown)
                dropDown.Items.Add(s);


            // All possible activities when using the timer
            actionSelector.Items.Add("Tentamen leren");
            actionSelector.Items.Add("Lezen");
            actionSelector.Items.Add("Opgaven maken");
            actionSelector.Items.Add("Practicum werken");
            actionSelector.Items.Add("Herhalen");
            actionSelector.Items.Add("Overig");
            actionSelector.Items.Add("College");
        }


        private void startButton_Click(object sender, EventArgs e)
        {
            clicked = !clicked;
            startButton.Text = GetMessage(clicked);

            if (clicked) StartTimer(); else StopTimer();


        }

        string GetMessage(bool on) => on ?  "Stop Timer" : "Start Timer";


        DateTime startTime;
        string currentSession;
        string curCourse;
        bool inProgress = false;

        void StartTimer()
        {
            inProgress = true;

            startTime = DateTime.Now;
            curCourse = (string)dropDown.SelectedItem;


            if ((string)actionSelector.SelectedItem == "College")
            {
                Calendar cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                int curWeek = cal.GetWeekOfYear(startTime, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);

                currentSession = $"Date;{startTime},{startTime.DayOfWeek}-Week;{curWeek}-Time;01:45:00-Activity;{currentAction}//";

                using (StreamWriter sw = new StreamWriter($"../../data/{curCourse}.txt",true))
                    sw.WriteLine(currentSession);

                currentAction = "";
                currentSession = "";

                inProgress = false;
                clicked = false;
            }
        }

        void StopTimer()
        {
            DateTime endTime = DateTime.Now;

            TimeSpan time = endTime.Subtract(startTime);

            Calendar cal = DateTimeFormatInfo.CurrentInfo.Calendar;
            int curWeek = cal.GetWeekOfYear(startTime, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);

            currentSession = $"Date;{startTime},{startTime.DayOfWeek}-Week;{curWeek}-Time;{time}-Activity;{currentAction}//";
            //Console.WriteLine(currentSession);


            using (StreamWriter sw = new StreamWriter($"../../data/{curCourse}.txt", true))
                sw.WriteLine(currentSession);

        }

        private void addCourseButton_Click(object sender, EventArgs e)
        {
            string newCourse = textBox1.Text;
            textBox1.Text = "";



            if (!dropDown.Items.Contains(newCourse))
            {
                using (StreamWriter sw = new StreamWriter($"../../data/{newCourse}.txt", true))
                    sw.WriteLine($"Created on: {DateTime.Now}");

                using (StreamWriter sw = new StreamWriter($"../../data/courses.txt", true))
                    sw.Write($";{newCourse}");
            }
            else return;

            dropDown.Items.Add(newCourse);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inProgress)
                StopTimer();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        string currentAction;

        private void actionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curAction = (string)actionSelector.SelectedItem;

            currentAction = curAction;
        }
    }
}
