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
            mainTabControl = new TabControl();
            operationsTab = new TabPage();
            loadingSpinner = new PictureBox();
            PriceUpdatesButton = new Button();
            logTextBox = new RichTextBox();
            eslTransferJob = new Button();
            operTableJob = new Button();
            promotionsTempJob = new Button();
            catalogTempJob = new Button();
            tempPullDateTime = new DateTimePicker();
            branchCatalogCheckBox = new CheckBox();
            singleBranchCheckBox = new CheckBox();
            branchLabel = new Label();
            branchList = new ComboBox();
            dataManagementTab = new TabPage();
            dataManagementTabControl = new TabControl();
            itemsTab = new TabPage();
            itemsDataGridView = new DataGridView();
            itemSearchPanel = new Panel();
            itemSearchBranchComboBox = new ComboBox();
            itemSearchBranchLabel = new Label();
            itemSearchBarcodeTextBox = new TextBox();
            itemSearchBarcodeLabel = new Label();
            itemSearchNameTextBox = new TextBox();
            itemSearchNameLabel = new Label();
            itemSearchButton = new Button();
            itemSearchClearButton = new Button();
            itemActionsPanel = new Panel();
            setItemNotSentButton = new Button();
            removePromotionButton = new Button();
            viewItemDetailsButton = new Button();
            promotionsTab = new TabPage();
            promotionsDataGridView = new DataGridView();
            promotionSearchPanel = new Panel();
            promotionSearchBranchComboBox = new ComboBox();
            promotionSearchBranchLabel = new Label();
            promotionSearchKodRadioButton = new RadioButton();
            promotionSearchItemKodRadioButton = new RadioButton();
            promotionSearchValueTextBox = new TextBox();
            promotionSearchValueLabel = new Label();
            promotionSearchButton = new Button();
            promotionSearchClearButton = new Button();
            promotionActionsPanel = new Panel();
            setPromotionNotTransferredButton = new Button();
            viewPromotionDetailsButton = new Button();
            itemsApiTab = new TabPage();
            itemsApiPanel = new Panel();
            itemsApiBranchComboBox = new ComboBox();
            itemsApiBranchLabel = new Label();
            itemsApiItemIdTextBox = new TextBox();
            itemsApiItemIdLabel = new Label();
            itemsApiGetItemButton = new Button();
            itemsApiResultsDataGridView = new DataGridView();
            itemsApiAddSelectedButton = new Button();
            itemsApiAddAllButton = new Button();
            promotionsApiTab = new TabPage();
            promotionsApiPanel = new Panel();
            promotionsApiBranchComboBox = new ComboBox();
            promotionsApiBranchLabel = new Label();
            promotionsApiBarcodeFilterTextBox = new TextBox();
            promotionsApiBarcodeFilterLabel = new Label();
            promotionsApiGetPromotionsButton = new Button();
            promotionsApiApplyFilterButton = new Button();
            promotionsApiResultsDataGridView = new DataGridView();
            promotionsApiAddSelectedButton = new Button();
            promotionsApiAddAllButton = new Button();
            comaxApiPanel = new Panel();
            comaxApiBranchComboBox = new ComboBox();
            comaxApiBranchLabel = new Label();
            comaxApiItemsBarcodesTextBox = new TextBox();
            comaxApiItemsBarcodesLabel = new Label();
            comaxApiGetItemsButton = new Button();
            comaxApiPromotionsBarcodesTextBox = new TextBox();
            comaxApiPromotionsBarcodesLabel = new Label();
            comaxApiGetPromotionsButton = new Button();
            comaxApiResultsDataGridView = new DataGridView();
            comaxApiAddSelectedButton = new Button();
            comaxApiAddAllButton = new Button();
            mainTabControl.SuspendLayout();
            operationsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)loadingSpinner).BeginInit();
            dataManagementTab.SuspendLayout();
            dataManagementTabControl.SuspendLayout();
            itemsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemsDataGridView).BeginInit();
            itemSearchPanel.SuspendLayout();
            itemActionsPanel.SuspendLayout();
            promotionsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)promotionsDataGridView).BeginInit();
            promotionSearchPanel.SuspendLayout();
            promotionActionsPanel.SuspendLayout();
            itemsApiTab.SuspendLayout();
            itemsApiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemsApiResultsDataGridView).BeginInit();
            promotionsApiTab.SuspendLayout();
            promotionsApiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)promotionsApiResultsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comaxApiResultsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // mainTabControl
            // 
            mainTabControl.Controls.Add(operationsTab);
            mainTabControl.Controls.Add(dataManagementTab);
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(0, 0);
            mainTabControl.Margin = new Padding(2);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.RightToLeft = RightToLeft.Yes;
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(902, 578);
            mainTabControl.TabIndex = 11;
            // 
            // operationsTab
            // 
            operationsTab.Controls.Add(loadingSpinner);
            operationsTab.Controls.Add(PriceUpdatesButton);
            operationsTab.Controls.Add(logTextBox);
            operationsTab.Controls.Add(eslTransferJob);
            operationsTab.Controls.Add(operTableJob);
            operationsTab.Controls.Add(promotionsTempJob);
            operationsTab.Controls.Add(catalogTempJob);
            operationsTab.Controls.Add(tempPullDateTime);
            operationsTab.Controls.Add(branchCatalogCheckBox);
            operationsTab.Controls.Add(singleBranchCheckBox);
            operationsTab.Controls.Add(branchLabel);
            operationsTab.Controls.Add(branchList);
            operationsTab.Location = new Point(4, 24);
            operationsTab.Margin = new Padding(2);
            operationsTab.Name = "operationsTab";
            operationsTab.Padding = new Padding(2);
            operationsTab.Size = new Size(692, 452);
            operationsTab.TabIndex = 0;
            operationsTab.Text = "פעולות";
            operationsTab.UseVisualStyleBackColor = true;
            // 
            // PriceUpdatesButton
            // 
            PriceUpdatesButton.Location = new Point(150, 206);
            PriceUpdatesButton.Margin = new Padding(2);
            PriceUpdatesButton.Name = "PriceUpdatesButton";
            PriceUpdatesButton.Size = new Size(233, 20);
            PriceUpdatesButton.TabIndex = 10;
            PriceUpdatesButton.Text = "עדכוני מחירים";
            PriceUpdatesButton.UseVisualStyleBackColor = true;
            PriceUpdatesButton.Click += PriceUpdatesButton_Click;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(41, 314);
            logTextBox.Margin = new Padding(2);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(425, 123);
            logTextBox.TabIndex = 9;
            logTextBox.Text = "";
            // 
            // eslTransferJob
            // 
            eslTransferJob.Location = new Point(150, 275);
            eslTransferJob.Margin = new Padding(2);
            eslTransferJob.Name = "eslTransferJob";
            eslTransferJob.Size = new Size(233, 20);
            eslTransferJob.TabIndex = 8;
            eslTransferJob.Text = "העברה למנג'ר";
            eslTransferJob.UseVisualStyleBackColor = true;
            eslTransferJob.Click += eslTransferJob_Click;
            // 
            // operTableJob
            // 
            operTableJob.Location = new Point(150, 239);
            operTableJob.Margin = new Padding(2);
            operTableJob.Name = "operTableJob";
            operTableJob.Size = new Size(233, 20);
            operTableJob.TabIndex = 7;
            operTableJob.Text = "העברה לטבלה אופרטיבית";
            operTableJob.UseVisualStyleBackColor = true;
            operTableJob.Click += operTableJob_Click;
            // 
            // promotionsTempJob
            // 
            promotionsTempJob.Location = new Point(150, 182);
            promotionsTempJob.Margin = new Padding(2);
            promotionsTempJob.Name = "promotionsTempJob";
            promotionsTempJob.Size = new Size(233, 20);
            promotionsTempJob.TabIndex = 6;
            promotionsTempJob.Text = "ייבוא מבצעים";
            promotionsTempJob.UseVisualStyleBackColor = true;
            promotionsTempJob.Click += promotionsTempJob_Click;
            // 
            // catalogTempJob
            // 
            catalogTempJob.Location = new Point(150, 158);
            catalogTempJob.Margin = new Padding(2);
            catalogTempJob.Name = "catalogTempJob";
            catalogTempJob.Size = new Size(233, 20);
            catalogTempJob.TabIndex = 5;
            catalogTempJob.Text = "ייבוא פריטים קטלוגיים";
            catalogTempJob.UseVisualStyleBackColor = true;
            catalogTempJob.Click += catalogTempJob_Click;
            // 
            // tempPullDateTime
            // 
            tempPullDateTime.AllowDrop = true;
            tempPullDateTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            tempPullDateTime.Format = DateTimePickerFormat.Custom;
            tempPullDateTime.Location = new Point(340, 124);
            tempPullDateTime.Margin = new Padding(2);
            tempPullDateTime.Name = "tempPullDateTime";
            tempPullDateTime.Size = new Size(166, 23);
            tempPullDateTime.TabIndex = 4;
            tempPullDateTime.ValueChanged += tempPullDateTime_ValueChanged;
            // 
            // branchCatalogCheckBox
            // 
            branchCatalogCheckBox.AutoSize = true;
            branchCatalogCheckBox.Location = new Point(386, 95);
            branchCatalogCheckBox.Margin = new Padding(2);
            branchCatalogCheckBox.Name = "branchCatalogCheckBox";
            branchCatalogCheckBox.Size = new Size(114, 19);
            branchCatalogCheckBox.TabIndex = 3;
            branchCatalogCheckBox.Text = "משיכת ספר סניף";
            branchCatalogCheckBox.UseVisualStyleBackColor = true;
            branchCatalogCheckBox.CheckedChanged += branchCatalogCheckBox_CheckedChanged;
            // 
            // singleBranchCheckBox
            // 
            singleBranchCheckBox.AutoSize = true;
            singleBranchCheckBox.Location = new Point(372, 14);
            singleBranchCheckBox.Margin = new Padding(2);
            singleBranchCheckBox.Name = "singleBranchCheckBox";
            singleBranchCheckBox.Size = new Size(125, 19);
            singleBranchCheckBox.TabIndex = 2;
            singleBranchCheckBox.Text = "פעולה על סניף יחיד";
            singleBranchCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            singleBranchCheckBox.UseVisualStyleBackColor = true;
            singleBranchCheckBox.CheckedChanged += singleBranchCheckBox_CheckedChanged;
            // 
            // branchLabel
            // 
            branchLabel.AutoSize = true;
            branchLabel.Location = new Point(472, 41);
            branchLabel.Margin = new Padding(2, 0, 2, 0);
            branchLabel.Name = "branchLabel";
            branchLabel.Size = new Size(30, 15);
            branchLabel.TabIndex = 1;
            branchLabel.Text = "סניף";
            // 
            // branchList
            // 
            branchList.FormattingEnabled = true;
            branchList.Location = new Point(230, 65);
            branchList.Margin = new Padding(2);
            branchList.Name = "branchList";
            branchList.Size = new Size(276, 23);
            branchList.TabIndex = 0;
            // 
            // dataManagementTab
            // 
            dataManagementTab.Controls.Add(dataManagementTabControl);
            dataManagementTab.Location = new Point(4, 24);
            dataManagementTab.Margin = new Padding(2);
            dataManagementTab.Name = "dataManagementTab";
            dataManagementTab.Padding = new Padding(2);
            dataManagementTab.Size = new Size(894, 550);
            dataManagementTab.TabIndex = 1;
            dataManagementTab.Text = "ניהול נתונים";
            dataManagementTab.UseVisualStyleBackColor = true;
            // 
            // dataManagementTabControl
            // 
            dataManagementTabControl.Controls.Add(itemsTab);
            dataManagementTabControl.Controls.Add(promotionsTab);
            dataManagementTabControl.Controls.Add(itemsApiTab);
            dataManagementTabControl.Controls.Add(promotionsApiTab);
            dataManagementTabControl.Dock = DockStyle.Fill;
            dataManagementTabControl.Location = new Point(2, 2);
            dataManagementTabControl.Margin = new Padding(2);
            dataManagementTabControl.Name = "dataManagementTabControl";
            dataManagementTabControl.RightToLeft = RightToLeft.Yes;
            dataManagementTabControl.SelectedIndex = 0;
            dataManagementTabControl.Size = new Size(890, 546);
            dataManagementTabControl.TabIndex = 0;
            // 
            // itemsTab
            // 
            itemsTab.Controls.Add(itemsDataGridView);
            itemsTab.Controls.Add(itemSearchPanel);
            itemsTab.Controls.Add(itemActionsPanel);
            itemsTab.Location = new Point(4, 24);
            itemsTab.Margin = new Padding(2);
            itemsTab.Name = "itemsTab";
            itemsTab.Padding = new Padding(2);
            itemsTab.Size = new Size(882, 518);
            itemsTab.TabIndex = 0;
            itemsTab.Text = "פריטים";
            itemsTab.UseVisualStyleBackColor = true;
            // 
            // itemsDataGridView
            // 
            itemsDataGridView.AllowUserToAddRows = false;
            itemsDataGridView.AllowUserToDeleteRows = false;
            itemsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            itemsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            itemsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemsDataGridView.Location = new Point(4, 90);
            itemsDataGridView.Margin = new Padding(2);
            itemsDataGridView.Name = "itemsDataGridView";
            itemsDataGridView.ReadOnly = true;
            itemsDataGridView.RowHeadersWidth = 62;
            itemsDataGridView.RowTemplate.Height = 33;
            itemsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsDataGridView.Size = new Size(876, 392);
            itemsDataGridView.TabIndex = 0;
            // 
            // itemSearchPanel
            // 
            itemSearchPanel.Controls.Add(itemSearchBranchComboBox);
            itemSearchPanel.Controls.Add(itemSearchBranchLabel);
            itemSearchPanel.Controls.Add(itemSearchBarcodeTextBox);
            itemSearchPanel.Controls.Add(itemSearchBarcodeLabel);
            itemSearchPanel.Controls.Add(itemSearchNameTextBox);
            itemSearchPanel.Controls.Add(itemSearchNameLabel);
            itemSearchPanel.Controls.Add(itemSearchButton);
            itemSearchPanel.Controls.Add(itemSearchClearButton);
            itemSearchPanel.Dock = DockStyle.Top;
            itemSearchPanel.Location = new Point(2, 2);
            itemSearchPanel.Margin = new Padding(2);
            itemSearchPanel.Name = "itemSearchPanel";
            itemSearchPanel.Size = new Size(878, 86);
            itemSearchPanel.TabIndex = 1;
            // 
            // itemSearchBranchComboBox
            // 
            itemSearchBranchComboBox.FormattingEnabled = true;
            itemSearchBranchComboBox.Location = new Point(490, 12);
            itemSearchBranchComboBox.Margin = new Padding(2);
            itemSearchBranchComboBox.Name = "itemSearchBranchComboBox";
            itemSearchBranchComboBox.Size = new Size(141, 23);
            itemSearchBranchComboBox.TabIndex = 0;
            // 
            // itemSearchBranchLabel
            // 
            itemSearchBranchLabel.AutoSize = true;
            itemSearchBranchLabel.Location = new Point(637, 14);
            itemSearchBranchLabel.Margin = new Padding(2, 0, 2, 0);
            itemSearchBranchLabel.Name = "itemSearchBranchLabel";
            itemSearchBranchLabel.Size = new Size(30, 15);
            itemSearchBranchLabel.TabIndex = 1;
            itemSearchBranchLabel.Text = "סניף";
            // 
            // itemSearchBarcodeTextBox
            // 
            itemSearchBarcodeTextBox.Location = new Point(490, 36);
            itemSearchBarcodeTextBox.Margin = new Padding(2);
            itemSearchBarcodeTextBox.Name = "itemSearchBarcodeTextBox";
            itemSearchBarcodeTextBox.Size = new Size(141, 23);
            itemSearchBarcodeTextBox.TabIndex = 2;
            // 
            // itemSearchBarcodeLabel
            // 
            itemSearchBarcodeLabel.AutoSize = true;
            itemSearchBarcodeLabel.Location = new Point(637, 38);
            itemSearchBarcodeLabel.Margin = new Padding(2, 0, 2, 0);
            itemSearchBarcodeLabel.Name = "itemSearchBarcodeLabel";
            itemSearchBarcodeLabel.Size = new Size(38, 15);
            itemSearchBarcodeLabel.TabIndex = 3;
            itemSearchBarcodeLabel.Text = "ברקוד";
            // 
            // itemSearchNameTextBox
            // 
            itemSearchNameTextBox.Location = new Point(490, 60);
            itemSearchNameTextBox.Margin = new Padding(2);
            itemSearchNameTextBox.Name = "itemSearchNameTextBox";
            itemSearchNameTextBox.Size = new Size(141, 23);
            itemSearchNameTextBox.TabIndex = 4;
            // 
            // itemSearchNameLabel
            // 
            itemSearchNameLabel.AutoSize = true;
            itemSearchNameLabel.Location = new Point(637, 62);
            itemSearchNameLabel.Margin = new Padding(2, 0, 2, 0);
            itemSearchNameLabel.Name = "itemSearchNameLabel";
            itemSearchNameLabel.Size = new Size(24, 15);
            itemSearchNameLabel.TabIndex = 5;
            itemSearchNameLabel.Text = "שם";
            // 
            // itemSearchButton
            // 
            itemSearchButton.Location = new Point(420, 36);
            itemSearchButton.Margin = new Padding(2);
            itemSearchButton.Name = "itemSearchButton";
            itemSearchButton.Size = new Size(66, 20);
            itemSearchButton.TabIndex = 6;
            itemSearchButton.Text = "חפש";
            itemSearchButton.UseVisualStyleBackColor = true;
            // 
            // itemSearchClearButton
            // 
            itemSearchClearButton.Location = new Point(350, 36);
            itemSearchClearButton.Margin = new Padding(2);
            itemSearchClearButton.Name = "itemSearchClearButton";
            itemSearchClearButton.Size = new Size(66, 20);
            itemSearchClearButton.TabIndex = 7;
            itemSearchClearButton.Text = "נקה";
            itemSearchClearButton.UseVisualStyleBackColor = true;
            // 
            // itemActionsPanel
            // 
            itemActionsPanel.Controls.Add(setItemNotSentButton);
            itemActionsPanel.Controls.Add(removePromotionButton);
            itemActionsPanel.Controls.Add(viewItemDetailsButton);
            itemActionsPanel.Dock = DockStyle.Bottom;
            itemActionsPanel.Location = new Point(2, 481);
            itemActionsPanel.Margin = new Padding(2);
            itemActionsPanel.Name = "itemActionsPanel";
            itemActionsPanel.Size = new Size(878, 35);
            itemActionsPanel.TabIndex = 2;
            // 
            // setItemNotSentButton
            // 
            setItemNotSentButton.Location = new Point(560, 8);
            setItemNotSentButton.Margin = new Padding(2);
            setItemNotSentButton.Name = "setItemNotSentButton";
            setItemNotSentButton.Size = new Size(105, 20);
            setItemNotSentButton.TabIndex = 0;
            setItemNotSentButton.Text = "סמן כלא נשלח";
            setItemNotSentButton.UseVisualStyleBackColor = true;
            // 
            // removePromotionButton
            // 
            removePromotionButton.Location = new Point(455, 8);
            removePromotionButton.Margin = new Padding(2);
            removePromotionButton.Name = "removePromotionButton";
            removePromotionButton.Size = new Size(105, 20);
            removePromotionButton.TabIndex = 1;
            removePromotionButton.Text = "הסר מבצע";
            removePromotionButton.UseVisualStyleBackColor = true;
            // 
            // viewItemDetailsButton
            // 
            viewItemDetailsButton.Location = new Point(350, 8);
            viewItemDetailsButton.Margin = new Padding(2);
            viewItemDetailsButton.Name = "viewItemDetailsButton";
            viewItemDetailsButton.Size = new Size(105, 20);
            viewItemDetailsButton.TabIndex = 2;
            viewItemDetailsButton.Text = "הצג פרטים";
            viewItemDetailsButton.UseVisualStyleBackColor = true;
            // 
            // promotionsTab
            // 
            promotionsTab.Controls.Add(promotionsDataGridView);
            promotionsTab.Controls.Add(promotionSearchPanel);
            promotionsTab.Controls.Add(promotionActionsPanel);
            promotionsTab.Location = new Point(4, 24);
            promotionsTab.Margin = new Padding(2);
            promotionsTab.Name = "promotionsTab";
            promotionsTab.Padding = new Padding(2);
            promotionsTab.Size = new Size(882, 518);
            promotionsTab.TabIndex = 1;
            promotionsTab.Text = "מבצעים";
            promotionsTab.UseVisualStyleBackColor = true;
            // 
            // promotionsDataGridView
            // 
            promotionsDataGridView.AllowUserToAddRows = false;
            promotionsDataGridView.AllowUserToDeleteRows = false;
            promotionsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            promotionsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            promotionsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            promotionsDataGridView.Location = new Point(4, 90);
            promotionsDataGridView.Margin = new Padding(2);
            promotionsDataGridView.Name = "promotionsDataGridView";
            promotionsDataGridView.ReadOnly = true;
            promotionsDataGridView.RowHeadersWidth = 62;
            promotionsDataGridView.RowTemplate.Height = 33;
            promotionsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            promotionsDataGridView.Size = new Size(876, 392);
            promotionsDataGridView.TabIndex = 0;
            // 
            // promotionSearchPanel
            // 
            promotionSearchPanel.Controls.Add(promotionSearchBranchComboBox);
            promotionSearchPanel.Controls.Add(promotionSearchBranchLabel);
            promotionSearchPanel.Controls.Add(promotionSearchKodRadioButton);
            promotionSearchPanel.Controls.Add(promotionSearchItemKodRadioButton);
            promotionSearchPanel.Controls.Add(promotionSearchValueTextBox);
            promotionSearchPanel.Controls.Add(promotionSearchValueLabel);
            promotionSearchPanel.Controls.Add(promotionSearchButton);
            promotionSearchPanel.Controls.Add(promotionSearchClearButton);
            promotionSearchPanel.Dock = DockStyle.Top;
            promotionSearchPanel.Location = new Point(2, 2);
            promotionSearchPanel.Margin = new Padding(2);
            promotionSearchPanel.Name = "promotionSearchPanel";
            promotionSearchPanel.Size = new Size(878, 86);
            promotionSearchPanel.TabIndex = 1;
            // 
            // promotionSearchBranchComboBox
            // 
            promotionSearchBranchComboBox.FormattingEnabled = true;
            promotionSearchBranchComboBox.Location = new Point(490, 12);
            promotionSearchBranchComboBox.Margin = new Padding(2);
            promotionSearchBranchComboBox.Name = "promotionSearchBranchComboBox";
            promotionSearchBranchComboBox.Size = new Size(141, 23);
            promotionSearchBranchComboBox.TabIndex = 0;
            // 
            // promotionSearchBranchLabel
            // 
            promotionSearchBranchLabel.AutoSize = true;
            promotionSearchBranchLabel.Location = new Point(637, 14);
            promotionSearchBranchLabel.Margin = new Padding(2, 0, 2, 0);
            promotionSearchBranchLabel.Name = "promotionSearchBranchLabel";
            promotionSearchBranchLabel.Size = new Size(30, 15);
            promotionSearchBranchLabel.TabIndex = 1;
            promotionSearchBranchLabel.Text = "סניף";
            // 
            // promotionSearchKodRadioButton
            // 
            promotionSearchKodRadioButton.AutoSize = true;
            promotionSearchKodRadioButton.Location = new Point(560, 36);
            promotionSearchKodRadioButton.Margin = new Padding(2);
            promotionSearchKodRadioButton.Name = "promotionSearchKodRadioButton";
            promotionSearchKodRadioButton.Size = new Size(74, 19);
            promotionSearchKodRadioButton.TabIndex = 2;
            promotionSearchKodRadioButton.Text = "קוד מבצע";
            promotionSearchKodRadioButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchItemKodRadioButton
            // 
            promotionSearchItemKodRadioButton.AutoSize = true;
            promotionSearchItemKodRadioButton.Checked = true;
            promotionSearchItemKodRadioButton.Location = new Point(560, 54);
            promotionSearchItemKodRadioButton.Margin = new Padding(2);
            promotionSearchItemKodRadioButton.Name = "promotionSearchItemKodRadioButton";
            promotionSearchItemKodRadioButton.Size = new Size(71, 19);
            promotionSearchItemKodRadioButton.TabIndex = 3;
            promotionSearchItemKodRadioButton.TabStop = true;
            promotionSearchItemKodRadioButton.Text = "קוד פריט";
            promotionSearchItemKodRadioButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchValueTextBox
            // 
            promotionSearchValueTextBox.Location = new Point(420, 45);
            promotionSearchValueTextBox.Margin = new Padding(2);
            promotionSearchValueTextBox.Name = "promotionSearchValueTextBox";
            promotionSearchValueTextBox.Size = new Size(106, 23);
            promotionSearchValueTextBox.TabIndex = 4;
            // 
            // promotionSearchValueLabel
            // 
            promotionSearchValueLabel.AutoSize = true;
            promotionSearchValueLabel.Location = new Point(532, 47);
            promotionSearchValueLabel.Margin = new Padding(2, 0, 2, 0);
            promotionSearchValueLabel.Name = "promotionSearchValueLabel";
            promotionSearchValueLabel.Size = new Size(28, 15);
            promotionSearchValueLabel.TabIndex = 5;
            promotionSearchValueLabel.Text = "ערך";
            // 
            // promotionSearchButton
            // 
            promotionSearchButton.Location = new Point(350, 45);
            promotionSearchButton.Margin = new Padding(2);
            promotionSearchButton.Name = "promotionSearchButton";
            promotionSearchButton.Size = new Size(66, 20);
            promotionSearchButton.TabIndex = 6;
            promotionSearchButton.Text = "חפש";
            promotionSearchButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchClearButton
            // 
            promotionSearchClearButton.Location = new Point(280, 45);
            promotionSearchClearButton.Margin = new Padding(2);
            promotionSearchClearButton.Name = "promotionSearchClearButton";
            promotionSearchClearButton.Size = new Size(66, 20);
            promotionSearchClearButton.TabIndex = 7;
            promotionSearchClearButton.Text = "נקה";
            promotionSearchClearButton.UseVisualStyleBackColor = true;
            // 
            // promotionActionsPanel
            // 
            promotionActionsPanel.Controls.Add(setPromotionNotTransferredButton);
            promotionActionsPanel.Controls.Add(viewPromotionDetailsButton);
            promotionActionsPanel.Dock = DockStyle.Bottom;
            promotionActionsPanel.Location = new Point(2, 481);
            promotionActionsPanel.Margin = new Padding(2);
            promotionActionsPanel.Name = "promotionActionsPanel";
            promotionActionsPanel.Size = new Size(878, 35);
            promotionActionsPanel.TabIndex = 2;
            // 
            // setPromotionNotTransferredButton
            // 
            setPromotionNotTransferredButton.Location = new Point(560, 8);
            setPromotionNotTransferredButton.Margin = new Padding(2);
            setPromotionNotTransferredButton.Name = "setPromotionNotTransferredButton";
            setPromotionNotTransferredButton.Size = new Size(105, 20);
            setPromotionNotTransferredButton.TabIndex = 0;
            setPromotionNotTransferredButton.Text = "סמן כלא הועבר";
            setPromotionNotTransferredButton.UseVisualStyleBackColor = true;
            // 
            // viewPromotionDetailsButton
            // 
            viewPromotionDetailsButton.Location = new Point(455, 8);
            viewPromotionDetailsButton.Margin = new Padding(2);
            viewPromotionDetailsButton.Name = "viewPromotionDetailsButton";
            viewPromotionDetailsButton.Size = new Size(105, 20);
            viewPromotionDetailsButton.TabIndex = 1;
            viewPromotionDetailsButton.Text = "הצג פרטים";
            viewPromotionDetailsButton.UseVisualStyleBackColor = true;
            // 
            // itemsApiTab
            // 
            itemsApiTab.Controls.Add(itemsApiPanel);
            itemsApiTab.Controls.Add(itemsApiResultsDataGridView);
            itemsApiTab.Controls.Add(itemsApiAddSelectedButton);
            itemsApiTab.Controls.Add(itemsApiAddAllButton);
            itemsApiTab.Location = new Point(4, 24);
            itemsApiTab.Margin = new Padding(2);
            itemsApiTab.Name = "itemsApiTab";
            itemsApiTab.Padding = new Padding(2);
            itemsApiTab.Size = new Size(882, 518);
            itemsApiTab.TabIndex = 2;
            itemsApiTab.Text = "ממשק API פריטים";
            itemsApiTab.UseVisualStyleBackColor = true;
            // 
            // itemsApiPanel
            // 
            itemsApiPanel.Controls.Add(itemsApiBranchComboBox);
            itemsApiPanel.Controls.Add(itemsApiBranchLabel);
            itemsApiPanel.Controls.Add(itemsApiItemIdTextBox);
            itemsApiPanel.Controls.Add(itemsApiItemIdLabel);
            itemsApiPanel.Controls.Add(itemsApiGetItemButton);
            itemsApiPanel.Dock = DockStyle.Top;
            itemsApiPanel.Location = new Point(2, 2);
            itemsApiPanel.Margin = new Padding(2);
            itemsApiPanel.Name = "itemsApiPanel";
            itemsApiPanel.Size = new Size(878, 60);
            itemsApiPanel.TabIndex = 0;
            // 
            // itemsApiBranchComboBox
            // 
            itemsApiBranchComboBox.FormattingEnabled = true;
            itemsApiBranchComboBox.Location = new Point(350, 6);
            itemsApiBranchComboBox.Margin = new Padding(2);
            itemsApiBranchComboBox.Name = "itemsApiBranchComboBox";
            itemsApiBranchComboBox.Size = new Size(141, 23);
            itemsApiBranchComboBox.TabIndex = 0;
            // 
            // itemsApiBranchLabel
            // 
            itemsApiBranchLabel.AutoSize = true;
            itemsApiBranchLabel.Location = new Point(517, 9);
            itemsApiBranchLabel.Margin = new Padding(2, 0, 2, 0);
            itemsApiBranchLabel.Name = "itemsApiBranchLabel";
            itemsApiBranchLabel.Size = new Size(30, 15);
            itemsApiBranchLabel.TabIndex = 1;
            itemsApiBranchLabel.Text = "סניף";
            // 
            // itemsApiItemIdTextBox
            // 
            itemsApiItemIdTextBox.Location = new Point(210, 33);
            itemsApiItemIdTextBox.Margin = new Padding(2);
            itemsApiItemIdTextBox.Name = "itemsApiItemIdTextBox";
            itemsApiItemIdTextBox.Size = new Size(281, 23);
            itemsApiItemIdTextBox.TabIndex = 2;
            // 
            // itemsApiItemIdLabel
            // 
            itemsApiItemIdLabel.AutoSize = true;
            itemsApiItemIdLabel.Location = new Point(497, 38);
            itemsApiItemIdLabel.Margin = new Padding(2, 0, 2, 0);
            itemsApiItemIdLabel.Name = "itemsApiItemIdLabel";
            itemsApiItemIdLabel.Size = new Size(169, 15);
            itemsApiItemIdLabel.TabIndex = 3;
            itemsApiItemIdLabel.Text = "ברקוד פריט (מופרדים בפסיקים)";
            // 
            // itemsApiGetItemButton
            // 
            itemsApiGetItemButton.Location = new Point(119, 32);
            itemsApiGetItemButton.Margin = new Padding(2);
            itemsApiGetItemButton.Name = "itemsApiGetItemButton";
            itemsApiGetItemButton.Size = new Size(75, 26);
            itemsApiGetItemButton.TabIndex = 4;
            itemsApiGetItemButton.Text = "משוך פריטים";
            itemsApiGetItemButton.UseVisualStyleBackColor = true;
            // 
            // itemsApiResultsDataGridView
            // 
            itemsApiResultsDataGridView.AllowUserToAddRows = false;
            itemsApiResultsDataGridView.AllowUserToDeleteRows = false;
            itemsApiResultsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            itemsApiResultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            itemsApiResultsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemsApiResultsDataGridView.Location = new Point(4, 66);
            itemsApiResultsDataGridView.Margin = new Padding(2);
            itemsApiResultsDataGridView.Name = "itemsApiResultsDataGridView";
            itemsApiResultsDataGridView.ReadOnly = true;
            itemsApiResultsDataGridView.RowHeadersWidth = 62;
            itemsApiResultsDataGridView.RowTemplate.Height = 33;
            itemsApiResultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsApiResultsDataGridView.Size = new Size(876, 416);
            itemsApiResultsDataGridView.TabIndex = 1;
            // 
            // itemsApiAddSelectedButton
            // 
            itemsApiAddSelectedButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            itemsApiAddSelectedButton.Location = new Point(760, 486);
            itemsApiAddSelectedButton.Margin = new Padding(2);
            itemsApiAddSelectedButton.Name = "itemsApiAddSelectedButton";
            itemsApiAddSelectedButton.Size = new Size(105, 20);
            itemsApiAddSelectedButton.TabIndex = 2;
            itemsApiAddSelectedButton.Text = "הוסף נבחרים";
            itemsApiAddSelectedButton.UseVisualStyleBackColor = true;
            // 
            // itemsApiAddAllButton
            // 
            itemsApiAddAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            itemsApiAddAllButton.Location = new Point(655, 486);
            itemsApiAddAllButton.Margin = new Padding(2);
            itemsApiAddAllButton.Name = "itemsApiAddAllButton";
            itemsApiAddAllButton.Size = new Size(105, 20);
            itemsApiAddAllButton.TabIndex = 3;
            itemsApiAddAllButton.Text = "הוסף הכל";
            itemsApiAddAllButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiTab
            // 
            promotionsApiTab.Controls.Add(promotionsApiPanel);
            promotionsApiTab.Controls.Add(promotionsApiResultsDataGridView);
            promotionsApiTab.Controls.Add(promotionsApiAddSelectedButton);
            promotionsApiTab.Controls.Add(promotionsApiAddAllButton);
            promotionsApiTab.Location = new Point(4, 24);
            promotionsApiTab.Margin = new Padding(2);
            promotionsApiTab.Name = "promotionsApiTab";
            promotionsApiTab.Padding = new Padding(2);
            promotionsApiTab.Size = new Size(680, 420);
            promotionsApiTab.TabIndex = 3;
            promotionsApiTab.Text = "ממשק API מבצעים";
            promotionsApiTab.UseVisualStyleBackColor = true;
            // 
            // promotionsApiPanel
            // 
            promotionsApiPanel.Controls.Add(promotionsApiBranchComboBox);
            promotionsApiPanel.Controls.Add(promotionsApiBranchLabel);
            promotionsApiPanel.Controls.Add(promotionsApiBarcodeFilterTextBox);
            promotionsApiPanel.Controls.Add(promotionsApiBarcodeFilterLabel);
            promotionsApiPanel.Controls.Add(promotionsApiGetPromotionsButton);
            promotionsApiPanel.Controls.Add(promotionsApiApplyFilterButton);
            promotionsApiPanel.Dock = DockStyle.Top;
            promotionsApiPanel.Location = new Point(2, 2);
            promotionsApiPanel.Margin = new Padding(2);
            promotionsApiPanel.Name = "promotionsApiPanel";
            promotionsApiPanel.Size = new Size(676, 60);
            promotionsApiPanel.TabIndex = 0;
            // 
            // promotionsApiBranchComboBox
            // 
            promotionsApiBranchComboBox.FormattingEnabled = true;
            promotionsApiBranchComboBox.Location = new Point(350, 6);
            promotionsApiBranchComboBox.Margin = new Padding(2);
            promotionsApiBranchComboBox.Name = "promotionsApiBranchComboBox";
            promotionsApiBranchComboBox.Size = new Size(141, 23);
            promotionsApiBranchComboBox.TabIndex = 0;
            // 
            // promotionsApiBranchLabel
            // 
            promotionsApiBranchLabel.AutoSize = true;
            promotionsApiBranchLabel.Location = new Point(517, 9);
            promotionsApiBranchLabel.Margin = new Padding(2, 0, 2, 0);
            promotionsApiBranchLabel.Name = "promotionsApiBranchLabel";
            promotionsApiBranchLabel.Size = new Size(30, 15);
            promotionsApiBranchLabel.TabIndex = 1;
            promotionsApiBranchLabel.Text = "סניף";
            // 
            // promotionsApiBarcodeFilterTextBox
            // 
            promotionsApiBarcodeFilterTextBox.Location = new Point(210, 30);
            promotionsApiBarcodeFilterTextBox.Margin = new Padding(2);
            promotionsApiBarcodeFilterTextBox.Name = "promotionsApiBarcodeFilterTextBox";
            promotionsApiBarcodeFilterTextBox.Size = new Size(281, 23);
            promotionsApiBarcodeFilterTextBox.TabIndex = 2;
            // 
            // promotionsApiBarcodeFilterLabel
            // 
            promotionsApiBarcodeFilterLabel.AutoSize = true;
            promotionsApiBarcodeFilterLabel.Location = new Point(495, 33);
            promotionsApiBarcodeFilterLabel.Margin = new Padding(2, 0, 2, 0);
            promotionsApiBarcodeFilterLabel.Name = "promotionsApiBarcodeFilterLabel";
            promotionsApiBarcodeFilterLabel.Size = new Size(84, 15);
            promotionsApiBarcodeFilterLabel.TabIndex = 3;
            promotionsApiBarcodeFilterLabel.Text = "סינון לפי ברקוד";
            // 
            // promotionsApiGetPromotionsButton
            // 
            promotionsApiGetPromotionsButton.Location = new Point(146, 26);
            promotionsApiGetPromotionsButton.Margin = new Padding(2);
            promotionsApiGetPromotionsButton.Name = "promotionsApiGetPromotionsButton";
            promotionsApiGetPromotionsButton.Size = new Size(63, 29);
            promotionsApiGetPromotionsButton.TabIndex = 4;
            promotionsApiGetPromotionsButton.Text = "משוך מבצעים";
            promotionsApiGetPromotionsButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiApplyFilterButton
            // 
            promotionsApiApplyFilterButton.Location = new Point(81, 27);
            promotionsApiApplyFilterButton.Margin = new Padding(2);
            promotionsApiApplyFilterButton.Name = "promotionsApiApplyFilterButton";
            promotionsApiApplyFilterButton.Size = new Size(61, 26);
            promotionsApiApplyFilterButton.TabIndex = 5;
            promotionsApiApplyFilterButton.Text = "סנן";
            promotionsApiApplyFilterButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiResultsDataGridView
            // 
            promotionsApiResultsDataGridView.AllowUserToAddRows = false;
            promotionsApiResultsDataGridView.AllowUserToDeleteRows = false;
            promotionsApiResultsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            promotionsApiResultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            promotionsApiResultsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            promotionsApiResultsDataGridView.Location = new Point(4, 66);
            promotionsApiResultsDataGridView.Margin = new Padding(2);
            promotionsApiResultsDataGridView.Name = "promotionsApiResultsDataGridView";
            promotionsApiResultsDataGridView.ReadOnly = true;
            promotionsApiResultsDataGridView.RowHeadersWidth = 62;
            promotionsApiResultsDataGridView.RowTemplate.Height = 33;
            promotionsApiResultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            promotionsApiResultsDataGridView.Size = new Size(674, 318);
            promotionsApiResultsDataGridView.TabIndex = 1;
            // 
            // promotionsApiAddSelectedButton
            // 
            promotionsApiAddSelectedButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            promotionsApiAddSelectedButton.Location = new Point(558, 388);
            promotionsApiAddSelectedButton.Margin = new Padding(2);
            promotionsApiAddSelectedButton.Name = "promotionsApiAddSelectedButton";
            promotionsApiAddSelectedButton.Size = new Size(105, 20);
            promotionsApiAddSelectedButton.TabIndex = 2;
            promotionsApiAddSelectedButton.Text = "הוסף נבחרים";
            promotionsApiAddSelectedButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiAddAllButton
            // 
            promotionsApiAddAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            promotionsApiAddAllButton.Location = new Point(453, 388);
            promotionsApiAddAllButton.Margin = new Padding(2);
            promotionsApiAddAllButton.Name = "promotionsApiAddAllButton";
            promotionsApiAddAllButton.Size = new Size(105, 20);
            promotionsApiAddAllButton.TabIndex = 3;
            promotionsApiAddAllButton.Text = "הוסף הכל";
            promotionsApiAddAllButton.UseVisualStyleBackColor = true;
            // 
            // comaxApiPanel
            // 
            comaxApiPanel.Location = new Point(0, 0);
            comaxApiPanel.Name = "comaxApiPanel";
            comaxApiPanel.Size = new Size(200, 100);
            comaxApiPanel.TabIndex = 0;
            // 
            // comaxApiBranchComboBox
            // 
            comaxApiBranchComboBox.Location = new Point(0, 0);
            comaxApiBranchComboBox.Name = "comaxApiBranchComboBox";
            comaxApiBranchComboBox.Size = new Size(121, 23);
            comaxApiBranchComboBox.TabIndex = 0;
            // 
            // comaxApiBranchLabel
            // 
            comaxApiBranchLabel.Location = new Point(0, 0);
            comaxApiBranchLabel.Name = "comaxApiBranchLabel";
            comaxApiBranchLabel.Size = new Size(100, 23);
            comaxApiBranchLabel.TabIndex = 0;
            // 
            // comaxApiItemsBarcodesTextBox
            // 
            comaxApiItemsBarcodesTextBox.Location = new Point(0, 0);
            comaxApiItemsBarcodesTextBox.Name = "comaxApiItemsBarcodesTextBox";
            comaxApiItemsBarcodesTextBox.Size = new Size(100, 23);
            comaxApiItemsBarcodesTextBox.TabIndex = 0;
            // 
            // comaxApiItemsBarcodesLabel
            // 
            comaxApiItemsBarcodesLabel.Location = new Point(0, 0);
            comaxApiItemsBarcodesLabel.Name = "comaxApiItemsBarcodesLabel";
            comaxApiItemsBarcodesLabel.Size = new Size(100, 23);
            comaxApiItemsBarcodesLabel.TabIndex = 0;
            // 
            // comaxApiGetItemsButton
            // 
            comaxApiGetItemsButton.Location = new Point(0, 0);
            comaxApiGetItemsButton.Name = "comaxApiGetItemsButton";
            comaxApiGetItemsButton.Size = new Size(75, 23);
            comaxApiGetItemsButton.TabIndex = 0;
            // 
            // comaxApiPromotionsBarcodesTextBox
            // 
            comaxApiPromotionsBarcodesTextBox.Location = new Point(0, 0);
            comaxApiPromotionsBarcodesTextBox.Name = "comaxApiPromotionsBarcodesTextBox";
            comaxApiPromotionsBarcodesTextBox.Size = new Size(100, 23);
            comaxApiPromotionsBarcodesTextBox.TabIndex = 0;
            // 
            // comaxApiPromotionsBarcodesLabel
            // 
            comaxApiPromotionsBarcodesLabel.Location = new Point(0, 0);
            comaxApiPromotionsBarcodesLabel.Name = "comaxApiPromotionsBarcodesLabel";
            comaxApiPromotionsBarcodesLabel.Size = new Size(100, 23);
            comaxApiPromotionsBarcodesLabel.TabIndex = 0;
            // 
            // comaxApiGetPromotionsButton
            // 
            comaxApiGetPromotionsButton.Location = new Point(0, 0);
            comaxApiGetPromotionsButton.Name = "comaxApiGetPromotionsButton";
            comaxApiGetPromotionsButton.Size = new Size(75, 23);
            comaxApiGetPromotionsButton.TabIndex = 0;
            // 
            // comaxApiResultsDataGridView
            // 
            comaxApiResultsDataGridView.Location = new Point(0, 0);
            comaxApiResultsDataGridView.Name = "comaxApiResultsDataGridView";
            comaxApiResultsDataGridView.Size = new Size(240, 150);
            comaxApiResultsDataGridView.TabIndex = 0;
            // 
            // comaxApiAddSelectedButton
            // 
            comaxApiAddSelectedButton.Location = new Point(0, 0);
            comaxApiAddSelectedButton.Name = "comaxApiAddSelectedButton";
            comaxApiAddSelectedButton.Size = new Size(75, 23);
            comaxApiAddSelectedButton.TabIndex = 0;
            // 
            // comaxApiAddAllButton
            // 
            comaxApiAddAllButton.Location = new Point(0, 0);
            comaxApiAddAllButton.Name = "comaxApiAddAllButton";
            comaxApiAddAllButton.Size = new Size(75, 23);
            comaxApiAddAllButton.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(902, 578);
            Controls.Add(mainTabControl);
            Margin = new Padding(2);
            Name = "Form1";
            RightToLeft = RightToLeft.Yes;
            Text = "ניהול נתונים";
            mainTabControl.ResumeLayout(false);
            operationsTab.ResumeLayout(false);
            operationsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)loadingSpinner).EndInit();
            dataManagementTab.ResumeLayout(false);
            dataManagementTabControl.ResumeLayout(false);
            itemsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)itemsDataGridView).EndInit();
            itemSearchPanel.ResumeLayout(false);
            itemSearchPanel.PerformLayout();
            itemActionsPanel.ResumeLayout(false);
            promotionsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)promotionsDataGridView).EndInit();
            promotionSearchPanel.ResumeLayout(false);
            promotionSearchPanel.PerformLayout();
            promotionActionsPanel.ResumeLayout(false);
            itemsApiTab.ResumeLayout(false);
            itemsApiPanel.ResumeLayout(false);
            itemsApiPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)itemsApiResultsDataGridView).EndInit();
            promotionsApiTab.ResumeLayout(false);
            promotionsApiPanel.ResumeLayout(false);
            promotionsApiPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)promotionsApiResultsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)comaxApiResultsDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl mainTabControl;
        private TabPage operationsTab;
        private TabPage dataManagementTab;
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
        private PictureBox loadingSpinner;
        private Button PriceUpdatesButton;
        private TabControl dataManagementTabControl;
        private TabPage itemsTab;
        private TabPage promotionsTab;
        // comaxApiTab removed
        private TabPage itemsApiTab;
        private TabPage promotionsApiTab;
        private DataGridView itemsDataGridView;
        private DataGridView promotionsDataGridView;
        private Panel itemSearchPanel;
        private ComboBox itemSearchBranchComboBox;
        private Label itemSearchBranchLabel;
        private TextBox itemSearchBarcodeTextBox;
        private Label itemSearchBarcodeLabel;
        private TextBox itemSearchNameTextBox;
        private Label itemSearchNameLabel;
        private Button itemSearchButton;
        private Button itemSearchClearButton;
        private Panel itemActionsPanel;
        private Button setItemNotSentButton;
        private Button removePromotionButton;
        private Button viewItemDetailsButton;
        private Panel promotionSearchPanel;
        private ComboBox promotionSearchBranchComboBox;
        private Label promotionSearchBranchLabel;
        private RadioButton promotionSearchKodRadioButton;
        private RadioButton promotionSearchItemKodRadioButton;
        private TextBox promotionSearchValueTextBox;
        private Label promotionSearchValueLabel;
        private Button promotionSearchButton;
        private Button promotionSearchClearButton;
        private Panel promotionActionsPanel;
        private Button setPromotionNotTransferredButton;
        private Button viewPromotionDetailsButton;
        private Panel comaxApiPanel;
        private ComboBox comaxApiBranchComboBox;
        private Label comaxApiBranchLabel;
        private TextBox comaxApiItemsBarcodesTextBox;
        private Label comaxApiItemsBarcodesLabel;
        private Button comaxApiGetItemsButton;
        private TextBox comaxApiPromotionsBarcodesTextBox;
        private Label comaxApiPromotionsBarcodesLabel;
        private Button comaxApiGetPromotionsButton;
        private DataGridView comaxApiResultsDataGridView;
        private Button comaxApiAddSelectedButton;
        private Button comaxApiAddAllButton;
        
        // Items API Tab Controls
        private Panel itemsApiPanel;
        private ComboBox itemsApiBranchComboBox;
        private Label itemsApiBranchLabel;
        private TextBox itemsApiItemIdTextBox;
        private Label itemsApiItemIdLabel;
        private Button itemsApiGetItemButton;
        private DataGridView itemsApiResultsDataGridView;
        private Button itemsApiAddSelectedButton;
        private Button itemsApiAddAllButton;
        
        // Promotions API Tab Controls
        private Panel promotionsApiPanel;
        private ComboBox promotionsApiBranchComboBox;
        private Label promotionsApiBranchLabel;
        private TextBox promotionsApiBarcodeFilterTextBox;
        private Label promotionsApiBarcodeFilterLabel;
        private Button promotionsApiGetPromotionsButton;
        private Button promotionsApiApplyFilterButton;
        private DataGridView promotionsApiResultsDataGridView;
        private Button promotionsApiAddSelectedButton;
        private Button promotionsApiAddAllButton;
    }
}
