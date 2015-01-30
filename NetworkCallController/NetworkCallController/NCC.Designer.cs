namespace NetworkCallController
{
    partial class NCC
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
            this.labIP = new System.Windows.Forms.Label();
            this.labPort = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.butConn = new System.Windows.Forms.Button();
            this.butClear = new System.Windows.Forms.Button();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labIP
            // 
            this.labIP.AutoSize = true;
            this.labIP.Location = new System.Drawing.Point(29, 21);
            this.labIP.Name = "labIP";
            this.labIP.Size = new System.Drawing.Size(57, 13);
            this.labIP.TabIndex = 0;
            this.labIP.Text = "Ip Address";
            // 
            // labPort
            // 
            this.labPort.AutoSize = true;
            this.labPort.Location = new System.Drawing.Point(29, 72);
            this.labPort.Name = "labPort";
            this.labPort.Size = new System.Drawing.Size(26, 13);
            this.labPort.TabIndex = 1;
            this.labPort.Text = "Port";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(32, 37);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxIP.TabIndex = 2;
            this.textBoxIP.Text = "127.0.0.1";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(32, 98);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(100, 20);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "14000";
            // 
            // butConn
            // 
            this.butConn.Location = new System.Drawing.Point(32, 163);
            this.butConn.Name = "butConn";
            this.butConn.Size = new System.Drawing.Size(75, 23);
            this.butConn.TabIndex = 4;
            this.butConn.Text = "Connect";
            this.butConn.UseVisualStyleBackColor = true;
            // 
            // butClear
            // 
            this.butClear.Location = new System.Drawing.Point(259, 181);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(75, 23);
            this.butClear.TabIndex = 5;
            this.butClear.Text = "Clear Log";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Location = new System.Drawing.Point(32, 219);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.Size = new System.Drawing.Size(302, 163);
            this.richTextBox_Log.TabIndex = 6;
            this.richTextBox_Log.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(145, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Odswiez obdior";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 433);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox_Log);
            this.Controls.Add(this.butClear);
            this.Controls.Add(this.butConn);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.labPort);
            this.Controls.Add(this.labIP);
            this.Name = "NCC";
            this.Text = "NCC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labIP;
        private System.Windows.Forms.Label labPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button butConn;
        private System.Windows.Forms.Button butClear;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Button button1;
    }
}

