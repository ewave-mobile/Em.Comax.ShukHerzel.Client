namespace Em.Comax.ShukHerzel.Client
{
    partial class GenerateUserPasswordForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnEncrypt = new Button();
            lblEncryptedResult = new Label();
            txtEncryptedResult = new TextBox();
            btnCopyPassword = new Button();
            label1 = new Label();
            button1 = new Button();
            toolTip = new ToolTip(components);
            button2 = new Button();
            label2 = new Label();
            label3 = new Label();
            txtEncryptedPassword = new TextBox();
            label4 = new Label();
            label5 = new Label();
            btnDecryptedPassword = new Button();
            txtDecryptedResult = new TextBox();
            SuspendLayout();
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(20, 43);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(45, 15);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "סיסמה:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(20, 64);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(270, 23);
            txtPassword.TabIndex = 1;
            // 
            // btnEncrypt
            // 
            btnEncrypt.Location = new Point(20, 98);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(270, 28);
            btnEncrypt.TabIndex = 2;
            btnEncrypt.Text = "צור סיסמה מוצפנת";
            btnEncrypt.UseVisualStyleBackColor = true;
            btnEncrypt.Click += btnEncrypt_Click;
            // 
            // lblEncryptedResult
            // 
            lblEncryptedResult.AutoSize = true;
            lblEncryptedResult.Location = new Point(20, 145);
            lblEncryptedResult.Name = "lblEncryptedResult";
            lblEncryptedResult.Size = new Size(88, 15);
            lblEncryptedResult.TabIndex = 3;
            lblEncryptedResult.Text = "סיסמה מוצפנת:";
            // 
            // txtEncryptedResult
            // 
            txtEncryptedResult.Location = new Point(20, 163);
            txtEncryptedResult.Multiline = true;
            txtEncryptedResult.Name = "txtEncryptedResult";
            txtEncryptedResult.ReadOnly = true;
            txtEncryptedResult.ScrollBars = ScrollBars.Vertical;
            txtEncryptedResult.Size = new Size(270, 80);
            txtEncryptedResult.TabIndex = 4;
            // 
            // btnCopyPassword
            // 
            btnCopyPassword.Font = new Font("Segoe MDL2 Assets", 12F);
            btnCopyPassword.Location = new Point(296, 163);
            btnCopyPassword.Name = "btnCopyPassword";
            btnCopyPassword.Size = new Size(32, 32);
            btnCopyPassword.TabIndex = 5;
            btnCopyPassword.Text = "";
            toolTip.SetToolTip(btnCopyPassword, "העתקת סיסמה מוצפנת");
            btnCopyPassword.UseVisualStyleBackColor = true;
            btnCopyPassword.Click += btnCopyPassword_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold | FontStyle.Underline);
            label1.Location = new Point(113, 9);
            label1.Name = "label1";
            label1.Size = new Size(132, 28);
            label1.TabIndex = 6;
            label1.Text = "יצירת סיסמה";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button1.Location = new Point(128, 531);
            button1.Name = "button1";
            button1.Size = new Size(99, 35);
            button1.TabIndex = 7;
            button1.Text = "סגירה";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe MDL2 Assets", 12F);
            button2.Location = new Point(296, 456);
            button2.Name = "button2";
            button2.Size = new Size(32, 31);
            button2.TabIndex = 15;
            button2.Text = "";
            toolTip.SetToolTip(button2, "העתקת סיסמה מוצפנת");
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // label2
            // 
            label2.BackColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(-4, 268);
            label2.Name = "label2";
            label2.Size = new Size(356, 5);
            label2.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 318);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 9;
            label3.Text = "סיסמה מוצפנת";
            // 
            // txtEncryptedPassword
            // 
            txtEncryptedPassword.Location = new Point(20, 336);
            txtEncryptedPassword.Multiline = true;
            txtEncryptedPassword.Name = "txtEncryptedPassword";
            txtEncryptedPassword.ScrollBars = ScrollBars.Vertical;
            txtEncryptedPassword.Size = new Size(270, 80);
            txtEncryptedPassword.TabIndex = 10;
            // 
            // label4
            // 
            label4.BackColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(-4, 508);
            label4.Name = "label4";
            label4.Size = new Size(356, 10);
            label4.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F, FontStyle.Bold | FontStyle.Underline);
            label5.Location = new Point(113, 289);
            label5.Name = "label5";
            label5.Size = new Size(135, 28);
            label5.TabIndex = 12;
            label5.Text = "שחזור סיסמה";
            // 
            // btnDecryptedPassword
            // 
            btnDecryptedPassword.Location = new Point(20, 422);
            btnDecryptedPassword.Name = "btnDecryptedPassword";
            btnDecryptedPassword.Size = new Size(270, 28);
            btnDecryptedPassword.TabIndex = 13;
            btnDecryptedPassword.Text = "הצג סיסמה";
            btnDecryptedPassword.UseVisualStyleBackColor = true;
            btnDecryptedPassword.Click += button2_Click;
            // 
            // txtDecryptedResult
            // 
            txtDecryptedResult.Location = new Point(20, 456);
            txtDecryptedResult.Name = "txtDecryptedResult";
            txtDecryptedResult.Size = new Size(270, 23);
            txtDecryptedResult.TabIndex = 14;
            // 
            // GenerateUserPasswordForm
            // 
            AcceptButton = btnEncrypt;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(349, 580);
            Controls.Add(button2);
            Controls.Add(txtDecryptedResult);
            Controls.Add(btnDecryptedPassword);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txtEncryptedPassword);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(btnCopyPassword);
            Controls.Add(txtEncryptedResult);
            Controls.Add(lblEncryptedResult);
            Controls.Add(btnEncrypt);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GenerateUserPasswordForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnEncrypt;
        private Label lblEncryptedResult;
        private TextBox txtEncryptedResult;
        private Button btnCopyPassword;
        private Label label1;
        private Button button1;
        private ToolTip toolTip;
        private Label label2;
        private Label label3;
        private TextBox txtEncryptedPassword;
        private Label label4;
        private Label label5;
        private Button btnDecryptedPassword;
        private TextBox txtDecryptedResult;
        private Button button2;
    }
}
