namespace Client
{
    partial class ClientWindow
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
            this.failButton = new System.Windows.Forms.Button();
            this.getStatusButton = new System.Windows.Forms.Button();
            this.startConButton = new System.Windows.Forms.Button();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.freeze = new System.Windows.Forms.Button();
            this.recover = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // failButton
            // 
            this.failButton.Location = new System.Drawing.Point(13, 39);
            this.failButton.Name = "failButton";
            this.failButton.Size = new System.Drawing.Size(75, 23);
            this.failButton.TabIndex = 0;
            this.failButton.Text = "Fail";
            this.failButton.UseVisualStyleBackColor = true;
            this.failButton.Click += new System.EventHandler(this.failButton_Click);
            // 
            // getStatusButton
            // 
            this.getStatusButton.Location = new System.Drawing.Point(12, 68);
            this.getStatusButton.Name = "getStatusButton";
            this.getStatusButton.Size = new System.Drawing.Size(75, 23);
            this.getStatusButton.TabIndex = 3;
            this.getStatusButton.Text = "getStatus";
            this.getStatusButton.UseVisualStyleBackColor = true;
            this.getStatusButton.Click += new System.EventHandler(this.getStatusButton_Click);
            // 
            // startConButton
            // 
            this.startConButton.Location = new System.Drawing.Point(177, 227);
            this.startConButton.Name = "startConButton";
            this.startConButton.Size = new System.Drawing.Size(75, 23);
            this.startConButton.TabIndex = 4;
            this.startConButton.Text = "Start";
            this.startConButton.UseVisualStyleBackColor = true;
            this.startConButton.Click += new System.EventHandler(this.startConButton_Click);
            // 
            // serverPort
            // 
            this.serverPort.Location = new System.Drawing.Point(80, 12);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(100, 20);
            this.serverPort.TabIndex = 5;
            this.serverPort.TextChanged += new System.EventHandler(this.serverPort_TextChanged);
            // 
            // freeze
            // 
            this.freeze.Location = new System.Drawing.Point(95, 39);
            this.freeze.Name = "freeze";
            this.freeze.Size = new System.Drawing.Size(75, 23);
            this.freeze.TabIndex = 6;
            this.freeze.Text = "Freeze";
            this.freeze.UseVisualStyleBackColor = true;
            this.freeze.Click += new System.EventHandler(this.freeze_Click);
            // 
            // recover
            // 
            this.recover.Location = new System.Drawing.Point(177, 39);
            this.recover.Name = "recover";
            this.recover.Size = new System.Drawing.Size(75, 23);
            this.recover.TabIndex = 7;
            this.recover.Text = "Recover";
            this.recover.UseVisualStyleBackColor = true;
            this.recover.Click += new System.EventHandler(this.recover_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server Port";
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recover);
            this.Controls.Add(this.freeze);
            this.Controls.Add(this.serverPort);
            this.Controls.Add(this.startConButton);
            this.Controls.Add(this.getStatusButton);
            this.Controls.Add(this.failButton);
            this.Name = "ClientWindow";
            this.Text = "ClientWindow";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button failButton;
        private System.Windows.Forms.Button getStatusButton;
        private System.Windows.Forms.Button startConButton;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.Button freeze;
        private System.Windows.Forms.Button recover;
        private System.Windows.Forms.Label label1;
    }
}

