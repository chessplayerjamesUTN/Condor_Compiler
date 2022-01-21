using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Compilador.Clases;
using Compilador.Classes;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using SharpUpdate;
using System.Net;

namespace Compilador
{
    /// <summary>
    /// Main form that shows controls to user, and performs front-end tasks and initializes back-end ones as well.
    /// </summary>
    public partial class frmCompiler : Form, ISharpUpdatable
    {
        /// <summary>
        /// The maximum amount of recent files to store.
        /// Default value: 10
        /// </summary>
        private const int MAXRECENTFILES = 10;
private const string surveyLink = "https://forms.office.com/Pages/ResponsePage.aspx?id=aRS-jZzHIU6dQ8pl2enEdTFpBHFzM7tBuBMr6LgSr3tUMUJVTDhZNDFGOE02VkdMUk9ZUVVNRDM0WC4u";

        /// <summary>
        /// Used with the Scintilla Text text box.
        /// </summary>
        private int maxLineNumberCharLength;
        /// <summary>
        /// Indicates whether or not the code will be auto-scanned for errors every certain interval of time.
        /// Default value: true
        /// </summary>
        private bool autoScan;
        /// <summary>
        /// A string that holds all of the reserved keywords.  Used for autocompletion.
        /// </summary>
        private string allKeywords;
        /// <summary>
        /// Indicates the user's name.
        /// </summary>
        private string name;
        /// <summary>
        /// Indicates the path of the source code file.
        /// </summary>
        private string path;
        /// <summary>
        /// Indicates the hash of the source code.  Used to instantly execute an already compiled program.
        /// This value isn't currently in use, but it is on the TODO list.
        /// </summary>
        private byte[] hash;
        /// <summary>
        /// A stack that holds the most recently opened and saved source code files.
        /// </summary>
        private Stack<string> recentFiles;
        /// <summary>
        /// The object that handles updates.
        /// </summary>
        private SharpUpdater updater;

        /// <summary>
        /// Returns the application's name.
        /// </summary>
        public string ApplicationName
        {
            get { return "Compilador"; }
        }

        /// <summary>
        /// Returns the application's ID.
        /// </summary>
        public string ApplicationID
        {
            get { return "Compilador"; }
        }

        /// <summary>
        /// Returns the application's assembly information.
        /// </summary>
        public Assembly ApplicationAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }

        /// <summary>
        /// Returns the location of the online XML document.
        /// </summary>
        public Uri UpdateXmlLocation
        {
            get { return new Uri("https://compiladorespanol.000webhostapp.com/update.xml"); }
        }

        /// <summary>
        /// Creates and initializes the form.  Shows and later disposes the welcome screen.
        /// </summary>
        public frmCompiler()
        {
            InitializeComponent();
            Welcome w = new Welcome();
            w.ShowDialog();
            w.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Code that runs when loading the form.  Initializes main objects, loads recent files list, sets interface
        /// and control dimensions, loads required compiler files, applies code styling to text box, presents disclaimer,
        /// and initializes update sequence.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(""));
            updater = new SharpUpdater(this);
            Semantic.debugVariableList = new List<string>();
            path = "";
            autoScan = true;
            LoadRecentFiles();
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            this.Size = new Size((int)(resolution.Width * 0.9), (int)(resolution.Height * 0.9));
            ResizeFormElements();
            ReadMainFiles();
            CodeStyling();
            LoadName();
            DialogResult result = MessageBox.Show(@"Mensaje del creador del programa:

Este programa está todavía en fases de prueba.  Al usarlo, usted reconoce y acepta que el código que escriba será enviado"
+ @"al creador para el análisis y recopilación de datos respectivos.

¡Se le agradece mucho por su participación!

¿Entiende y acepta los términos descritos?  ¿Desea continuar?", "Mensaje Importante", MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                Application.Exit();
            }
                updater.DoUpdate();
        }

        /// <summary>
        /// Runs every time text is changed within the txtCode text box.
        /// Special functions within the Scintilla Text control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scintilla1_TextChanged(object sender, EventArgs e)
        {
            int currentLineCount = txtCode.Lines.Count.ToString().Length;
            if (currentLineCount == this.maxLineNumberCharLength)
            {
                return;
            }
            const int padding = 2;
            txtCode.Margins[0].Width = txtCode.TextWidth(Style.LineNumber, new string('9', currentLineCount + 1)) + padding;
            maxLineNumberCharLength = currentLineCount;
        }

