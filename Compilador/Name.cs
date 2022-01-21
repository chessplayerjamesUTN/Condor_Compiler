using System;
using System.IO;
using System.Windows.Forms;

namespace Compilador
{
    /// <summary>
    /// The form that obtains the user's name.  It is not closeable until a name is entered.
    /// This form is executed on the program's first run (if name.txt is available).
    /// </summary>
    public partial class Name : Form
    {
        /// <summary>
        /// Indicates the minimum required length for the entered name.
        /// </summary>
        private const byte MINIMUM_NAME_LENGTH = 6;
        /// <summary>
        /// Used for hiding the close button.
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;
        /// <summary>
        /// Indicates where the file should be written to.
        /// </summary>
        private const string PATH = "name.txt";

        /// <summary>
        /// A publicly accessible field, storing the user's name.
        /// </summary>
        public string name;

        /// <summary>
        /// Used to hide the close button.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        /// <summary>
        /// Initializes and creates the form.
        /// </summary>
        public Name()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Writes username to disk and stores it for future access, if name length is sufficient.
        /// Default MINIMUL_NAME_LENGTH value: 6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length >= MINIMUM_NAME_LENGTH)
            {
                StreamWriter write = new StreamWriter(PATH);
                write.WriteLine(txtName.Text);
                write.Close();
                this.Close();
                name = txtName.Text;
            }
            else
            {
                MessageBox.Show("Longitud de nombre no es suficiente.  Intente de nuevo.  Ingrese su nombre completo.");
            }
        }
    }
}
