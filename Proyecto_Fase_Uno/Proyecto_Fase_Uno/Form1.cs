using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Proyecto_Fase_Uno
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog AbrirArchivo = new OpenFileDialog();
            DialogResult Resultado = AbrirArchivo.ShowDialog();
            if (Resultado == DialogResult.OK)
            {
                VerificarGramatica(AbrirArchivo.FileName);

            }
            else
            {
                MessageBox.Show("Archivo no correcto");
            }
        }
        private void VerificarGramatica(string Archivo)
        {
            RTBMostrarGramatica.Select(0, RTBMostrarGramatica.Lines.Length);

            try
            {
                int linea = 0;
                string texto = File.ReadAllText(Archivo);
                ReglasExpresionRegular VerificarReglas = new ReglasExpresionRegular();
                TBMostrarResultado.Text = VerificarReglas.Archivo(texto, ref linea);
                RTBMostrarGramatica.Text = texto;
                if (TBMostrarResultado.Text.Contains("Correcto"))
                {
                    TBMostrarResultado.ForeColor = Color.Green;
                }
                else
                {
                    TBMostrarResultado.ForeColor = Color.Red;

                    int ContadorLinea = 0;
                    foreach (string item in RTBMostrarGramatica.Lines)
                    {
                        if (linea - 1 == ContadorLinea)
                        {
                            RTBMostrarGramatica.Select(RTBMostrarGramatica.GetFirstCharIndexFromLine(ContadorLinea), item.Length);
                        }
                    }
                    ContadorLinea++;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
