namespace Proyecto_Fase_Uno
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.BCargar = new System.Windows.Forms.Button();
            this.RTBMostrarGramatica = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TBMostrarResultado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BCargar
            // 
            this.BCargar.Location = new System.Drawing.Point(115, 103);
            this.BCargar.Margin = new System.Windows.Forms.Padding(2);
            this.BCargar.Name = "BCargar";
            this.BCargar.Size = new System.Drawing.Size(81, 27);
            this.BCargar.TabIndex = 1;
            this.BCargar.Text = "Cargar archivo";
            this.BCargar.UseVisualStyleBackColor = true;
            this.BCargar.Click += new System.EventHandler(this.BCargar_Click);
            // 
            // RTBMostrarGramatica
            // 
            this.RTBMostrarGramatica.Location = new System.Drawing.Point(361, 52);
            this.RTBMostrarGramatica.Margin = new System.Windows.Forms.Padding(2);
            this.RTBMostrarGramatica.Name = "RTBMostrarGramatica";
            this.RTBMostrarGramatica.Size = new System.Drawing.Size(206, 190);
            this.RTBMostrarGramatica.TabIndex = 4;
            this.RTBMostrarGramatica.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 178);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Resultado de la gramática";
            // 
            // TBMostrarResultado
            // 
            this.TBMostrarResultado.Location = new System.Drawing.Point(97, 202);
            this.TBMostrarResultado.Margin = new System.Windows.Forms.Padding(2);
            this.TBMostrarResultado.Name = "TBMostrarResultado";
            this.TBMostrarResultado.Size = new System.Drawing.Size(122, 20);
            this.TBMostrarResultado.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 310);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBMostrarResultado);
            this.Controls.Add(this.RTBMostrarGramatica);
            this.Controls.Add(this.BCargar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BCargar;
        private System.Windows.Forms.RichTextBox RTBMostrarGramatica;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBMostrarResultado;
    }
}

