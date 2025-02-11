namespace Em.Comax.ShukHerzel.Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            branchList = new ComboBox();
            branchLabel = new Label();
            singleBranchCheckBox = new CheckBox();
            branchCatalogCheckBox = new CheckBox();
            tempPullDateTime = new DateTimePicker();
            catalogTempJob = new Button();
            promotionsTempJob = new Button();
            operTableJob = new Button();
            eslTransferJob = new Button();
            logTextBox = new RichTextBox();
            PriceUpdatesButton = new Button();
            SuspendLayout();
            // 
            // branchList
            // 
            branchList.FormattingEnabled = true;
            branchList.Location = new Point(328, 109);
            branchList.Name = "branchList";
            branchList.Size = new Size(393, 33);
            branchList.TabIndex = 0;
            // 
            // branchLabel
            // 
            branchLabel.AutoSize = true;
            branchLabel.Location = new Point(674, 68);
            branchLabel.Name = "branchLabel";
            branchLabel.Size = new Size(47, 25);
            branchLabel.TabIndex = 1;
            branchLabel.Text = "סניף";
            // 
            // singleBranchCheckBox
            // 
            singleBranchCheckBox.AutoSize = true;
            singleBranchCheckBox.Location = new Point(532, 24);
            singleBranchCheckBox.Name = "singleBranchCheckBox";
            singleBranchCheckBox.Size = new Size(189, 29);
            singleBranchCheckBox.TabIndex = 2;
            singleBranchCheckBox.Text = "פעולה על סניף יחיד";
            singleBranchCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            singleBranchCheckBox.UseVisualStyleBackColor = true;
            singleBranchCheckBox.CheckedChanged += singleBranchCheckBox_CheckedChanged;
            // 
            // branchCatalogCheckBox
            // 
            branchCatalogCheckBox.AutoSize = true;
            branchCatalogCheckBox.Location = new Point(551, 159);
            branchCatalogCheckBox.Name = "branchCatalogCheckBox";
            branchCatalogCheckBox.Size = new Size(170, 29);
            branchCatalogCheckBox.TabIndex = 3;
            branchCatalogCheckBox.Text = "משיכת ספר סניף";
            branchCatalogCheckBox.UseVisualStyleBackColor = true;
            branchCatalogCheckBox.CheckedChanged += branchCatalogCheckBox_CheckedChanged;
            // 
            // tempPullDateTime
            // 
            tempPullDateTime.AllowDrop = true;
            tempPullDateTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            tempPullDateTime.Format = DateTimePickerFormat.Custom;
            tempPullDateTime.Location = new Point(485, 207);
            tempPullDateTime.Name = "tempPullDateTime";
            tempPullDateTime.Size = new Size(236, 31);
            tempPullDateTime.TabIndex = 4;
            tempPullDateTime.ValueChanged += tempPullDateTime_ValueChanged;
            // 
            // catalogTempJob
            // 
            catalogTempJob.Location = new Point(215, 264);
            catalogTempJob.Name = "catalogTempJob";
            catalogTempJob.Size = new Size(333, 34);
            catalogTempJob.TabIndex = 5;
            catalogTempJob.Text = "ייבוא פריטים קטלוגיים";
            catalogTempJob.UseVisualStyleBackColor = true;
            catalogTempJob.Click += catalogTempJob_Click;
            // 
            // promotionsTempJob
            // 
            promotionsTempJob.Location = new Point(215, 304);
            promotionsTempJob.Name = "promotionsTempJob";
            promotionsTempJob.Size = new Size(333, 34);
            promotionsTempJob.TabIndex = 6;
            promotionsTempJob.Text = "ייבוא מבצעים";
            promotionsTempJob.UseVisualStyleBackColor = true;
            promotionsTempJob.Click += promotionsTempJob_Click;
            // 
            // operTableJob
            // 
            operTableJob.Location = new Point(215, 398);
            operTableJob.Name = "operTableJob";
            operTableJob.Size = new Size(333, 34);
            operTableJob.TabIndex = 7;
            operTableJob.Text = "העברה לטבלה אופרטיבית";
            operTableJob.UseVisualStyleBackColor = true;
            operTableJob.Click += operTableJob_Click;
            // 
            // eslTransferJob
            // 
            eslTransferJob.Location = new Point(215, 458);
            eslTransferJob.Name = "eslTransferJob";
            eslTransferJob.Size = new Size(333, 34);
            eslTransferJob.TabIndex = 8;
            eslTransferJob.Text = "העברה למנג'ר";
            eslTransferJob.UseVisualStyleBackColor = true;
            eslTransferJob.Click += eslTransferJob_Click;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(58, 523);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(606, 203);
            logTextBox.TabIndex = 9;
            logTextBox.Text = "";
            // 
            // PriceUpdatesButton
            // 
            PriceUpdatesButton.Location = new Point(215, 344);
            PriceUpdatesButton.Name = "PriceUpdatesButton";
            PriceUpdatesButton.Size = new Size(333, 34);
            PriceUpdatesButton.TabIndex = 10;
            PriceUpdatesButton.Text = "עדכוני מחירים";
            PriceUpdatesButton.UseVisualStyleBackColor = true;
            PriceUpdatesButton.Click += PriceUpdatesButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(733, 781);
            Controls.Add(PriceUpdatesButton);
            Controls.Add(logTextBox);
            Controls.Add(eslTransferJob);
            Controls.Add(operTableJob);
            Controls.Add(promotionsTempJob);
            Controls.Add(catalogTempJob);
            Controls.Add(tempPullDateTime);
            Controls.Add(branchCatalogCheckBox);
            Controls.Add(singleBranchCheckBox);
            Controls.Add(branchLabel);
            Controls.Add(branchList);
            Name = "Form1";
            RightToLeft = RightToLeft.Yes;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox branchList;
        private Label branchLabel;
        private CheckBox singleBranchCheckBox;
        private CheckBox branchCatalogCheckBox;
        private DateTimePicker tempPullDateTime;
        private Button catalogTempJob;
        private Button promotionsTempJob;
        private Button operTableJob;
        private Button eslTransferJob;
        private RichTextBox logTextBox;
        private Button PriceUpdatesButton;
    }
}
