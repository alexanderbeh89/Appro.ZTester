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
            this.ResetButton = new System.Windows.Forms.Button();
            this.TestOperationTextBox = new System.Windows.Forms.TextBox();
            this.labelTestStatus = new System.Windows.Forms.Label();
            this.labelTestLogsView = new System.Windows.Forms.Label();
            this.labelInterfaceBoardStateView = new System.Windows.Forms.Label();
            this.labelInterfaceBoardState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(645, 312);
            this.StartButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(130, 35);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start Test";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(645, 353);
            this.StopButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(130, 35);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Abort Test";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(645, 394);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(130, 35);
            this.ResetButton.TabIndex = 3;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // TestOperationTextBox
            // 
            this.TestOperationTextBox.Location = new System.Drawing.Point(29, 312);
            this.TestOperationTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TestOperationTextBox.Multiline = true;
            this.TestOperationTextBox.Name = "TestOperationTextBox";
            this.TestOperationTextBox.ReadOnly = true;
            this.TestOperationTextBox.Size = new System.Drawing.Size(499, 116);
            this.TestOperationTextBox.TabIndex = 7;
            // 
            // labelTestStatus
            // 
            this.labelTestStatus.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelTestStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTestStatus.ForeColor = System.Drawing.Color.Black;
            this.labelTestStatus.Location = new System.Drawing.Point(27, 21);
            this.labelTestStatus.Name = "labelTestStatus";
            this.labelTestStatus.Size = new System.Drawing.Size(748, 170);
            this.labelTestStatus.TabIndex = 8;
            this.labelTestStatus.Text = "IDLE";
            this.labelTestStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelTestLogsView
            // 
            this.labelTestLogsView.AutoSize = true;
            this.labelTestLogsView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTestLogsView.Location = new System.Drawing.Point(25, 279);
            this.labelTestLogsView.Name = "labelTestLogsView";
            this.labelTestLogsView.Size = new System.Drawing.Size(79, 20);
            this.labelTestLogsView.TabIndex = 9;
            this.labelTestLogsView.Text = "Test Logs";
            // 
            // labelInterfaceBoardStateView
            // 
            this.labelInterfaceBoardStateView.AutoSize = true;
            this.labelInterfaceBoardStateView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInterfaceBoardStateView.Location = new System.Drawing.Point(591, 210);
            this.labelInterfaceBoardStateView.Name = "labelInterfaceBoardStateView";
            this.labelInterfaceBoardStateView.Size = new System.Drawing.Size(163, 20);
            this.labelInterfaceBoardStateView.TabIndex = 10;
            this.labelInterfaceBoardStateView.Text = "Interface Board State";
            // 
            // labelInterfaceBoardState
            // 
            this.labelInterfaceBoardState.BackColor = System.Drawing.SystemColors.Window;
            this.labelInterfaceBoardState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInterfaceBoardState.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelInterfaceBoardState.Location = new System.Drawing.Point(645, 236);
            this.labelInterfaceBoardState.Name = "labelInterfaceBoardState";
            this.labelInterfaceBoardState.Size = new System.Drawing.Size(130, 33);
            this.labelInterfaceBoardState.TabIndex = 11;
            this.labelInterfaceBoardState.Text = "OK";
            this.labelInterfaceBoardState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // QDOS14514ButtonMicFunctionalTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelInterfaceBoardState);
            this.Controls.Add(this.labelInterfaceBoardStateView);
            this.Controls.Add(this.labelTestLogsView);
            this.Controls.Add(this.labelTestStatus);
            this.Controls.Add(this.TestOperationTextBox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QDOS14514ButtonMicFunctionalTesterForm";
            this.Text = "QDOS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QDOS14514ButtonMicFunctionalTesterForm_FormClosing);
            this.Load += new System.EventHandler(this.QDOS14514ButtonMicFunctionalTesterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.TextBox TestOperationTextBox;
        private System.Windows.Forms.Label labelTestStatus;
        private System.Windows.Forms.Label labelTestLogsView;
        private System.Windows.Forms.Label labelInterfaceBoardStateView;
        private System.Windows.Forms.Label labelInterfaceBoardState;
    }
}

