using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Compilador.Clases;
using Compilador.Classes;

namespace Compilador
{
    /// <summary>
    /// Allows the desired variables to be selected and "debugged."
    /// </summary>
    public partial class VariableSelection : Form
    {
        /// <summary>
        /// Creates and initializes the form.
        /// </summary>
        public VariableSelection()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the list of currently available variables, and shows them.
        /// Previously checked variable names will remain checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VariableSelection_Load(object sender, EventArgs e)
        {
            clbVariables.Items.Clear();
            foreach(Symbol symbol in Symbol.symbolTable)
            {
                if (!clbVariables.Items.Contains(symbol.identifier))
                {
                    clbVariables.Items.Add(symbol.identifier);
                }
                clbVariables.SetItemChecked(clbVariables.Items.IndexOf(symbol.identifier),
                    Semantic.debugVariableList.Contains(symbol.identifier));
            }
        }

        /// <summary>
        /// Saves the list of checked variable names to the Semantic debug list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            Semantic.debugVariableList.Clear();
            foreach(object o in clbVariables.CheckedItems)
            {
                Semantic.debugVariableList.Add(o.ToString());
            }
            this.Close();
        }

        /// <summary>
        /// Instantly selects all variable names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbVariables.Items.Count; i++)
            {
                clbVariables.SetItemChecked(i, true);
            }
        }
    }
}
