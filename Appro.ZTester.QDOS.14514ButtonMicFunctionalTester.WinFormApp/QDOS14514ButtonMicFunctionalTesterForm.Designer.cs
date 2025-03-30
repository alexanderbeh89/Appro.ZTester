namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.WinFormApp
{
    partial class QDOS14514ButtonMicFunctionalTesterForm
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
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.MsgTextBox = new System.Windows.Forms.TextBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.PTTButton = new System.Windows.Forms.Button();
            this.VolumeUpButton = new System.Windows.Forms.Button();
            this.VolumeDownButton = new System.Windows.Forms.Button();
            this.TestOperationTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(645, 299);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(131, 35);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start Test";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(645, 340);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(131, 35);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop Test";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // MsgTextBox
            // 
            this.MsgTextBox.Location = new System.Drawing.Point(29, 34);
            this.MsgTextBox.Multiline = true;
            this.MsgTextBox.Name = "MsgTextBox";
            this.MsgTextBox.ReadOnly = true;
            this.MsgTextBox.Size = new System.Drawing.Size(499, 117);
            this.MsgTextBox.TabIndex = 2;
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(645, 381);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(131, 35);
            this.ResetButton.TabIndex = 3;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // PTTButton
            // 
            this.PTTButton.Location = new System.Drawing.Point(645, 34);
            this.PTTButton.Name = "PTTButton";
            this.PTTButton.Size = new System.Drawing.Size(131, 35);
            this.PTTButton.TabIndex = 4;
            this.PTTButton.Text = "PTT";
            this.PTTButton.UseVisualStyleBackColor = true;
            this.PTTButton.Click += new System.EventHandler(this.PTTButton_Click);
            // 
            // VolumeUpButton
            // 
            this.VolumeUpButton.Location = new System.Drawing.Point(645, 75);
            this.VolumeUpButton.Name = "VolumeUpButton";
            this.VolumeUpButton.Size = new System.Drawing.Size(131, 35);
            this.VolumeUpButton.TabIndex = 5;
            this.VolumeUpButton.Text = "Volume Up";
            this.VolumeUpButton.UseVisualStyleBackColor = true;
            this.VolumeUpButton.Click += new System.EventHandler(this.VolumeUpButton_Click);
            // 
            // VolumeDownButton
            // 
            this.VolumeDownButton.Location = new System.Drawing.Point(645, 116);
            this.VolumeDownButton.Name = "VolumeDownButton";
            this.VolumeDownButton.Size = new System.Drawing.Size(131, 35);
            this.VolumeDownButton.TabIndex = 6;
            this.VolumeDownButton.Text = "Volume Down";
            this.VolumeDownButton.UseVisualStyleBackColor = true;
            // 
            // TestOperationTextBox
            // 
            this.TestOperationTextBox.Location = new System.Drawing.Point(29, 299);
            this.TestOperationTextBox.Multiline = true;
            this.TestOperationTextBox.Name = "TestOperationTextBox";
            this.TestOperationTextBox.ReadOnly = true;
            this.TestOperationTextBox.Size = new System.Drawing.Size(499, 117);
            this.TestOperationTextBox.TabIndex = 7;
            // 
            // QDOS14514ButtonMicFunctionalTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TestOperationTextBox);
            this.Controls.Add(this.VolumeDownButton);
            this.Controls.Add(this.VolumeUpButton);
            this.Controls.Add(this.PTTButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.MsgTextBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Name = "QDOS14514ButtonMicFunctionalTesterForm";
            this.Text = "QDOS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QDOS14514ButtonMicFunctionalTesterForm_FormClosing_1);
            this.Load += new System.EventHandler(this.QDOS14514ButtonMicFunctionalTesterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.TextBox MsgTextBox;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button PTTButton;
        private System.Windows.Forms.Button VolumeUpButton;
        private System.Windows.Forms.Button VolumeDownButton;
        private System.Windows.Forms.TextBox TestOperationTextBox;
    }
}

