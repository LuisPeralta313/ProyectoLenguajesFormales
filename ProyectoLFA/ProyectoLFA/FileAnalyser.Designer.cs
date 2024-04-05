
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
            this.SuspendLayout();
            // 
            // BTNUpload
            // 
            this.BTNUpload.BackColor = System.Drawing.SystemColors.ControlText;
            this.BTNUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTNUpload.ForeColor = System.Drawing.SystemColors.Control;
            this.BTNUpload.Location = new System.Drawing.Point(10, 22);
            this.BTNUpload.Name = "BTNUpload";
            this.BTNUpload.Size = new System.Drawing.Size(118, 23);
            this.BTNUpload.TabIndex = 0;
            this.BTNUpload.Text = "Cargar Gramática";
            this.BTNUpload.UseVisualStyleBackColor = false;
            this.BTNUpload.Click += new System.EventHandler(this.BTNUpload_Click);
            // 
            // RTBGrammar
            // 
            this.RTBGrammar.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.RTBGrammar.Location = new System.Drawing.Point(12, 84);
            this.RTBGrammar.Name = "RTBGrammar";
            this.RTBGrammar.Size = new System.Drawing.Size(370, 241);
            this.RTBGrammar.TabIndex = 1;
            this.RTBGrammar.Text = "";
            this.RTBGrammar.TextChanged += new System.EventHandler(this.RTBGrammar_TextChanged);
            // 
            // TResult
            // 
            this.TResult.Location = new System.Drawing.Point(136, 58);
            this.TResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TResult.Name = "TResult";
            this.TResult.Size = new System.Drawing.Size(246, 20);
            this.TResult.TabIndex = 2;
            this.TResult.TextChanged += new System.EventHandler(this.TResult_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Resultado:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TXTPath
            // 
            this.TXTPath.Location = new System.Drawing.Point(136, 27);
            this.TXTPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TXTPath.Name = "TXTPath";
            this.TXTPath.Size = new System.Drawing.Size(246, 20);
            this.TXTPath.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Desktop;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(260, 336);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Regresar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Desktop;
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(337, 336);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 27);
            this.button2.TabIndex = 6;
            this.button2.Text = "Salir";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TransitionBTN
            // 
            this.TransitionBTN.BackColor = System.Drawing.SystemColors.Desktop;
            this.TransitionBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TransitionBTN.Location = new System.Drawing.Point(183, 330);
            this.TransitionBTN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TransitionBTN.Name = "TransitionBTN";
            this.TransitionBTN.Size = new System.Drawing.Size(73, 38);
            this.TransitionBTN.TabIndex = 7;
            this.TransitionBTN.Text = "Obtener transiciones\r\n";
            this.TransitionBTN.UseVisualStyleBackColor = false;
            this.TransitionBTN.Click += new System.EventHandler(this.button3_Click);
            // 
            // FileAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(412, 372);
            this.Controls.Add(this.TransitionBTN);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TXTPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TResult);
            this.Controls.Add(this.RTBGrammar);
            this.Controls.Add(this.BTNUpload);
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
    }
}

