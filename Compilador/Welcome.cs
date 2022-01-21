using System;
using System.Windows.Forms;

namespace Compilador
{
    /// <summary>
    /// The form that displays the main Welcome screen.  This allows the users to familiarize themselves with
    /// the program's logo, author, running version, etc.  This form is to be displayed during four seconds.
    /// Formula: (progressBar1.Maximum * tickTimer.Interval + finishTimer.Interval)ms
    /// Default: (300 * 10 + 1000)ms = 4s
    /// </summary>
    public partial class Welcome : Form
    {
        /// <summary>
        /// Creates and initializes the form.
        /// </summary>
        public Welcome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows the progress bar to advance one unit every tickTimer.Interval milliseconds.
        /// Default value: 10ms
        /// Bar stops when full.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value == progressBar1.Maximum)
            {
                tickTimer.Stop();
                finishTimer.Start();
            }
        }

        /// <summary>
        /// Gives an extra unit of time to allow for bar animation to complete, during finishTimer.Interval.
        /// Default value: 1000ms
        /// Disposes of picture to free resources, and closes this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            finishTimer.Stop();
            pictureBox1.BackgroundImage = null;
            pictureBox1.Dispose();
            this.Close();
        }
    }
}
