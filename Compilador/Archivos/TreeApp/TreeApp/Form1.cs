using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TreeApp
{
    public partial class Form1 : Form
    {

	[DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();

Console.WriteLine("Ingrese su nombre por favor:");
string nombre=Console.ReadLine();
Console.WriteLine("Hola "+nombre+"!");
Console.WriteLine("Ingrese su edad:");
int edad=Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Ingrese su peso en kilogramos:");
double peso=Convert.ToDouble(Console.ReadLine());
Console.WriteLine("Ingrese su estatura en centimetros:");
double estatura=Convert.ToDouble(Console.ReadLine());
char genero;
bool _found0 = false;
for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){if (treeView1.Nodes[_i1].Text == "genero") _found0 = true; }if (!_found0) treeView1.Nodes.Add("genero");
do
{
Console.WriteLine("Ingrese M si es mujer, o H si es hombre:");
genero=Convert.ToChar(Console.ReadLine());
treeView1.Nodes[4].Nodes.Add((genero).ToString());
bool esMujer;
bool _found1 = false;
for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){if (treeView1.Nodes[_i1].Text == "esMujer") _found1 = true; }if (!_found1) treeView1.Nodes.Add("esMujer");
if(genero=='M'||genero=='m')
{
esMujer=true ;
treeView1.Nodes[5].Nodes.Add((esMujer).ToString());
}
else if(genero=='H'||genero=='h')
{
esMujer=false ;
treeView1.Nodes[5].Nodes.Add((esMujer).ToString());
}
else
{
Console.WriteLine("Opcion no reconocida");
}

}
 while(genero!='M'&&genero!='m'&&genero!='H'&&genero!='h');
if(edad<16)
{
Console.WriteLine("No tiene edad suficiente para sufragar");
}
else if(edad<18)
{
Console.WriteLine("En su caso, el voto es opcional");
}
else
{
Console.WriteLine("El sufragio es obligatorio en su caso");
}

Console.WriteLine("Su indice de masa corporal es:");
try
{
Console.WriteLine(peso/(Math.Pow((estatura/100),2)));
}
catch
{
Console.WriteLine("División para cero.","502");
Console.WriteLine("Console.WriteLine(peso/(Math.Pow((estatura/100),2)));");
}

for(int i=nombre.Length-1;i>=0;i--)
{
try
{
Console.WriteLine(nombre[i]);
}
catch
{
Console.WriteLine("Acceso a índice inexistente del arreglo.","503");
Console.WriteLine("Console.WriteLine(nombre[i]);");
}

}

Console.ReadLine();


        }
    }
}
