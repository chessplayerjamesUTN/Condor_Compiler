using System;
using System.Windows.Forms;

namespace Compilador
{
    /// <summary>
    /// The form that allows quick code insertion.
    /// </summary>
    public partial class TemplateSelector : Form
    {
        /// <summary>
        /// Used to hide the close button from the form.
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;

        /// <summary>
        /// The amount of units to increase the cursor index to a convenient position in the code text box.
        /// </summary>
        private int addCursorIndex;
        /// <summary>
        /// The selected index of the combo box.
        /// </summary>
        private int selectedIndex;
        /// <summary>
        /// The text to insert into the source code.
        /// </summary>
        private string insertText;


        /// <summary>
        /// Used to hide the close button from the form.
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
        /// Creates and initializes the form.
        /// </summary>
        public TemplateSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Whenever the select button is pressed.  If the text in the combo box is not one of the available options,
        /// an error is presented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if ((selectedIndex = cmbTemplate.SelectedIndex) < 0)
            {
                MessageBox.Show("Debe seleccionar un elemento de la lista.", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                TextSelection();
                this.Close();
            }
        }

        /// <summary>
        /// Runs when the cancel button is pressed.  Sets the selected index to -1, indicating that no option was selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedIndex = -1;
            this.Close();
        }

        /// <summary>
        /// Returns the currently selected index from the combo box.
        /// </summary>
        /// <returns></returns>
        public int GetSelectedIndex()
        {
            return this.selectedIndex;
        }

        /// <summary>
        /// Returns the amount of units to advance the cursor to a convenient location in the source code text box.
        /// </summary>
        /// <returns></returns>
        public int GetAddCursorIndex()
        {
            return this.addCursorIndex;
        }

        /// <summary>
        /// Returns the text to be inserted into the source code.
        /// </summary>
        /// <returns></returns>
        public string GetInsertText()
        {
            return this.insertText;
        }

        /// <summary>
        /// The default selected quick code insertion sequence.
        /// Default value: 6: Para/For
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateSelector_Load(object sender, EventArgs e)
        {
            cmbTemplate.SelectedIndex = 6;
        }

        /// <summary>
        /// Obtains the selected index in the combo box, inserts the correct text, and applies the correct value to the
        /// add cursor index variable.
        /// </summary>
        private void TextSelection()
        {
            switch (selectedIndex)
            {
                case 0://If
                    insertText = "si(i == 0):\n\t\nfin";
                    addCursorIndex = 13;
                    break;
                case 1://If-Else If
                    insertText = "si(i == 0):\n\t\nfin\nsinosi(i != 0):\n\t\nfin";
                    addCursorIndex = 13;
                    break;
                case 2://If-Else
                    insertText = "si(i == 0):\n\t\nfin\nsino:\n\t\nfin";
                    addCursorIndex = 13;
                    break;
                case 3://If-Else If-Else
                    insertText = "si(i == 0):\n\t\nfin\nsinosi(i != 0):\n\t\nfin sino:\n\t\nfin";
                    addCursorIndex = 13;
                    break;
                case 4://Switch
                    insertText = "selección(i):\n\tcaso 1:\n\t\t\n\tfin\n\tpredeterminado:\n\t\t\n\tfin\nfin";
                    addCursorIndex = 25;
                    break;
                case 5://Repeat
                    insertText += "repetir:\n\t\nfin (i)";
                    addCursorIndex = 10;
                    break;
                case 6://For
                    insertText += "para(ent i = 0; i < 10; i++):\n\t\nfin";
                    addCursorIndex = 31;
                    break;
                case 7://While
                    insertText += "mientras(i < 10):\n\t\nfin";
                    addCursorIndex = 19;
                    break;
                case 8://Do-While
                    insertText += "hacer:\n\t\nfin mientras(i < 10)";
                    addCursorIndex = 8;
                    break;
                default:
                    throw new Exception("Error insesperado");
            }
        }
    }
}
