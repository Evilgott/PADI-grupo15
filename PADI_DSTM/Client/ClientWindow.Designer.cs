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
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.getStatusButton = new System.Windows.Forms.Button();
            this.startConButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // failButton
            // 
            this.failButton.Location = new System.Drawing.Point(13, 227);
            this.failButton.Name = "failButton";
            this.failButton.Size = new System.Drawing.Size(75, 23);
            this.failButton.TabIndex = 0;
            this.failButton.Text = "Fail";
            this.failButton.UseVisualStyleBackColor = true;
            this.failButton.Click += new System.EventHandler(this.failButton_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(13, 13);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(100, 20);
            this.statusTextBox.TabIndex = 2;
            // 
            // getStatusButton
            // 
            this.getStatusButton.Location = new System.Drawing.Point(95, 227);
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
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.startConButton);
            this.Controls.Add(this.getStatusButton);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.failButton);
            this.Name = "ClientWindow";
            this.Text = "ClientWindow";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button failButton;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Button getStatusButton;
        private System.Windows.Forms.Button startConButton;
    }
}

