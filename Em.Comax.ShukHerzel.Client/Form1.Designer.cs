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
            catalogNewTempJob = new Button();
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
            loadingSpinner = new PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)loadingSpinner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comaxApiResultsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // mainTabControl
            // 
            mainTabControl.Controls.Add(operationsTab);
            mainTabControl.Controls.Add(dataManagementTab);
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(0, 0);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.RightToLeft = RightToLeft.Yes;
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(1289, 963);
            mainTabControl.TabIndex = 11;
            // 
            // operationsTab
            // 
            operationsTab.Controls.Add(catalogNewTempJob);
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
            operationsTab.Location = new Point(4, 34);
            operationsTab.Name = "operationsTab";
            operationsTab.Padding = new Padding(3);
            operationsTab.Size = new Size(1281, 925);
            operationsTab.TabIndex = 0;
            operationsTab.Text = "פעולות";
            operationsTab.UseVisualStyleBackColor = true;
            // 
            // catalogNewTempJob
            // 
            catalogNewTempJob.Location = new Point(214, 302);
            catalogNewTempJob.Name = "catalogNewTempJob";
            catalogNewTempJob.Size = new Size(333, 33);
            catalogNewTempJob.TabIndex = 11;
            catalogNewTempJob.Text = "ייבוא פריטים קטלוגיים חדש";
            catalogNewTempJob.UseVisualStyleBackColor = true;
            catalogNewTempJob.Click += catalogNewTempJob_Click;
            // 
            // PriceUpdatesButton
            // 
            PriceUpdatesButton.Location = new Point(214, 392);
            PriceUpdatesButton.Name = "PriceUpdatesButton";
            PriceUpdatesButton.Size = new Size(333, 33);
            PriceUpdatesButton.TabIndex = 10;
            PriceUpdatesButton.Text = "עדכוני מחירים";
            PriceUpdatesButton.UseVisualStyleBackColor = true;
            PriceUpdatesButton.Click += PriceUpdatesButton_Click;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(59, 523);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(605, 202);
            logTextBox.TabIndex = 9;
            logTextBox.Text = "";
            // 
            // eslTransferJob
            // 
            eslTransferJob.Location = new Point(214, 484);
            eslTransferJob.Name = "eslTransferJob";
            eslTransferJob.Size = new Size(333, 33);
            eslTransferJob.TabIndex = 8;
            eslTransferJob.Text = "העברה למנג'ר";
            eslTransferJob.UseVisualStyleBackColor = true;
            eslTransferJob.Click += eslTransferJob_Click;
            // 
            // operTableJob
            // 
            operTableJob.Location = new Point(214, 438);
            operTableJob.Name = "operTableJob";
            operTableJob.Size = new Size(333, 33);
            operTableJob.TabIndex = 7;
            operTableJob.Text = "העברה לטבלה אופרטיבית";
            operTableJob.UseVisualStyleBackColor = true;
            operTableJob.Click += operTableJob_Click;
            // 
            // promotionsTempJob
            // 
            promotionsTempJob.Location = new Point(214, 349);
            promotionsTempJob.Name = "promotionsTempJob";
            promotionsTempJob.Size = new Size(333, 33);
            promotionsTempJob.TabIndex = 6;
            promotionsTempJob.Text = "ייבוא מבצעים";
            promotionsTempJob.UseVisualStyleBackColor = true;
            promotionsTempJob.Click += promotionsTempJob_Click;
            // 
            // catalogTempJob
            // 
            catalogTempJob.Location = new Point(214, 263);
            catalogTempJob.Name = "catalogTempJob";
            catalogTempJob.Size = new Size(333, 33);
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
            tempPullDateTime.Location = new Point(486, 207);
            tempPullDateTime.Name = "tempPullDateTime";
            tempPullDateTime.Size = new Size(235, 31);
            tempPullDateTime.TabIndex = 4;
            tempPullDateTime.ValueChanged += tempPullDateTime_ValueChanged;
            // 
            // branchCatalogCheckBox
            // 
            branchCatalogCheckBox.AutoSize = true;
            branchCatalogCheckBox.Location = new Point(551, 158);
            branchCatalogCheckBox.Name = "branchCatalogCheckBox";
            branchCatalogCheckBox.Size = new Size(170, 29);
            branchCatalogCheckBox.TabIndex = 3;
            branchCatalogCheckBox.Text = "משיכת ספר סניף";
            branchCatalogCheckBox.UseVisualStyleBackColor = true;
            branchCatalogCheckBox.CheckedChanged += branchCatalogCheckBox_CheckedChanged;
            // 
            // singleBranchCheckBox
            // 
            singleBranchCheckBox.AutoSize = true;
            singleBranchCheckBox.Location = new Point(531, 23);
            singleBranchCheckBox.Name = "singleBranchCheckBox";
            singleBranchCheckBox.Size = new Size(189, 29);
            singleBranchCheckBox.TabIndex = 2;
            singleBranchCheckBox.Text = "פעולה על סניף יחיד";
            singleBranchCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            singleBranchCheckBox.UseVisualStyleBackColor = true;
            singleBranchCheckBox.CheckedChanged += singleBranchCheckBox_CheckedChanged;
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
            // branchList
            // 
            branchList.FormattingEnabled = true;
            branchList.Location = new Point(329, 108);
            branchList.Name = "branchList";
            branchList.Size = new Size(393, 33);
            branchList.TabIndex = 0;
            // 
            // dataManagementTab
            // 
            dataManagementTab.Controls.Add(dataManagementTabControl);
            dataManagementTab.Location = new Point(4, 34);
            dataManagementTab.Name = "dataManagementTab";
            dataManagementTab.Padding = new Padding(3);
            dataManagementTab.Size = new Size(1281, 925);
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
            dataManagementTabControl.Location = new Point(3, 3);
            dataManagementTabControl.Name = "dataManagementTabControl";
            dataManagementTabControl.RightToLeft = RightToLeft.Yes;
            dataManagementTabControl.SelectedIndex = 0;
            dataManagementTabControl.Size = new Size(1275, 919);
            dataManagementTabControl.TabIndex = 0;
            // 
            // itemsTab
            // 
            itemsTab.Controls.Add(itemsDataGridView);
            itemsTab.Controls.Add(itemSearchPanel);
            itemsTab.Controls.Add(itemActionsPanel);
            itemsTab.Location = new Point(4, 34);
            itemsTab.Name = "itemsTab";
            itemsTab.Padding = new Padding(3);
            itemsTab.Size = new Size(1267, 881);
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
            itemsDataGridView.Location = new Point(6, 150);
            itemsDataGridView.Name = "itemsDataGridView";
            itemsDataGridView.ReadOnly = true;
            itemsDataGridView.RowHeadersWidth = 62;
            itemsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsDataGridView.Size = new Size(1255, 646);
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
            itemSearchPanel.Location = new Point(3, 3);
            itemSearchPanel.Name = "itemSearchPanel";
            itemSearchPanel.Size = new Size(1261, 143);
            itemSearchPanel.TabIndex = 1;
            // 
            // itemSearchBranchComboBox
            // 
            itemSearchBranchComboBox.FormattingEnabled = true;
            itemSearchBranchComboBox.Location = new Point(700, 20);
            itemSearchBranchComboBox.Name = "itemSearchBranchComboBox";
            itemSearchBranchComboBox.Size = new Size(200, 33);
            itemSearchBranchComboBox.TabIndex = 0;
            // 
            // itemSearchBranchLabel
            // 
            itemSearchBranchLabel.AutoSize = true;
            itemSearchBranchLabel.Location = new Point(910, 23);
            itemSearchBranchLabel.Name = "itemSearchBranchLabel";
            itemSearchBranchLabel.Size = new Size(47, 25);
            itemSearchBranchLabel.TabIndex = 1;
            itemSearchBranchLabel.Text = "סניף";
            // 
            // itemSearchBarcodeTextBox
            // 
            itemSearchBarcodeTextBox.Location = new Point(700, 60);
            itemSearchBarcodeTextBox.Name = "itemSearchBarcodeTextBox";
            itemSearchBarcodeTextBox.Size = new Size(200, 31);
            itemSearchBarcodeTextBox.TabIndex = 2;
            // 
            // itemSearchBarcodeLabel
            // 
            itemSearchBarcodeLabel.AutoSize = true;
            itemSearchBarcodeLabel.Location = new Point(910, 63);
            itemSearchBarcodeLabel.Name = "itemSearchBarcodeLabel";
            itemSearchBarcodeLabel.Size = new Size(58, 25);
            itemSearchBarcodeLabel.TabIndex = 3;
            itemSearchBarcodeLabel.Text = "ברקוד";
            // 
            // itemSearchNameTextBox
            // 
            itemSearchNameTextBox.Location = new Point(700, 100);
            itemSearchNameTextBox.Name = "itemSearchNameTextBox";
            itemSearchNameTextBox.Size = new Size(200, 31);
            itemSearchNameTextBox.TabIndex = 4;
            // 
            // itemSearchNameLabel
            // 
            itemSearchNameLabel.AutoSize = true;
            itemSearchNameLabel.Location = new Point(910, 103);
            itemSearchNameLabel.Name = "itemSearchNameLabel";
            itemSearchNameLabel.Size = new Size(38, 25);
            itemSearchNameLabel.TabIndex = 5;
            itemSearchNameLabel.Text = "שם";
            // 
            // itemSearchButton
            // 
            itemSearchButton.Location = new Point(600, 60);
            itemSearchButton.Name = "itemSearchButton";
            itemSearchButton.Size = new Size(94, 33);
            itemSearchButton.TabIndex = 6;
            itemSearchButton.Text = "חפש";
            itemSearchButton.UseVisualStyleBackColor = true;
            // 
            // itemSearchClearButton
            // 
            itemSearchClearButton.Location = new Point(500, 60);
            itemSearchClearButton.Name = "itemSearchClearButton";
            itemSearchClearButton.Size = new Size(94, 33);
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
            itemActionsPanel.Location = new Point(3, 820);
            itemActionsPanel.Name = "itemActionsPanel";
            itemActionsPanel.Size = new Size(1261, 58);
            itemActionsPanel.TabIndex = 2;
            // 
            // setItemNotSentButton
            // 
            setItemNotSentButton.Location = new Point(800, 13);
            setItemNotSentButton.Name = "setItemNotSentButton";
            setItemNotSentButton.Size = new Size(150, 33);
            setItemNotSentButton.TabIndex = 0;
            setItemNotSentButton.Text = "סמן כלא נשלח";
            setItemNotSentButton.UseVisualStyleBackColor = true;
            // 
            // removePromotionButton
            // 
            removePromotionButton.Location = new Point(650, 13);
            removePromotionButton.Name = "removePromotionButton";
            removePromotionButton.Size = new Size(150, 33);
            removePromotionButton.TabIndex = 1;
            removePromotionButton.Text = "הסר מבצע";
            removePromotionButton.UseVisualStyleBackColor = true;
            // 
            // viewItemDetailsButton
            // 
            viewItemDetailsButton.Location = new Point(500, 13);
            viewItemDetailsButton.Name = "viewItemDetailsButton";
            viewItemDetailsButton.Size = new Size(150, 33);
            viewItemDetailsButton.TabIndex = 2;
            viewItemDetailsButton.Text = "הצג פרטים";
            viewItemDetailsButton.UseVisualStyleBackColor = true;
            // 
            // promotionsTab
            // 
            promotionsTab.Controls.Add(promotionsDataGridView);
            promotionsTab.Controls.Add(promotionSearchPanel);
            promotionsTab.Controls.Add(promotionActionsPanel);
            promotionsTab.Location = new Point(4, 34);
            promotionsTab.Name = "promotionsTab";
            promotionsTab.Padding = new Padding(3);
            promotionsTab.Size = new Size(1267, 881);
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
            promotionsDataGridView.Location = new Point(6, 150);
            promotionsDataGridView.Name = "promotionsDataGridView";
            promotionsDataGridView.ReadOnly = true;
            promotionsDataGridView.RowHeadersWidth = 62;
            promotionsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            promotionsDataGridView.Size = new Size(1255, 646);
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
            promotionSearchPanel.Location = new Point(3, 3);
            promotionSearchPanel.Name = "promotionSearchPanel";
            promotionSearchPanel.Size = new Size(1261, 143);
            promotionSearchPanel.TabIndex = 1;
            // 
            // promotionSearchBranchComboBox
            // 
            promotionSearchBranchComboBox.FormattingEnabled = true;
            promotionSearchBranchComboBox.Location = new Point(700, 20);
            promotionSearchBranchComboBox.Name = "promotionSearchBranchComboBox";
            promotionSearchBranchComboBox.Size = new Size(200, 33);
            promotionSearchBranchComboBox.TabIndex = 0;
            // 
            // promotionSearchBranchLabel
            // 
            promotionSearchBranchLabel.AutoSize = true;
            promotionSearchBranchLabel.Location = new Point(910, 23);
            promotionSearchBranchLabel.Name = "promotionSearchBranchLabel";
            promotionSearchBranchLabel.Size = new Size(47, 25);
            promotionSearchBranchLabel.TabIndex = 1;
            promotionSearchBranchLabel.Text = "סניף";
            // 
            // promotionSearchKodRadioButton
            // 
            promotionSearchKodRadioButton.AutoSize = true;
            promotionSearchKodRadioButton.Location = new Point(800, 60);
            promotionSearchKodRadioButton.Name = "promotionSearchKodRadioButton";
            promotionSearchKodRadioButton.Size = new Size(112, 29);
            promotionSearchKodRadioButton.TabIndex = 2;
            promotionSearchKodRadioButton.Text = "קוד מבצע";
            promotionSearchKodRadioButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchItemKodRadioButton
            // 
            promotionSearchItemKodRadioButton.AutoSize = true;
            promotionSearchItemKodRadioButton.Checked = true;
            promotionSearchItemKodRadioButton.Location = new Point(800, 90);
            promotionSearchItemKodRadioButton.Name = "promotionSearchItemKodRadioButton";
            promotionSearchItemKodRadioButton.Size = new Size(106, 29);
            promotionSearchItemKodRadioButton.TabIndex = 3;
            promotionSearchItemKodRadioButton.TabStop = true;
            promotionSearchItemKodRadioButton.Text = "קוד פריט";
            promotionSearchItemKodRadioButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchValueTextBox
            // 
            promotionSearchValueTextBox.Location = new Point(600, 75);
            promotionSearchValueTextBox.Name = "promotionSearchValueTextBox";
            promotionSearchValueTextBox.Size = new Size(150, 31);
            promotionSearchValueTextBox.TabIndex = 4;
            // 
            // promotionSearchValueLabel
            // 
            promotionSearchValueLabel.AutoSize = true;
            promotionSearchValueLabel.Location = new Point(760, 78);
            promotionSearchValueLabel.Name = "promotionSearchValueLabel";
            promotionSearchValueLabel.Size = new Size(43, 25);
            promotionSearchValueLabel.TabIndex = 5;
            promotionSearchValueLabel.Text = "ערך";
            // 
            // promotionSearchButton
            // 
            promotionSearchButton.Location = new Point(500, 75);
            promotionSearchButton.Name = "promotionSearchButton";
            promotionSearchButton.Size = new Size(94, 33);
            promotionSearchButton.TabIndex = 6;
            promotionSearchButton.Text = "חפש";
            promotionSearchButton.UseVisualStyleBackColor = true;
            // 
            // promotionSearchClearButton
            // 
            promotionSearchClearButton.Location = new Point(400, 75);
            promotionSearchClearButton.Name = "promotionSearchClearButton";
            promotionSearchClearButton.Size = new Size(94, 33);
            promotionSearchClearButton.TabIndex = 7;
            promotionSearchClearButton.Text = "נקה";
            promotionSearchClearButton.UseVisualStyleBackColor = true;
            // 
            // promotionActionsPanel
            // 
            promotionActionsPanel.Controls.Add(setPromotionNotTransferredButton);
            promotionActionsPanel.Controls.Add(viewPromotionDetailsButton);
            promotionActionsPanel.Dock = DockStyle.Bottom;
            promotionActionsPanel.Location = new Point(3, 820);
            promotionActionsPanel.Name = "promotionActionsPanel";
            promotionActionsPanel.Size = new Size(1261, 58);
            promotionActionsPanel.TabIndex = 2;
            // 
            // setPromotionNotTransferredButton
            // 
            setPromotionNotTransferredButton.Location = new Point(800, 13);
            setPromotionNotTransferredButton.Name = "setPromotionNotTransferredButton";
            setPromotionNotTransferredButton.Size = new Size(150, 33);
            setPromotionNotTransferredButton.TabIndex = 0;
            setPromotionNotTransferredButton.Text = "סמן כלא הועבר";
            setPromotionNotTransferredButton.UseVisualStyleBackColor = true;
            // 
            // viewPromotionDetailsButton
            // 
            viewPromotionDetailsButton.Location = new Point(650, 13);
            viewPromotionDetailsButton.Name = "viewPromotionDetailsButton";
            viewPromotionDetailsButton.Size = new Size(150, 33);
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
            itemsApiTab.Location = new Point(4, 34);
            itemsApiTab.Name = "itemsApiTab";
            itemsApiTab.Padding = new Padding(3);
            itemsApiTab.Size = new Size(1267, 881);
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
            itemsApiPanel.Location = new Point(3, 3);
            itemsApiPanel.Name = "itemsApiPanel";
            itemsApiPanel.Size = new Size(1261, 100);
            itemsApiPanel.TabIndex = 0;
            // 
            // itemsApiBranchComboBox
            // 
            itemsApiBranchComboBox.FormattingEnabled = true;
            itemsApiBranchComboBox.Location = new Point(500, 10);
            itemsApiBranchComboBox.Name = "itemsApiBranchComboBox";
            itemsApiBranchComboBox.Size = new Size(200, 33);
            itemsApiBranchComboBox.TabIndex = 0;
            // 
            // itemsApiBranchLabel
            // 
            itemsApiBranchLabel.AutoSize = true;
            itemsApiBranchLabel.Location = new Point(739, 15);
            itemsApiBranchLabel.Name = "itemsApiBranchLabel";
            itemsApiBranchLabel.Size = new Size(47, 25);
            itemsApiBranchLabel.TabIndex = 1;
            itemsApiBranchLabel.Text = "סניף";
            // 
            // itemsApiItemIdTextBox
            // 
            itemsApiItemIdTextBox.Location = new Point(300, 55);
            itemsApiItemIdTextBox.Name = "itemsApiItemIdTextBox";
            itemsApiItemIdTextBox.Size = new Size(400, 31);
            itemsApiItemIdTextBox.TabIndex = 2;
            // 
            // itemsApiItemIdLabel
            // 
            itemsApiItemIdLabel.AutoSize = true;
            itemsApiItemIdLabel.Location = new Point(710, 63);
            itemsApiItemIdLabel.Name = "itemsApiItemIdLabel";
            itemsApiItemIdLabel.Size = new Size(252, 25);
            itemsApiItemIdLabel.TabIndex = 3;
            itemsApiItemIdLabel.Text = "ברקוד פריט (מופרדים בפסיקים)";
            // 
            // itemsApiGetItemButton
            // 
            itemsApiGetItemButton.Location = new Point(170, 53);
            itemsApiGetItemButton.Name = "itemsApiGetItemButton";
            itemsApiGetItemButton.Size = new Size(107, 43);
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
            itemsApiResultsDataGridView.Location = new Point(6, 110);
            itemsApiResultsDataGridView.Name = "itemsApiResultsDataGridView";
            itemsApiResultsDataGridView.ReadOnly = true;
            itemsApiResultsDataGridView.RowHeadersWidth = 62;
            itemsApiResultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsApiResultsDataGridView.Size = new Size(1255, 686);
            itemsApiResultsDataGridView.TabIndex = 1;
            // 
            // itemsApiAddSelectedButton
            // 
            itemsApiAddSelectedButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            itemsApiAddSelectedButton.Location = new Point(1090, 802);
            itemsApiAddSelectedButton.Name = "itemsApiAddSelectedButton";
            itemsApiAddSelectedButton.Size = new Size(150, 33);
            itemsApiAddSelectedButton.TabIndex = 2;
            itemsApiAddSelectedButton.Text = "הוסף נבחרים";
            itemsApiAddSelectedButton.UseVisualStyleBackColor = true;
            // 
            // itemsApiAddAllButton
            // 
            itemsApiAddAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            itemsApiAddAllButton.Location = new Point(940, 802);
            itemsApiAddAllButton.Name = "itemsApiAddAllButton";
            itemsApiAddAllButton.Size = new Size(150, 33);
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
            promotionsApiTab.Location = new Point(4, 34);
            promotionsApiTab.Name = "promotionsApiTab";
            promotionsApiTab.Padding = new Padding(3);
            promotionsApiTab.Size = new Size(1267, 881);
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
            promotionsApiPanel.Location = new Point(3, 3);
            promotionsApiPanel.Name = "promotionsApiPanel";
            promotionsApiPanel.Size = new Size(1261, 100);
            promotionsApiPanel.TabIndex = 0;
            // 
            // promotionsApiBranchComboBox
            // 
            promotionsApiBranchComboBox.FormattingEnabled = true;
            promotionsApiBranchComboBox.Location = new Point(500, 10);
            promotionsApiBranchComboBox.Name = "promotionsApiBranchComboBox";
            promotionsApiBranchComboBox.Size = new Size(200, 33);
            promotionsApiBranchComboBox.TabIndex = 0;
            // 
            // promotionsApiBranchLabel
            // 
            promotionsApiBranchLabel.AutoSize = true;
            promotionsApiBranchLabel.Location = new Point(739, 15);
            promotionsApiBranchLabel.Name = "promotionsApiBranchLabel";
            promotionsApiBranchLabel.Size = new Size(47, 25);
            promotionsApiBranchLabel.TabIndex = 1;
            promotionsApiBranchLabel.Text = "סניף";
            // 
            // promotionsApiBarcodeFilterTextBox
            // 
            promotionsApiBarcodeFilterTextBox.Location = new Point(300, 50);
            promotionsApiBarcodeFilterTextBox.Name = "promotionsApiBarcodeFilterTextBox";
            promotionsApiBarcodeFilterTextBox.Size = new Size(400, 31);
            promotionsApiBarcodeFilterTextBox.TabIndex = 2;
            // 
            // promotionsApiBarcodeFilterLabel
            // 
            promotionsApiBarcodeFilterLabel.AutoSize = true;
            promotionsApiBarcodeFilterLabel.Location = new Point(707, 55);
            promotionsApiBarcodeFilterLabel.Name = "promotionsApiBarcodeFilterLabel";
            promotionsApiBarcodeFilterLabel.Size = new Size(128, 25);
            promotionsApiBarcodeFilterLabel.TabIndex = 3;
            promotionsApiBarcodeFilterLabel.Text = "סינון לפי ברקוד";
            // 
            // promotionsApiGetPromotionsButton
            // 
            promotionsApiGetPromotionsButton.Location = new Point(209, 43);
            promotionsApiGetPromotionsButton.Name = "promotionsApiGetPromotionsButton";
            promotionsApiGetPromotionsButton.Size = new Size(90, 48);
            promotionsApiGetPromotionsButton.TabIndex = 4;
            promotionsApiGetPromotionsButton.Text = "משוך מבצעים";
            promotionsApiGetPromotionsButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiApplyFilterButton
            // 
            promotionsApiApplyFilterButton.Location = new Point(116, 45);
            promotionsApiApplyFilterButton.Name = "promotionsApiApplyFilterButton";
            promotionsApiApplyFilterButton.Size = new Size(87, 43);
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
            promotionsApiResultsDataGridView.Location = new Point(6, 110);
            promotionsApiResultsDataGridView.Name = "promotionsApiResultsDataGridView";
            promotionsApiResultsDataGridView.ReadOnly = true;
            promotionsApiResultsDataGridView.RowHeadersWidth = 62;
            promotionsApiResultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            promotionsApiResultsDataGridView.Size = new Size(1255, 686);
            promotionsApiResultsDataGridView.TabIndex = 1;
            // 
            // promotionsApiAddSelectedButton
            // 
            promotionsApiAddSelectedButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            promotionsApiAddSelectedButton.Location = new Point(1090, 802);
            promotionsApiAddSelectedButton.Name = "promotionsApiAddSelectedButton";
            promotionsApiAddSelectedButton.Size = new Size(150, 33);
            promotionsApiAddSelectedButton.TabIndex = 2;
            promotionsApiAddSelectedButton.Text = "הוסף נבחרים";
            promotionsApiAddSelectedButton.UseVisualStyleBackColor = true;
            // 
            // promotionsApiAddAllButton
            // 
            promotionsApiAddAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            promotionsApiAddAllButton.Location = new Point(940, 802);
            promotionsApiAddAllButton.Name = "promotionsApiAddAllButton";
            promotionsApiAddAllButton.Size = new Size(150, 33);
            promotionsApiAddAllButton.TabIndex = 3;
            promotionsApiAddAllButton.Text = "הוסף הכל";
            promotionsApiAddAllButton.UseVisualStyleBackColor = true;
            // 
            // loadingSpinner
            // 
            loadingSpinner.Location = new Point(0, 0);
            loadingSpinner.Margin = new Padding(4, 5, 4, 5);
            loadingSpinner.Name = "loadingSpinner";
            loadingSpinner.Size = new Size(143, 83);
            loadingSpinner.TabIndex = 0;
            loadingSpinner.TabStop = false;
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
            comaxApiBranchComboBox.Size = new Size(121, 33);
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
            comaxApiItemsBarcodesTextBox.Size = new Size(100, 31);
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
            comaxApiPromotionsBarcodesTextBox.Size = new Size(100, 31);
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
            comaxApiResultsDataGridView.ColumnHeadersHeight = 34;
            comaxApiResultsDataGridView.Location = new Point(0, 0);
            comaxApiResultsDataGridView.Name = "comaxApiResultsDataGridView";
            comaxApiResultsDataGridView.RowHeadersWidth = 62;
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
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1289, 963);
            Controls.Add(loadingSpinner);
            Controls.Add(mainTabControl);
            Name = "Form1";
            RightToLeft = RightToLeft.Yes;
            Text = "ניהול נתונים";
            mainTabControl.ResumeLayout(false);
            operationsTab.ResumeLayout(false);
            operationsTab.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)loadingSpinner).EndInit();
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
        private Button catalogNewTempJob;
    }
}
