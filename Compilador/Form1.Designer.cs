
namespace Compilador
{
    partial class frmCompiler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompiler));
            this.txtCode = new ScintillaNET.Scintilla();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.TSMIFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINew = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISave = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIClose = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIEnableDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIChooseVariables = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIAutoScan = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIManuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIManual1 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIManuals2 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExercises = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExamples = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample1 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample2 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample3 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample4 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample5 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample6 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample7 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample8 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample9 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIExample10 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISurvey = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tblErrors = new System.Windows.Forms.DataGridView();
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tmrScanTime = new System.Windows.Forms.Timer(this.components);
            this.tmrReEnableCompile = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.EolMode = ScintillaNET.Eol.Lf;
            this.txtCode.Lexer = ScintillaNET.Lexer.Cpp;
            this.txtCode.Location = new System.Drawing.Point(12, 75);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(754, 165);
            this.txtCode.TabIndex = 0;
            this.txtCode.UseTabs = true;
            this.txtCode.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.txtCode_CharAdded);
            this.txtCode.TextChanged += new System.EventHandler(this.Scintilla1_TextChanged);
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCode_KeyPress);
            this.txtCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtCode_MouseDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIFile,
            this.TSMIOptions,
            this.TSMIHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(778, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TSMIFile
            // 
            this.TSMIFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMINew,
            this.TSMIOpen,
            this.TSMIRecentFiles,
            this.TSMISave,
            this.TSMISaveAs,
            this.TSMIClose});
            this.TSMIFile.Name = "TSMIFile";
            this.TSMIFile.Size = new System.Drawing.Size(60, 20);
            this.TSMIFile.Text = "Archivo";
            // 
            // TSMINew
            // 
            this.TSMINew.Name = "TSMINew";
            this.TSMINew.Size = new System.Drawing.Size(173, 22);
            this.TSMINew.Text = "Nuevo";
            this.TSMINew.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // TSMIOpen
            // 
            this.TSMIOpen.Name = "TSMIOpen";
            this.TSMIOpen.Size = new System.Drawing.Size(173, 22);
            this.TSMIOpen.Text = "Abrir";
            this.TSMIOpen.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // TSMIRecentFiles
            // 
            this.TSMIRecentFiles.Name = "TSMIRecentFiles";
            this.TSMIRecentFiles.Size = new System.Drawing.Size(173, 22);
            this.TSMIRecentFiles.Text = "Archivos Recientes";
            // 
            // TSMISave
            // 
            this.TSMISave.Name = "TSMISave";
            this.TSMISave.Size = new System.Drawing.Size(173, 22);
            this.TSMISave.Text = "Guardar";
            this.TSMISave.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // TSMISaveAs
            // 
            this.TSMISaveAs.Name = "TSMISaveAs";
            this.TSMISaveAs.Size = new System.Drawing.Size(173, 22);
            this.TSMISaveAs.Text = "Guardar Como";
            this.TSMISaveAs.Click += new System.EventHandler(this.guardarComoToolStripMenuItem_Click);
            // 
            // TSMIClose
            // 
            this.TSMIClose.Name = "TSMIClose";
            this.TSMIClose.Size = new System.Drawing.Size(173, 22);
            this.TSMIClose.Text = "Cerrar";
            this.TSMIClose.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // TSMIOptions
            // 
            this.TSMIOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIDebug,
            this.TSMIAutoScan});
            this.TSMIOptions.Name = "TSMIOptions";
            this.TSMIOptions.Size = new System.Drawing.Size(69, 20);
            this.TSMIOptions.Text = "Opciones";
            // 
            // TSMIDebug
            // 
            this.TSMIDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIEnableDebug,
            this.TSMIChooseVariables});
            this.TSMIDebug.Name = "TSMIDebug";
            this.TSMIDebug.Size = new System.Drawing.Size(249, 22);
            this.TSMIDebug.Text = "Depuración";
            // 
            // TSMIEnableDebug
            // 
            this.TSMIEnableDebug.CheckOnClick = true;
            this.TSMIEnableDebug.Name = "TSMIEnableDebug";
            this.TSMIEnableDebug.Size = new System.Drawing.Size(232, 22);
            this.TSMIEnableDebug.Text = "Habilitar Pruebas de Escritorio";
            this.TSMIEnableDebug.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // TSMIChooseVariables
            // 
            this.TSMIChooseVariables.Enabled = false;
            this.TSMIChooseVariables.Name = "TSMIChooseVariables";
            this.TSMIChooseVariables.Size = new System.Drawing.Size(232, 22);
            this.TSMIChooseVariables.Text = "Escoger Variables a Analizar";
            this.TSMIChooseVariables.Click += new System.EventHandler(this.escogerVariablesAAnalizarToolStripMenuItem_Click);
            // 
            // TSMIAutoScan
            // 
            this.TSMIAutoScan.Checked = true;
            this.TSMIAutoScan.CheckOnClick = true;
            this.TSMIAutoScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TSMIAutoScan.Name = "TSMIAutoScan";
            this.TSMIAutoScan.Size = new System.Drawing.Size(249, 22);
            this.TSMIAutoScan.Text = "Análisis de código en tiempo real";
            this.TSMIAutoScan.Click += new System.EventHandler(this.análisisDeCódigoEnTiempoRealToolStripMenuItem_Click);
            // 
            // TSMIHelp
            // 
            this.TSMIHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIManuals,
            this.TSMIExercises,
            this.TSMIExamples,
            this.TSMISurvey,
            this.TSMIAbout});
            this.TSMIHelp.Name = "TSMIHelp";
            this.TSMIHelp.Size = new System.Drawing.Size(53, 20);
            this.TSMIHelp.Text = "Ayuda";
            // 
            // TSMIManuals
            // 
            this.TSMIManuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIManual1,
            this.TSMIManuals2});
            this.TSMIManuals.Name = "TSMIManuals";
            this.TSMIManuals.Size = new System.Drawing.Size(135, 22);
            this.TSMIManuals.Text = "Manuales";
            // 
            // TSMIManual1
            // 
            this.TSMIManual1.Name = "TSMIManual1";
            this.TSMIManual1.Size = new System.Drawing.Size(199, 22);
            this.TSMIManual1.Text = "Conociendo el editor";
            this.TSMIManual1.Click += new System.EventHandler(this.conociendoElEditorToolStripMenuItem_Click);
            // 
            // TSMIManuals2
            // 
            this.TSMIManuals2.Name = "TSMIManuals2";
            this.TSMIManuals2.Size = new System.Drawing.Size(199, 22);
            this.TSMIManuals2.Text = "Conociendo el lenguaje";
            this.TSMIManuals2.Click += new System.EventHandler(this.conociendoElLenguajeToolStripMenuItem_Click);
            // 
            // TSMIExercises
            // 
            this.TSMIExercises.Name = "TSMIExercises";
            this.TSMIExercises.Size = new System.Drawing.Size(135, 22);
            this.TSMIExercises.Text = "Ejercicios";
            this.TSMIExercises.Click += new System.EventHandler(this.ejerciciosToolStripMenuItem_Click);
            // 
            // TSMIExamples
            // 
            this.TSMIExamples.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIExample1,
            this.TSMIExample2,
            this.TSMIExample3,
            this.TSMIExample4,
            this.TSMIExample5,
            this.TSMIExample6,
            this.TSMIExample7,
            this.TSMIExample8,
            this.TSMIExample9,
            this.TSMIExample10});
            this.TSMIExamples.Name = "TSMIExamples";
            this.TSMIExamples.Size = new System.Drawing.Size(135, 22);
            this.TSMIExamples.Text = "Ejemplos";
            // 
            // TSMIExample1
            // 
            this.TSMIExample1.Name = "TSMIExample1";
            this.TSMIExample1.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample1.Text = "Datos de Usuario";
            this.TSMIExample1.Click += new System.EventHandler(this.datosDeUsuarioToolStripMenuItem_Click);
            // 
            // TSMIExample2
            // 
            this.TSMIExample2.Name = "TSMIExample2";
            this.TSMIExample2.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample2.Text = "Promedio de Notas";
            this.TSMIExample2.Click += new System.EventHandler(this.promedioDeNotasToolStripMenuItem_Click);
            // 
            // TSMIExample3
            // 
            this.TSMIExample3.Name = "TSMIExample3";
            this.TSMIExample3.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample3.Text = "Carro en movimiento";
            this.TSMIExample3.Click += new System.EventHandler(this.carroEnMovimientoToolStripMenuItem_Click);
            // 
            // TSMIExample4
            // 
            this.TSMIExample4.Name = "TSMIExample4";
            this.TSMIExample4.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample4.Text = "Sonidos";
            this.TSMIExample4.Click += new System.EventHandler(this.sonidosToolStripMenuItem_Click);
            // 
            // TSMIExample5
            // 
            this.TSMIExample5.Name = "TSMIExample5";
            this.TSMIExample5.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample5.Text = "Orden de Números";
            this.TSMIExample5.Click += new System.EventHandler(this.oToolStripMenuItem_Click);
            // 
            // TSMIExample6
            // 
            this.TSMIExample6.Name = "TSMIExample6";
            this.TSMIExample6.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample6.Text = "Graficación de Funciones Trigonométricas";
            this.TSMIExample6.Click += new System.EventHandler(this.graficaciónDeFuncionesTrigonométricasToolStripMenuItem_Click);
            // 
            // TSMIExample7
            // 
            this.TSMIExample7.Name = "TSMIExample7";
            this.TSMIExample7.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample7.Text = "Generación de Ruido Visual";
            this.TSMIExample7.Click += new System.EventHandler(this.generaciónDeRuidoVisualToolStripMenuItem_Click);
            // 
            // TSMIExample8
            // 
            this.TSMIExample8.Name = "TSMIExample8";
            this.TSMIExample8.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample8.Text = "Serie Fibonacci";
            this.TSMIExample8.Click += new System.EventHandler(this.serieFibonacciToolStripMenuItem_Click);
            // 
            // TSMIExample9
            // 
            this.TSMIExample9.Name = "TSMIExample9";
            this.TSMIExample9.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample9.Text = "Sufragio";
            this.TSMIExample9.Click += new System.EventHandler(this.sufragioToolStripMenuItem_Click);
            // 
            // TSMIExample10
            // 
            this.TSMIExample10.Name = "TSMIExample10";
            this.TSMIExample10.Size = new System.Drawing.Size(295, 22);
            this.TSMIExample10.Text = "Función por Partes";
            this.TSMIExample10.Click += new System.EventHandler(this.funciónPorPartesToolStripMenuItem_Click);
            // 
            // TSMISurvey
            // 
            this.TSMISurvey.Name = "TSMISurvey";
            this.TSMISurvey.Size = new System.Drawing.Size(135, 22);
            this.TSMISurvey.Text = "Encuesta";
            this.TSMISurvey.Click += new System.EventHandler(this.encuestaToolStripMenuItem_Click);
            // 
            // TSMIAbout
            // 
            this.TSMIAbout.Name = "TSMIAbout";
            this.TSMIAbout.Size = new System.Drawing.Size(135, 22);
            this.TSMIAbout.Text = "Acerca de...";
            this.TSMIAbout.Click += new System.EventHandler(this.acercaDeToolStripMenuItem_Click);
            // 
            // tblErrors
            // 
            this.tblErrors.AllowUserToAddRows = false;
            this.tblErrors.AllowUserToDeleteRows = false;
            this.tblErrors.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tblErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.number,
            this.lineNumber,
            this.errorType,
            this.description});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblErrors.DefaultCellStyle = dataGridViewCellStyle1;
            this.tblErrors.Location = new System.Drawing.Point(12, 240);
            this.tblErrors.Name = "tblErrors";
            this.tblErrors.ReadOnly = true;
            this.tblErrors.Size = new System.Drawing.Size(754, 187);
            this.tblErrors.TabIndex = 2;
            // 
            // number
            // 
            this.number.HeaderText = "#";
            this.number.Name = "number";
            this.number.ReadOnly = true;
            this.number.Width = 50;
            // 
            // lineNumber
            // 
            this.lineNumber.HeaderText = "Línea";
            this.lineNumber.Name = "lineNumber";
            this.lineNumber.ReadOnly = true;
            this.lineNumber.Width = 50;
            // 
            // errorType
            // 
            this.errorType.HeaderText = "Tipo Error";
            this.errorType.Name = "errorType";
            this.errorType.ReadOnly = true;
            this.errorType.Width = 125;
            // 
            // description
            // 
            this.description.HeaderText = "Descripción";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 450;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(58, 27);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(40, 40);
            this.btnSave.TabIndex = 34;
            this.toolTip1.SetToolTip(this.btnSave, "Guardar Código");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.BackgroundImage")));
            this.btnOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnOpen.Location = new System.Drawing.Point(12, 27);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(40, 40);
            this.btnOpen.TabIndex = 33;
            this.toolTip1.SetToolTip(this.btnOpen, "Abrir Código");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnScan.BackgroundImage")));
            this.btnScan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.Location = new System.Drawing.Point(104, 27);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(40, 40);
            this.btnScan.TabIndex = 35;
            this.toolTip1.SetToolTip(this.btnScan, "Buscar Errores");
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnRun
            // 
            this.btnRun.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRun.BackgroundImage")));
            this.btnRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(150, 27);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(40, 40);
            this.btnRun.TabIndex = 36;
            this.toolTip1.SetToolTip(this.btnRun, "Compilar y Ejecutar");
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Archivos SMS|*.sms";
            this.openFileDialog1.Title = "Abrir Archivo de Código Fuente";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "sms";
            this.saveFileDialog1.InitialDirectory = "Documents";
            this.saveFileDialog1.Title = "Guardar Archivo de Código Fuente";
            // 
            // tmrScanTime
            // 
            this.tmrScanTime.Interval = 3000;
            this.tmrScanTime.Tick += new System.EventHandler(this.tmrScanTime_Tick);
            // 
            // tmrReEnableCompile
            // 
            this.tmrReEnableCompile.Interval = 5000;
            this.tmrReEnableCompile.Tick += new System.EventHandler(this.tmrReEnableCompile_Tick);
            // 
            // frmCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 439);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.tblErrors);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmCompiler";
            this.Text = "Compilador";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScintillaNET.Scintilla txtCode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSMIFile;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpen;
        private System.Windows.Forms.ToolStripMenuItem TSMIRecentFiles;
        private System.Windows.Forms.ToolStripMenuItem TSMISave;
        private System.Windows.Forms.ToolStripMenuItem TSMIClose;
        private System.Windows.Forms.ToolStripMenuItem TSMIHelp;
        private System.Windows.Forms.DataGridView tblErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorType;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.ToolStripMenuItem TSMIOptions;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem TSMISaveAs;
        private System.Windows.Forms.ToolStripMenuItem TSMIDebug;
        private System.Windows.Forms.Timer tmrScanTime;
        private System.Windows.Forms.ToolStripMenuItem TSMINew;
        private System.Windows.Forms.ToolStripMenuItem TSMIAutoScan;
        private System.Windows.Forms.ToolStripMenuItem TSMIManuals;
        private System.Windows.Forms.ToolStripMenuItem TSMIExercises;
        private System.Windows.Forms.ToolStripMenuItem TSMIAbout;
        private System.Windows.Forms.ToolStripMenuItem TSMIManual1;
        private System.Windows.Forms.ToolStripMenuItem TSMIManuals2;
        private System.Windows.Forms.ToolStripMenuItem TSMIExamples;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample1;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample2;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample3;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample4;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample5;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample6;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample7;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample8;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample9;
        private System.Windows.Forms.ToolStripMenuItem TSMIExample10;
        private System.Windows.Forms.Timer tmrReEnableCompile;
        private System.Windows.Forms.ToolStripMenuItem TSMISurvey;
        private System.Windows.Forms.ToolStripMenuItem TSMIEnableDebug;
        private System.Windows.Forms.ToolStripMenuItem TSMIChooseVariables;
    }
}