        /// <summary>
        /// Does a full compilation and execution of inserted code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRun_Click(object sender, EventArgs e)
        {
            Semantic.debugging = TSMIEnableDebug.Checked;
            ScanCompile(true);
        }

        /// <summary>
        /// Runs every time text is changed within the txtCode text box.
        /// Special functions within the Scintilla Text control.
        /// Shows the autocompletion box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_CharAdded(object sender, CharAddedEventArgs e)
        {
            var currentPos = txtCode.CurrentPosition;
            var wordStartPos = txtCode.WordStartPosition(currentPos, true);
            var lenEntered = currentPos - wordStartPos;
            if ((lenEntered > 0) && !txtCode.AutoCActive)
            {
                txtCode.AutoCShow(lenEntered, allKeywords);
            }
        }

        /// <summary>
        /// Opens an existing source code file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        /// <summary>
        /// Saves as the most recenlty used file, or prompts user to save as.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        /// <summary>
        /// Opens an existing source code file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        /// <summary>
        /// Opens a source code file, prompting the user to choose it.
        /// Updates the stack and restarts the auto scan timer (if activated).
        /// </summary>
        private void Open()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader read = new StreamReader(openFileDialog1.FileName);
                txtCode.Text = read.ReadToEnd();
                read.Close();
                path = openFileDialog1.FileName;
                UpdateStack(path);
                tblErrors.Rows.Clear();
                if (autoScan)
                {
                    TimerRestart();
                }
            }
        }

        /// <summary>
        /// Opens a source code file, without an Open File prompt.  Indicates whether the stack should be updated or not.
        /// Restarts the auto scan timer (if activated).
        /// </summary>
        /// <param name="filePath">The location of the file to open.</param>
        /// <param name="updateStack">Indicates whether the recently accessed files stack should be updated or not.</param>
        private void Open(string filePath, bool updateStack)
        {
            StreamReader read = new StreamReader(filePath);
            txtCode.Text = read.ReadToEnd();
            read.Close();
            if (updateStack)
            {
                UpdateStack(filePath);
            }
            if (autoScan)
            {
                TimerRestart();
            }
        }

        /// <summary>
        /// Closes the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Saves the current source code into an .sms file.
        /// Save prompt will not be shown if the file path has been previously set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        /// <summary>
        /// Saves the file.  If saveAs is true, or if the path hasn't been set, the save prompt will be displayed.
        /// Updates the recent files stack.  Prompts the user that the file was successfully saved.
        /// </summary>
        /// <param name="saveAs">Indicates whether the showing the save prompt is mandatory or not.
        /// True: Shows the save prompt whether the path has previosuly been set or now.
        /// False: If the path has already been set, the save prompt is not shown.</param>
        private void Save(bool saveAs)
        {
            if (saveAs || path.Length == 0)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(saveFileDialog1.FileName);
                    path = saveFileDialog1.FileName;
                    UpdateStack(path);
                    MessageBox.Show("Archivo guardado con éxito.", "¡Éxito!");
                }
            }
            else
            {
                SaveFile(path);
            }
        }

        /// <summary>
        /// Saves a source code file with the given file path.
        /// </summary>
        /// <param name="filePath">The absolute or relative file path indicating where to save
        /// the source code file.</param>
        private void SaveFile(string filePath)
        {
            string code = txtCode.Text;
            code = code.Replace("\r", "");
            StreamWriter write = new StreamWriter(filePath);
            write.Write(code);
            write.Close();
        }

        /// <summary>
        /// Saves the file, with a mandatory save prompt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        /// <summary>
        /// Loads the recently accessed source code files and saves them in the recently used files list.
        /// </summary>
        private void PopulateRecentFiles()
        {
            TSMIRecentFiles.DropDownItems.Clear();
            List<string> recentFileList = new List<string>(recentFiles);
            for (int i = 0; i < recentFileList.Count; i++)
            {
                ToolStripMenuItem tsmitem = new ToolStripMenuItem();
                tsmitem.Text = recentFileList[i];
                tsmitem.Tag = i;
                tsmitem.Click += new EventHandler(processMeunItem);
                TSMIRecentFiles.DropDownItems.Add(tsmitem);
            }
        }

        /// <summary>
        /// Updates the recently accessed files stack with the newest addition.  Also writes the stack to a text file.
        /// </summary>
        /// <param name="filePath">The currently accessed file path.</param>
        private void UpdateStack(string filePath)
        {
            List<string> recentFileList;
            if (recentFiles.Count > 0)
            {
                if (recentFiles.Contains(filePath) && (recentFiles.Peek() != filePath))
                {
                    recentFileList = new List<string>(recentFiles);
                    recentFileList.Remove(filePath);
                    recentFiles = new Stack<string>();
                    for (int i = recentFileList.Count - 1; i >= 0; i--)
                    {
                        recentFiles.Push(recentFileList[i]);
                    }
                    recentFiles.Push(filePath);
                }
                else if (recentFiles.Peek() != filePath)
                {
                    recentFiles.Push(filePath);
                }
                else
                {
                    return;
                }
            }
            else
            {
                recentFiles.Push(filePath);
            }
            PopulateRecentFiles();
            recentFileList = new List<string>(recentFiles);
            WriteRecentFiles(recentFileList);
        }

        /// <summary>
        /// Opens a file in the recently accessed files stack.  Open prompt is not required.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void processMeunItem(object sender, EventArgs e)
        {
            int selectedMenuTag = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            string filePath = TSMIRecentFiles.DropDownItems[selectedMenuTag].Text;
            Open(filePath, true);
        }

        /// <summary>
        /// Restarts the auto scan timer every time a key is pressed within the source code text box,
        /// if auto scan is active.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (autoScan)
            {
                TimerRestart();
            }
        }

        /// <summary>
        /// When auto scan is enabled and the timer ticks, it is stopped and the code analyzed for errors,
        /// without saving code to disk, doing a second compilation, or executing the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrScanTime_Tick(object sender, EventArgs e)
        {
            tmrScanTime.Stop();
            ScanCompile(false);
        }

        /// <summary>
        /// Scans the source code for errors, and compiles and executes if desired.
        /// </summary>
        /// <param name="compile">Indicates whether or not the scanned code should be compiled and executed.
        /// True: Scanned code is compiled and executed, and sent to database.
        /// False: Code is only scanned.</param>
        private void ScanCompile(bool compile)
        {
            tmrScanTime.Stop();
            Error.errorsFound.Clear();
            Semantic.compile = compile;
            string source = txtCode.Text;
            if (compile)
            {
                btnRun.Enabled = false;
                Compile(source, compile);
                string errors = "\n\n";
                foreach (Error e in Error.errorsFound)
                {
                    errors += e.id + " " + e.description + ":" + e.incorrectText + "\n";
                }
                errors += "\n\n" + LALR.errorMessage;
                if (DataBaseConnection.userFound)
                {
                    tmrReEnableCompile.Start();
                    DataBaseConnection.errors = Error.errorsFound;
                    DataBaseConnection.code = source;
                    Thread t = new Thread(new ThreadStart(DataBaseConnection.SaveCompiledCode));
                    t.Start();
                }
                else
                {
                    btnRun.Enabled = true;
                }
            }
            else
            {
                Compile(source, compile);
            }
        }

        /// <summary>
        /// Opens a new file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            path = "";
            txtCode.Text = "";
        }

        /// <summary>
        /// The scan button is explicitly clicked and code is scanned, without compiling.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, EventArgs e)
        {
            ScanCompile(false);
        }

        /// <summary>
        /// When right-click is pressed the quick-code-insertion form is displayed, and if an option is selected,
        /// that code is inserted into the source code text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TemplateSelector ts = new TemplateSelector();
                ts.ShowDialog();
                if (ts.GetSelectedIndex() >= 0)
                {
                    int cursorIndex = txtCode.CurrentPosition;
                    string startText = txtCode.Text.Substring(0, cursorIndex);
                    string insertText = ts.GetInsertText();
                    string endText = txtCode.Text.Substring(cursorIndex);
                    cursorIndex += ts.GetAddCursorIndex();
                    txtCode.Text = startText + insertText + endText;
                    txtCode.GotoPosition(cursorIndex);
                    txtCode.Focus();
                    txtCode.Select();
                }
            }
        }

        /// <summary>
        /// Resizes the elements when the form size is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ResizeFormElements();
        }

        /// <summary>
        /// Changes the size of the txtCode text box and the tblErrors data grid based on current form dimensions.
        /// </summary>
        private void ResizeFormElements()
        {
            txtCode.Size = new Size(this.Size.Width - 40, (this.Size.Height - 130) / 2);
            tblErrors.Size = txtCode.Size;
            int width = tblErrors.Size.Width;
            tblErrors.Location = new Point(txtCode.Location.X, txtCode.Location.Y + 10 + txtCode.Size.Height);
            tblErrors.Columns[0].Width = (int)(width * 0.1);
            tblErrors.Columns[1].Width = (int)(width * 0.1);
            tblErrors.Columns[2].Width = (int)(width * 0.2);
            tblErrors.Columns[3].Width = (int)(width * 0.55);
        }

        /// <summary>
        /// Activates and deactivates the auto-scan feature.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void análisisDeCódigoEnTiempoRealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmrScanTime.Stop();
            autoScan = TSMIAutoScan.Checked;
            if (autoScan)
            {
                tmrScanTime.Start();
            }
        }

        /// <summary>
        /// Applies code styling to the Scintilla Text text box.
        /// </summary>
        private void CodeStyling()
        {
            txtCode.Margins[0].Width = 16;
            txtCode.SetKeywords(0, "entero ent decimal dec carácter caracter car texto tex lógico logico log textura ras"
                + " col color si sinosi sino selección seleccion caso predeterminado fin mientras hacer repetir para"
                + " salir nada retornar dibujar intentar error");
            txtCode.SetKeywords(1, "verdad falso negro azul verde celeste rojo rosado amarillo blanco pi sen cos tan"
                + " potencia esperar raízcuadrada raizcuadrada leer escribir convertir reproducir sonido leerras leertexto"
                + " escribirtexto dibujarras longitud azar convertirEnt convertirDec convertirTex convertirCar convertirLog");
            txtCode.StyleResetDefault();
            txtCode.Styles[Style.Cpp.Default].Font = "Consolas";
            txtCode.Styles[Style.Cpp.Default].Size = 11;
            txtCode.Styles[Style.Cpp.Default].ForeColor = Color.FromArgb(156, 156, 156);
            txtCode.StyleClearAll();
            txtCode.Styles[Style.Cpp.CommentLine].Font = "Consolas";
            txtCode.Styles[Style.Cpp.CommentLine].Size = 10;
            txtCode.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 200, 0);
            txtCode.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(200, 0, 0);
            txtCode.Styles[Style.Cpp.String].Size = 11;
            txtCode.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(200, 0, 200);
            txtCode.Styles[Style.Cpp.Character].Size = 11;
            txtCode.Styles[Style.Cpp.Identifier].ForeColor = Color.FromArgb(0, 200, 200);
            txtCode.Styles[Style.Cpp.Identifier].Size = 11;
            txtCode.Styles[Style.Cpp.Operator].ForeColor = Color.FromArgb(0, 0, 0);
            txtCode.Styles[Style.Cpp.Operator].Size = 11;
            txtCode.Styles[Style.Cpp.Number].ForeColor = Color.FromArgb(200, 200, 0);
            txtCode.Styles[Style.Cpp.Number].Size = 11;
            txtCode.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(0, 0, 200);
            txtCode.Styles[Style.Cpp.Word].Size = 11;
            txtCode.Styles[Style.Cpp.Word2].ForeColor = Color.FromArgb(225, 100, 0);
            txtCode.Styles[Style.Cpp.Word2].Size = 11;
            txtCode.Indicators[0].Style = IndicatorStyle.Squiggle;
            txtCode.Indicators[0].ForeColor = Color.Red;
            txtCode.IndicatorCurrent = 0;
        }

        /// <summary>
        /// Restarts the scan timer.
        /// </summary>
        private void TimerRestart()
        {
            tmrScanTime.Stop();
            tmrScanTime.Start();
        }

        /// <summary>
        /// Reads the main files required for compilation, such as the lexical automaton, syntax automaton, etc.
        /// </summary>
        private void ReadMainFiles()
        {
            LexicalAnalyzer.SetAutomaton(Automaton.LoadAutomaton(".\\Archivos\\EstadosTransición"));
            Lexeme.ReadLexemeCSV(".\\Archivos\\Lexemes");
            Keyword.ReadKeywordsCSV(".\\Archivos\\Keywords");
            allKeywords = Keyword.GetAllKeywords();
            try
            {
                Error.ReadErrorsTXT(".\\Archivos\\Errors2");
            }
            catch
            {
                WebClient wc = new WebClient();
                wc.DownloadFile("https://compiladorespanol.000webhostapp.com/Errors.txt", ".\\Archivos\\Errors2.txt");
                Error.ReadErrorsTXT(".\\Archivos\\Errors2");
            }
            LALR.ReadLALR1CSV(".\\Archivos\\LALR1");
            LALR.ReadLALR2CSV(".\\Archivos\\LALR2");
            LALR.ReadRulesCSV(".\\Archivos\\Rules");
        }

        /// <summary>
        /// Loads the recent file list and populates the menu strip item.
        /// </summary>
        private void LoadRecentFiles()
        {
            recentFiles = new Stack<string>(10);
            StreamReader read = new StreamReader(".\\Archivos\\options.txt");
            while (!read.EndOfStream)
            {
                recentFiles.Push(read.ReadLine());
            }
            read.Close();
            PopulateRecentFiles();
        }

        /// <summary>
        /// Writes the recent files list to disk.
        /// </summary>
        /// <param name="tempArray"></param>
        private void WriteRecentFiles(List<string> tempArray)
        {
            StreamWriter write = new StreamWriter(".\\Archivos\\options.txt");
            for (int i = Math.Min(tempArray.Count, MAXRECENTFILES) - 1; i >= 0; i--)
            {
                write.WriteLine(tempArray[i]);
            }
            write.Close();
        }

        /// <summary>
        /// Calculates the new hash of code and compares with previous compilated code.
        /// This currently isn't in use, but is on the TODO list.
        /// This can be used to speed-up compilation process.
        /// </summary>
        /// <param name="source">The source code to compile.</param>
        /// <returns></returns>
        private bool CalculateVerifyHash(string source)
        {
            byte[] newHash;
            newHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(source));
            for (int i = 0; i < newHash.Length; i++)
            {
                if (hash[i] != newHash[i])
                {
                    hash = newHash;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Populates the error table after a code scan or code compilation attempt.
        /// </summary>
        private void PopulateErrorTable()
        {
            for (int i = 0; i < Error.errorsFound.Count; i++)
            {
                Error error = Error.errorsFound[i];
                if (error.type == 2)
                {
                    string description = error.description;
                    if (description.Contains(":"))
                    {
                        description = description.Replace(',', '\n');
                        description = description.Substring(0, description.LastIndexOf('.', description.Length - 2)) + '\n'
                            + description.Substring(description.LastIndexOf('.', description.Length - 2) + 1);
                    }
                    tblErrors.Rows.Add(new object[] { (i + 1), error.lineNum, Error.TYPES[error.type], error.id + " "
                        + description + ": " + error.incorrectText });
                }
                else tblErrors.Rows.Add(new object[] { (i + 1), error.lineNum, Error.TYPES[error.type], error.id + " "
                    + error.description + ": " + error.incorrectText });
                int start = 0;
                while ((error.type == 1) && (start < txtCode.Text.Length)
                    && (txtCode.Text.IndexOf(error.incorrectText, start) >= 0))
                {
                    txtCode.IndicatorFillRange(txtCode.Text.IndexOf(error.incorrectText, start), error.length);
                    start = txtCode.Text.IndexOf(error.incorrectText, start) + 1;
                }
            }
        }

        /// <summary>
        /// Opens the first manual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conociendoElEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(".\\Archivos\\Extras\\Manual 1 - Conociendo el Editor.pdf");
        }

        /// <summary>
        /// Opens the second manual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conociendoElLenguajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(".\\Archivos\\Extras\\Manual 2 - Conociendo el lenguaje.pdf");
        }

        /// <summary>
        /// Opens the exercises document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ejerciciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(".\\Archivos\\Extras\\Ejercicios.pdf");
        }

        /// <summary>
        /// Opens example code 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datosDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\1.sms", false);
        }

        /// <summary>
        /// Opens example code 2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void promedioDeNotasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\2.sms", false);
        }

        /// <summary>
        /// Opens example code 3.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void carroEnMovimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\3.sms", false);
        }

        /// <summary>
        /// Opens example code 4.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sonidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Beep.sms", false);
        }

        /// <summary>
        /// Opens example code 6.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graficaciónDeFuncionesTrigonométricasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Draw4.sms", false);
        }

        /// <summary>
        /// Opens example code 7.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generaciónDeRuidoVisualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Draw5.sms", false);
        }

        /// <summary>
        /// Opens example code 8.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serieFibonacciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Fibonacci.sms", false);
        }

        /// <summary>
        /// Opens example code 9.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sufragioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Voto.sms", false);
        }

        /// <summary>
        /// Opens example code 10.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void funciónPorPartesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\FuncionPartes.sms", false);
        }

        /// <summary>
        /// Shows About information, such as author, year of creation, version number, and user ID if available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = @"Programa creado por James Scarberry - 2021

