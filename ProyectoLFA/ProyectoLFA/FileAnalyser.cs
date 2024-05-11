using Microsoft.CSharp;
using ProyectoLFA.Classes;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.WinForms;

namespace ProyectoLFA
{
    public partial class FileAnalyser : Form
    {
        ET ExpressionTree = new ET();
        public FileAnalyser()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BTNUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                AnalizarArchivo(openFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show(@"Error al leer el archivo.");
            }
        }
        private void AnalizarArchivo(string file)
        {
            TransitionBTN.Visible = false;
            TXTPath.Text = file;
            RTBGrammar.Select(0, RTBGrammar.Lines.Length);
            RTBGrammar.SelectionColor = Color.Black;

            try
            {
                int line1 = 0;
                string text = File.ReadAllText(file);
                //Send line
                TResult.Text = Classes.GrammarFormat.AnalyseFile(text, ref line1);
                RTBGrammar.Text = text;

                if (TResult.Text.Contains("Correcto"))
                {
                    TResult.BackColor = Color.LightGray;
                    TResult.ForeColor = Color.Cyan;
                    TransitionBTN.Visible = true;

                    ExpressionTree = Classes.GrammarFormat.GetExpressionTree(RTBGrammar.Text);
                }
                else
                {
                    TResult.BackColor = Color.LightGray;
                    TResult.ForeColor = Color.Crimson;

                    //Ubicacion del error
                    int lineCounter = 0;

                    foreach (string line in RTBGrammar.Lines)
                    {
                        if (line1 - 1 == lineCounter)
                        {
                            RTBGrammar.Select(RTBGrammar.GetFirstCharIndexFromLine(lineCounter), line.Length);
                            RTBGrammar.SelectionColor = Color.Red;
                        }
                        lineCounter++;
                    }
                }

            }
            catch (Exception ex)
            {

                TResult.BackColor = Color.LightGray;
                TResult.ForeColor = Color.Crimson;
                TResult.Text = @"Error en TOKENS";
                TransitionBTN.Visible = false;
                MessageBox.Show(ex.Message);

                //Show in red all lines in tokens
                int lineCounter = 0;

                foreach (string line in RTBGrammar.Lines)
                {
                    if (line.Contains("TOKEN"))
                    {
                        RTBGrammar.Select(RTBGrammar.GetFirstCharIndexFromLine(lineCounter), line.Length);
                        RTBGrammar.SelectionColor = Color.Red;
                    }
                    lineCounter++;
                }
            }
        }

        private void TResult_TextChanged(object sender, EventArgs e)
        {

        }

        private void RTBGrammar_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 frm2 = new Form2();

            frm2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FollowTable follows = new FollowTable(ExpressionTree);
                TransitionT transitions = new TransitionT(follows);
                TransitionsView tables = new TransitionsView(ExpressionTree, follows, transitions);
                tables.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string[] sourceCode = ScannerGenerator.GetSourceCode(ExpressionTree);

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.Desktop,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string Output = "Scanner.exe";
                string path = Path.Combine(dialog.SelectedPath, Output);

                //AQUÍ GUARDAMOS EL ARCHIVO DE JAVA

                File.WriteAllText(Path.Combine(dialog.SelectedPath, "Scanner.java"), sourceCode[1]);

                //Compilamos nuestro código

                CSharpCodeProvider codeProvider = new CSharpCodeProvider();


                CompilerParameters parameters = new CompilerParameters
                { GenerateExecutable = true, OutputAssembly = path };

                parameters.ReferencedAssemblies.AddRange(
                    Assembly.GetExecutingAssembly().GetReferencedAssemblies().
                        Select(a => a.Name + ".dll").ToArray());


                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode[0]);

                if (results.Errors.Count > 0)
                {
                    TResult.ForeColor = Color.Red;
                    TResult.Text = "";
                    foreach (CompilerError CompErr in results.Errors)
                    {
                        TResult.Text = TResult.Text +
                                             @"Line number " + CompErr.Line +
                                             @", Error Number: " + CompErr.ErrorNumber +
                                             ", '" + CompErr.ErrorText + ";" +
                                             Environment.NewLine + Environment.NewLine;
                    }
                }
                else
                {
                    //Compilamos y ejecutamos el código.
                    TResult.ForeColor = Color.BlueViolet;
                    TResult.Text = @"Tu scanner está listo.";
                    Process.Start(path);
                }
            }

        }
    }
}


