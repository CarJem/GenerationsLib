namespace GenerationsLib.UpdateAssistant
{
    partial class ConfigurationManager
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.configButton3 = new System.Windows.Forms.Button();
            this.configButton1 = new System.Windows.Forms.Button();
            this.configButton2 = new System.Windows.Forms.Button();
            this.configButton4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(53, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(264, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // configButton3
            // 
            this.configButton3.Location = new System.Drawing.Point(12, 96);
            this.configButton3.Name = "configButton3";
            this.configButton3.Size = new System.Drawing.Size(305, 23);
            this.configButton3.TabIndex = 7;
            this.configButton3.Text = "Version Metadata...";
            this.configButton3.UseVisualStyleBackColor = true;
            this.configButton3.Click += new System.EventHandler(this.configButton3_Click);
            // 
            // configButton1
            // 
            this.configButton1.Location = new System.Drawing.Point(12, 38);
            this.configButton1.Name = "configButton1";
            this.configButton1.Size = new System.Drawing.Size(305, 23);
            this.configButton1.TabIndex = 8;
            this.configButton1.Text = "Sites to Publish To...";
            this.configButton1.UseVisualStyleBackColor = true;
            this.configButton1.Click += new System.EventHandler(this.configButton1_Click);
            // 
            // configButton2
            // 
            this.configButton2.Location = new System.Drawing.Point(12, 67);
            this.configButton2.Name = "configButton2";
            this.configButton2.Size = new System.Drawing.Size(305, 23);
            this.configButton2.TabIndex = 9;
            this.configButton2.Text = "Download Hosts...";
            this.configButton2.UseVisualStyleBackColor = true;
            this.configButton2.Click += new System.EventHandler(this.configButton2_Click);
            // 
            // configButton4
            // 
            this.configButton4.Location = new System.Drawing.Point(12, 125);
            this.configButton4.Name = "configButton4";
            this.configButton4.Size = new System.Drawing.Size(305, 23);
            this.configButton4.TabIndex = 10;
            this.configButton4.Text = "Sites to Post to...";
            this.configButton4.UseVisualStyleBackColor = true;
            this.configButton4.Click += new System.EventHandler(this.configButton4_Click);
            // 
            // ConfigurationManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 155);
            this.Controls.Add(this.configButton4);
            this.Controls.Add(this.configButton2);
            this.Controls.Add(this.configButton1);
            this.Controls.Add(this.configButton3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "ConfigurationManager";
            this.Text = "Edit Configuration";
            this.VisibleChanged += new System.EventHandler(this.ConfigurationManager_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button configButton3;
        private System.Windows.Forms.Button configButton1;
        private System.Windows.Forms.Button configButton2;
        private System.Windows.Forms.Button configButton4;
    }
}