Compilador Cóndor
Version 1.2.1.2

" + name;
            if (DataBaseConnection.userFound)
            {
                message += "\nID de Usuario: " + DataBaseConnection.userId;
            }
            MessageBox.Show(message);
        }

        /// <summary>
        /// This ends the Re-enable compile timer.  The purpose of this is to give ample time for sending information to
        /// database before allowing a subsequent compilation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrReEnableCompile_Tick(object sender, EventArgs e)
        {
            btnRun.Enabled = true;
            tmrReEnableCompile.Stop();
        }

        /// <summary>
        /// When the form is being closed, it performs the closing sequence.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProgramClose();
        }

        /// <summary>
        /// Opens the survey for the user to fill out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encuestaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(surveyLink);
        }

        /// <summary>
        /// Opens example code 5.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(".\\Archivos\\Extras\\Sort.sms", false);
        }

        /// <summary>
        /// Enables and disables debugging features.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Semantic.debugging = TSMIEnableDebug.Checked;
            TSMIChooseVariables.Enabled = TSMIEnableDebug.Checked;
        }

        /// <summary>
        /// Shows the variable selection dialogue for debugging.  Scans for variable names first.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void escogerVariablesAAnalizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanCompile(false);
            VariableSelection vs = new VariableSelection();
            vs.ShowDialog();
        }

        /// <summary>
        /// Compiles (or scans) code, and executes if necessary.
        /// The error table is also filled with any errors that were found.
        /// Implements some methods to overcome certain errors during compilation.
        /// </summary>
        /// <param name="source">The source code to compile.</param>
        /// <param name="compile">Indicates whether or not the scanned code should be compiled and executed.
        /// True: Scanned code is compiled and executed, and sent to database.
        /// False: Code is only scanned.</param>
        private void Compile(string source, bool compile)
        {
            source = LexicalAnalyzer.StripTabsComments(source);
            tblErrors.Rows.Clear();
            LexicalAnalyzer.Reset(source);
            bool syntaxWorked = LALR.SyntaxAnalysis(true);
            //If signed numbers gives an error (+3 instead of + 3), this will count them as two different tokens
            if (!syntaxWorked)
            {
                Error.errorsFound.Clear();
                LexicalAnalyzer.Reset(source);
                LexicalAnalyzer.DisableSignedLiterals();
                syntaxWorked = LALR.SyntaxAnalysis(true);
            }
            if (compile)
            {
                if (!syntaxWorked)
                {
                    MessageBox.Show("Error!");
                    DataBaseConnection.worked = 0;
                }
                else if (!Executor.successfulCompilation)//Eliminates try-catch sequences from code and attempts to recompile.
                {
                    LexicalAnalyzer.Reset(source);
                    LALR.SyntaxAnalysis(false);
                    if (Executor.successfulCompilation)
                    {
                        DataBaseConnection.worked = 1;
                    }
                    else
                    {
                        DataBaseConnection.worked = 0;
                    }
                }
                else
                {
                    DataBaseConnection.worked = 1;
                }
            }
            PopulateErrorTable();
        }

        /// <summary>
        /// Attempts to load the user's name.  If the username isn't found, it prompts the userto enter it.
        /// Also, attempts to obtain user's ID from the database, on a separate thread.
        /// </summary>
        private void LoadName()
        {
            name = "";
            try
            {
                StreamReader read = new StreamReader("name.txt");
                name = read.ReadLine();
                read.Close();
            }
            catch
            {
                Name n = new Name();
                n.ShowDialog();
                name = n.name;
            }
            tmrReEnableCompile.Start();
            DataBaseConnection.userName = name;
            Thread t = new Thread(new ThreadStart(DataBaseConnection.GetUserID));
            t.Start();
        }

        /// <summary>
        /// The closing sequence.  Prompts user to fill the survey, if they haven't already accepted.
        /// </summary>
        private void ProgramClose()
        {
            bool worked;
            try
            {
                StreamReader read = new StreamReader(".\\Encuesta.txt");
                read.Close();
                worked = true;
            }
            catch
            {
                worked = false;
            }
            if (!worked)
            {
                DialogResult dr = MessageBox.Show(@"¡Gracias por probar Compilador Cóndor!
¿Tiene un momento para realizar una encuesta?  Solo se requiere de cinco a diez minutos de su tiempo.", "Encuesta",
                MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(surveyLink);
                    StreamWriter write = new StreamWriter(".\\Encuesta.txt");
                    write.WriteLine(" ");
                    write.Close();
                }
            }
        }
    }
}
