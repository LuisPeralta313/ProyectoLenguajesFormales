
namespace ProyectoLFA
{
    partial class FileAnalyser
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
            this.BTNUpload = new System.Windows.Forms.Button();
            this.RTBGrammar = new System.Windows.Forms.RichTextBox();
            this.TResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TXTPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TransitionBTN = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTNUpload
            // 
            this.BTNUpload.BackColor = System.Drawing.SystemColors.ControlText;
            this.BTNUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTNUpload.ForeColor = System.Drawing.SystemColors.Control;
            this.BTNUpload.Location = new System.Drawing.Point(13, 27);
            this.BTNUpload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BTNUpload.Name = "BTNUpload";
            this.BTNUpload.Size = new System.Drawing.Size(157, 28);
            this.BTNUpload.TabIndex = 0;
            this.BTNUpload.Text = "Cargar Gramática";
            this.BTNUpload.UseVisualStyleBackColor = false;
            this.BTNUpload.Click += new System.EventHandler(this.BTNUpload_Click);
            // 
            // RTBGrammar
            // 
            this.RTBGrammar.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.RTBGrammar.Location = new System.Drawing.Point(16, 103);
            this.RTBGrammar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RTBGrammar.Name = "RTBGrammar";
            this.RTBGrammar.Size = new System.Drawing.Size(492, 296);
            this.RTBGrammar.TabIndex = 1;
            this.RTBGrammar.Text = "";
            this.RTBGrammar.TextChanged += new System.EventHandler(this.RTBGrammar_TextChanged);
            // 
            // TResult
            // 
            this.TResult.Location = new System.Drawing.Point(181, 71);
            this.TResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TResult.Name = "TResult";
            this.TResult.Size = new System.Drawing.Size(327, 22);
            this.TResult.TabIndex = 2;
            this.TResult.TextChanged += new System.EventHandler(this.TResult_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Resultado:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TXTPath
            // 
            this.TXTPath.Location = new System.Drawing.Point(181, 33);
            this.TXTPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TXTPath.Name = "TXTPath";
            this.TXTPath.Size = new System.Drawing.Size(327, 22);
            this.TXTPath.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Desktop;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(347, 414);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 33);
            this.button1.TabIndex = 5;
            this.button1.Text = "Regresar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Desktop;
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(449, 414);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 33);
            this.button2.TabIndex = 6;
            this.button2.Text = "Salir";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TransitionBTN
            // 
            this.TransitionBTN.BackColor = System.Drawing.SystemColors.Desktop;
            this.TransitionBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TransitionBTN.Location = new System.Drawing.Point(160, 415);
            this.TransitionBTN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TransitionBTN.Name = "TransitionBTN";
            this.TransitionBTN.Size = new System.Drawing.Size(181, 32);
            this.TransitionBTN.TabIndex = 7;
            this.TransitionBTN.Text = "Obtener transiciones\r\n";
            this.TransitionBTN.UseVisualStyleBackColor = false;
            this.TransitionBTN.Click += new System.EventHandler(this.button3_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Desktop;
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button3.Location = new System.Drawing.Point(12, 415);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 33);
            this.button3.TabIndex = 8;
            this.button3.Text = "Generador Scanner";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // FileAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(549, 458);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.TransitionBTN);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TXTPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TResult);
            this.Controls.Add(this.RTBGrammar);
            this.Controls.Add(this.BTNUpload);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FileAnalyser";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTNUpload;
        private System.Windows.Forms.RichTextBox RTBGrammar;
        private System.Windows.Forms.TextBox TResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TXTPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button TransitionBTN;
        private System.Windows.Forms.Button button3;
    }
}

