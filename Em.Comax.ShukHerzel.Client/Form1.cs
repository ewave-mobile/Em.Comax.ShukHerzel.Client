using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Bl.services;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace Em.Comax.ShukHerzel.Client
{
    public partial class Form1 : Form
    {
        private readonly IBranchService _branchService;
        private readonly IAllItemsService _allItemsService;
        private readonly IAllItemsNewService _allItemsNewService;
        private List<Branch> _branches = null;
        private readonly IPromotionsService _promotionsService;
        private readonly IOperativeService _operativeService;
        private readonly IApiClientService _apiClientService;
        private readonly IPriceUpdateService _priceUpdateService;
        private readonly IPromotionsRepository _promotionsRepository;
        private readonly IComaxApiClient _comaxApiClient;
        private readonly IDatabaseLogger _databaseLogger;
        private System.Windows.Forms.Timer _spinnerTimer;
        private int _spinnerAngle = 0;
        //private const long SHUK_HERZEL_COMPANY_ID = 1;

        public Form1(
            IBranchService branchService,
            IAllItemsService allItemsService,
            IAllItemsNewService allItemsNewService,
            IPromotionsService promotionsService,
            IOperativeService operativeService,
            IApiClientService apiClientService,
            IPriceUpdateService priceUpdateService,
            IPromotionsRepository promotionsRepository,
            IComaxApiClient comaxApiClient,
            IDatabaseLogger databaseLogger)
        {
            _branchService = branchService;
            _allItemsService = allItemsService;
            _allItemsNewService = allItemsNewService;
            _promotionsService = promotionsService;
            _operativeService = operativeService;
            _apiClientService = apiClientService;
            _promotionsRepository = promotionsRepository;
            _comaxApiClient = comaxApiClient;
            _databaseLogger = databaseLogger;
            InitializeComponent();
            _ = initForm();
            _priceUpdateService = priceUpdateService;
        }
        private async Task<bool> initForm()
        {
            // Initialize the loading spinner
            loadingSpinner.Size = new Size(100, 100);
            loadingSpinner.Visible = false;
            loadingSpinner.Location = new Point(
                (this.ClientSize.Width - loadingSpinner.Width) / 2,
                (this.ClientSize.Height - loadingSpinner.Height) / 2);
            loadingSpinner.BackColor = Color.Transparent;
            loadingSpinner.BorderStyle = BorderStyle.None;

            // Set up the spinner animation
            _spinnerTimer = new System.Windows.Forms.Timer();
            _spinnerTimer.Interval = 50; // 50ms for smooth animation
            _spinnerTimer.Tick += SpinnerTimer_Tick;

            // Set up the spinner to paint itself
            loadingSpinner.Paint += LoadingSpinner_Paint;

            // We'll bring it to front when needed in ShowLoadingSpinner

            branchList.Enabled = false;
            tempPullDateTime.Enabled = true;
            //Remove 17-09-2025
            //  _branches = await _branchService.GetAllBranchesByCompany(Constants.SHUK_HERZEL_COMPANY_ID);
            _branches = await _branchService.GetAllBranches();
            branchList.DataSource = _branches;
            branchList.DisplayMember = "BranchName";
            branchList.ValueMember = "Id";

            // Initialize branch combo boxes in data management tabs
            // Create separate lists for each combo box to avoid binding issues
            itemSearchBranchComboBox.DataSource = new List<Branch>(_branches);
            itemSearchBranchComboBox.DisplayMember = "BranchName";
            itemSearchBranchComboBox.ValueMember = "Id";

            promotionSearchBranchComboBox.DataSource = new List<Branch>(_branches);
            promotionSearchBranchComboBox.DisplayMember = "BranchName";
            promotionSearchBranchComboBox.ValueMember = "Id";

            // comaxApiBranchComboBox removed

            // Initialize branch combo boxes for the new API tabs
            itemsApiBranchComboBox.DataSource = new List<Branch>(_branches);
            itemsApiBranchComboBox.DisplayMember = "BranchName";
            itemsApiBranchComboBox.ValueMember = "Id";

            promotionsApiBranchComboBox.DataSource = new List<Branch>(_branches);
            promotionsApiBranchComboBox.DisplayMember = "BranchName";
            promotionsApiBranchComboBox.ValueMember = "Id";

            // Set up event handlers for the new UI controls
            itemSearchButton.Click += ItemSearchButton_Click;
            itemSearchClearButton.Click += ItemSearchClearButton_Click;
            setItemNotSentButton.Click += SetItemNotSentButton_Click;
            removePromotionButton.Click += RemovePromotionButton_Click;
            viewItemDetailsButton.Click += ViewItemDetailsButton_Click;

            promotionSearchButton.Click += PromotionSearchButton_Click;
            promotionSearchClearButton.Click += PromotionSearchClearButton_Click;
            setPromotionNotTransferredButton.Click += SetPromotionNotTransferredButton_Click;
            viewPromotionDetailsButton.Click += ViewPromotionDetailsButton_Click;

            // Remove event handlers for the old comaxApiTab controls

            // Set up event handlers for the new API tabs
            itemsApiGetItemButton.Click += ItemsApiGetItemButton_Click;
            itemsApiAddSelectedButton.Click += ItemsApiAddSelectedButton_Click;
            itemsApiAddAllButton.Click += ItemsApiAddAllButton_Click;

            promotionsApiGetPromotionsButton.Click += PromotionsApiGetPromotionsButton_Click;
            promotionsApiApplyFilterButton.Click += PromotionsApiApplyFilterButton_Click;
            promotionsApiAddSelectedButton.Click += PromotionsApiAddSelectedButton_Click;
            promotionsApiAddAllButton.Click += PromotionsApiAddAllButton_Click;
            itemsApiResultsDataGridView.CellDoubleClick += ItemsApiResultsDataGridView_CellDoubleClick;

            // Set up DataGridView columns for items
            itemsDataGridView.AutoGenerateColumns = false;
            itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "מזהה",
                Width = 70
            });
            itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Barcode",
                HeaderText = "ברקוד",
                Width = 120
            });
            itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "שם",
                Width = 200
            });
            itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "מחיר",
                Width = 80
            });
            itemsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsSentToEsl",
                HeaderText = "נשלח ל-ESL",
                Width = 80
            });
            itemsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsPromotion",
                HeaderText = "מבצע",
                Width = 80
            });

            // Set up DataGridView columns for itemsApiResultsDataGridView
            itemsApiResultsDataGridView.AutoGenerateColumns = false;
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "מזהה",
                Width = 70
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Barcode",
                HeaderText = "ברקוד",
                Width = 120
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "שם",
                Width = 200
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "מחיר",
                Width = 80
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Department",
                HeaderText = "מחלקה",
                Width = 120
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GroupName",
                HeaderText = "קבוצה",
                Width = 120
            });
            itemsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SubGroup",
                HeaderText = "תת-קבוצה",
                Width = 120
            });

            // Set up DataGridView columns for promotions
            promotionsDataGridView.AutoGenerateColumns = false;
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "מזהה",
                Width = 70
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Kod",
                HeaderText = "קוד מבצע",
                Width = 120
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemKod",
                HeaderText = "קוד פריט",
                Width = 120
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nm",
                HeaderText = "שם",
                Width = 200
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FromDate",
                HeaderText = "מתאריך",
                Width = 100
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ToDate",
                HeaderText = "עד תאריך",
                Width = 100
            });
            promotionsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "מחיר",
                Width = 80
            });
            promotionsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsTransferredToOper",
                HeaderText = "הועבר לאופרטיבי",
                Width = 120
            });
            promotionsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "SwNotForShelfSignage",
                HeaderText = "האם להצגת שילוט דיגיטלי",
                Width = 80
            });

            // Set up DataGridView columns for promotionsApiResultsDataGridView
            promotionsApiResultsDataGridView.AutoGenerateColumns = false;
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "מזהה",
                Width = 70
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Kod",
                HeaderText = "קוד מבצע",
                Width = 120
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemKod",
                HeaderText = "קוד פריט",
                Width = 120
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nm",
                HeaderText = "שם",
                Width = 200
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FromDate",
                HeaderText = "מתאריך",
                Width = 100
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ToDate",
                HeaderText = "עד תאריך",
                Width = 100
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "מחיר",
                Width = 80
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nature",
                HeaderText = "סוג",
                Width = 100
            });
            promotionsApiResultsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SwAllCustomers",
                HeaderText = "לכל הלקוחות",
                Width = 100
            });

            return true;
        }
        private void tempPullDateTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private async void catalogTempJob_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            catalogTempJob.Enabled = false;
            //year ago date
            DateTime yearAgo = DateTime.Now.AddYears(-1);
            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            try
            {
                if (singleBranchCheckBox.Checked)
                {
                    if (branchCatalogCheckBox.Checked)
                    {
                        await _allItemsService.InsertCatalogAsync((Branch)branchList.SelectedItem, yearAgo, progress);
                    }
                    else
                    {
                        await _allItemsService.InsertCatalogAsync((Branch)branchList.SelectedItem, tempPullDateTime.Value, progress);
                    }

                }
                else
                {
                    foreach (var branch in _branches)
                    {
                        if (branchCatalogCheckBox.Checked)
                        {
                            await _allItemsService.InsertCatalogAsync(branch, yearAgo, progress);
                        }
                        else
                        {
                            await _allItemsService.InsertCatalogAsync(branch, tempPullDateTime.Value, progress);
                        }
                    }
                    // await _allItemsService.InsertCatalogAsync(null, tempPullDateTime.Value, progress);
                }
                //await _allItemsService.InsertCatalogAsync((Branch)branchList.SelectedItem, tempPullDateTime.Value, progress);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText(ex.Message);
            }
            finally
            {
                catalogTempJob.Enabled = true;
            }
        }
        private void singleBranchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (singleBranchCheckBox.Checked)
            {
                branchList.Enabled = true;
            }
            else
            {
                branchList.Enabled = false;
            }
        }

        private void branchCatalogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (branchCatalogCheckBox.Checked)
            {
                tempPullDateTime.Enabled = false;
            }
            else
            {
                tempPullDateTime.Enabled = true;
            }
        }

        private async void promotionsTempJob_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            promotionsTempJob.Enabled = false;
            //year ago date
            DateTime yearAgo = DateTime.Now.AddYears(-1);
            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            try
            {
                if (singleBranchCheckBox.Checked)
                {
                    // Insert promotions for just the selected branch
                    var branch = (Branch)branchList.SelectedItem;
                    if (branchCatalogCheckBox.Checked)
                    {
                        await _promotionsService.InsertPromotionsAsync(branch, yearAgo, progress);
                    }
                    else
                    {
                        await _promotionsService.InsertPromotionsAsync(branch, tempPullDateTime.Value, progress);
                    }
                }
                else
                {
                    // Insert promotions for all branches in _branches
                    foreach (var branch in _branches)
                    {
                        if (branchCatalogCheckBox.Checked)
                        {
                            await _promotionsService.InsertPromotionsAsync(branch, yearAgo, progress);
                        }
                        else
                        {
                            await _promotionsService.InsertPromotionsAsync(branch, tempPullDateTime.Value, progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logTextBox.AppendText($"Error: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                promotionsTempJob.Enabled = true;
            }
        }

        private async void operTableJob_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            operTableJob.Enabled = false;

            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            try
            {
                await _operativeService.SyncAllItemsAndPromotionsAsync(progress, CancellationToken.None);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText($"Error: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                operTableJob.Enabled = true;
            }
        }

        private async void operTableJobNew_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            operTableJob.Enabled = false;

            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            try
            {
                await _operativeService.SyncAllItemsNewAndPromotionsAsync(progress, CancellationToken.None);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText($"Error: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                operTableJob.Enabled = true;
            }
        }

        private async void eslTransferJob_Click(object sender, EventArgs e)
        {
            // Clear previous logs
            logTextBox.Clear();

            // Disable the button to prevent multiple clicks
            eslTransferJob.Enabled = false;

            // Initialize a Progress<string> instance to handle reporting
            var progress = new Progress<string>(message =>
            {
                // Append the progress message to the logTextBox
                logTextBox.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            });

            // Optionally, create a CancellationTokenSource if you want to support cancellation
            using var cts = new CancellationTokenSource();

            try
            {
                // Log the start of the synchronization
                await _apiClientService.SendItemsToEslAsync(true, progress, cts.Token);

                // Notify the user of completion
                // progress.Report("ESL Synchronization completed successfully.");
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                // progress.Report("ESL Synchronization was canceled.");
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                // progress.Report($"Error during ESL Synchronization: {ex.Message}");
                logTextBox.AppendText($"Exception: {ex}{Environment.NewLine}");
            }
            finally
            {
                // Re-enable the button regardless of the outcome
                eslTransferJob.Enabled = true;
            }
        }

        private async void PriceUpdatesButton_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();

            PriceUpdatesButton.Enabled = false;
            DateTime yearAgo = DateTime.Now.AddYears(-1);
            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            using var cts = new CancellationTokenSource();
            try
            {
                if (singleBranchCheckBox.Checked)
                {
                    // Insert promotions for just the selected branch
                    var branch = (Branch)branchList.SelectedItem;
                    if (branchCatalogCheckBox.Checked)
                    {
                        await _priceUpdateService.InsertPriceUpdatesAsync(branch, yearAgo, progress);
                    }
                    else
                    {
                        await _priceUpdateService.InsertPriceUpdatesAsync(branch, tempPullDateTime.Value, progress);
                    }
                }
                else
                {
                    // Insert promotions for all branches in _branches
                    foreach (var branch in _branches)
                    {
                        if (branchCatalogCheckBox.Checked)
                        {
                            await _priceUpdateService.InsertPriceUpdatesAsync(branch, yearAgo, progress);
                        }
                        else
                        {
                            await _priceUpdateService.InsertPriceUpdatesAsync(branch, tempPullDateTime.Value, progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logTextBox.AppendText($"Error: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                PriceUpdatesButton.Enabled = true;
            }
        }

        #region Items Tab Event Handlers

        private async void ItemSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                itemSearchButton.Enabled = false;

                // Get search parameters
                var barcode = string.IsNullOrWhiteSpace(itemSearchBarcodeTextBox.Text) ? null : itemSearchBarcodeTextBox.Text.Trim();
                var name = string.IsNullOrWhiteSpace(itemSearchNameTextBox.Text) ? null : itemSearchNameTextBox.Text.Trim();

                // Validate input - require either barcode or name with minimum 4 characters
                if ((string.IsNullOrEmpty(barcode) || barcode.Length < 4) &&
                    (string.IsNullOrEmpty(name) || name.Length < 4))
                {
                    MessageBox.Show("Please enter either a barcode or name with at least 4 characters.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get branch ID (optional)
                long? branchId = null;
                if (itemSearchBranchComboBox.SelectedItem != null)
                {
                    var branch = (Branch)itemSearchBranchComboBox.SelectedItem;
                    branchId = branch.Id;
                }

                // Search for items
                var items = await _operativeService.SearchItemsAsync(barcode, branchId, name);

                // Display results
                itemsDataGridView.DataSource = items;

                // Enable/disable action buttons based on search results
                bool hasItems = items != null && items.Count > 0;
                setItemNotSentButton.Enabled = hasItems;
                removePromotionButton.Enabled = hasItems;
                viewItemDetailsButton.Enabled = hasItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                itemSearchButton.Enabled = true;
            }
        }

        private void ItemSearchClearButton_Click(object sender, EventArgs e)
        {
            // Clear search fields
            itemSearchBarcodeTextBox.Text = string.Empty;
            itemSearchNameTextBox.Text = string.Empty;

            // Clear results
            itemsDataGridView.DataSource = null;

            // Disable action buttons
            setItemNotSentButton.Enabled = false;
            removePromotionButton.Enabled = false;
            viewItemDetailsButton.Enabled = false;
        }

        private async void SetItemNotSentButton_Click(object sender, EventArgs e)
        {
            if (itemsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                setItemNotSentButton.Enabled = false;

                // Get the selected item
                var selectedItem = (Item)itemsDataGridView.SelectedRows[0].DataBoundItem;

                // Set the item as not sent
                await _operativeService.SetItemNotSentAsync(selectedItem.Id);

                // Refresh the item in the grid
                selectedItem.IsSentToEsl = null;
                itemsDataGridView.Refresh();

                MessageBox.Show("Item marked as not sent to ESL.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting item as not sent: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                setItemNotSentButton.Enabled = true;
            }
        }

        private async void RemovePromotionButton_Click(object sender, EventArgs e)
        {
            if (itemsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                removePromotionButton.Enabled = false;

                // Get the selected item
                var selectedItem = (Item)itemsDataGridView.SelectedRows[0].DataBoundItem;

                // Remove the promotion
                await _operativeService.RemovePromotionAsync(selectedItem.Id);

                // Refresh the item in the grid
                selectedItem.IsPromotion = null;
                selectedItem.PromotionKod = null;
                selectedItem.PromotionFromDate = null;
                selectedItem.PromotionToDate = null;
                selectedItem.TotalPromotionPrice = null;
                selectedItem.SwAllCustomers = null;
                selectedItem.TextForWeb = null;
                selectedItem.Quantity = null;
                selectedItem.PromotionBarcodes = null;
                selectedItem.IsSentToEsl = null;
                itemsDataGridView.Refresh();

                MessageBox.Show("Promotion removed from item.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing promotion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                removePromotionButton.Enabled = true;
            }
        }

        void ViewItemDetailsButton_Click(object sender, EventArgs e)
        {
            if (itemsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the selected item
            var selectedItem = (Item)itemsDataGridView.SelectedRows[0].DataBoundItem;

            // Display item details
            var details = new StringBuilder();
            details.AppendLine($"ID: {selectedItem.Id}");
            details.AppendLine($"Barcode: {selectedItem.Barcode}");
            details.AppendLine($"Name: {selectedItem.Name}");
            details.AppendLine($"Price: {selectedItem.Price}");
            details.AppendLine($"Sent to ESL: {selectedItem.IsSentToEsl}");
            details.AppendLine($"Is Promotion: {selectedItem.IsPromotion ?? false}"); // Explicitly handle nullable bool

            if (selectedItem.IsPromotion == true) // Explicitly check for true
            {
                details.AppendLine($"Promotion Code: {selectedItem.PromotionKod}");
                details.AppendLine($"Promotion From Date: {selectedItem.PromotionFromDate}");
                details.AppendLine($"Promotion To Date: {selectedItem.PromotionToDate}");
                details.AppendLine($"Promotion Price: {selectedItem.TotalPromotionPrice}");
                details.AppendLine($"All Customers: {selectedItem.SwAllCustomers}");
                details.AppendLine($"Web Text: {selectedItem.TextForWeb}");
                details.AppendLine($"Quantity: {selectedItem.Quantity}");
                details.AppendLine($"Promotion Barcodes: {selectedItem.PromotionBarcodes}");
            }

            MessageBox.Show(details.ToString(), "Item Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Promotions Tab Event Handlers

        private async void PromotionSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                promotionSearchButton.Enabled = false;

                // Get search parameters
                var searchValue = promotionSearchValueTextBox.Text.Trim();

                // Validate input - require search value with minimum 4 characters
                if (string.IsNullOrEmpty(searchValue) || searchValue.Length < 4)
                {
                    MessageBox.Show("Please enter a search value with at least 4 characters.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string kod = null;
                string itemKod = null;

                if (promotionSearchKodRadioButton.Checked)
                {
                    kod = searchValue;
                }
                else
                {
                    itemKod = searchValue;
                }

                // Get branch ID (optional)
                long? branchId = null;
                if (promotionSearchBranchComboBox.SelectedItem != null)
                {
                    var branch = (Branch)promotionSearchBranchComboBox.SelectedItem;
                    branchId = branch.Id;
                }

                // Search for promotions
                var promotions = await _promotionsService.SearchPromotionsAsync(kod, itemKod, branchId);

                // Display results
                promotionsDataGridView.DataSource = promotions;

                // Enable/disable action buttons based on search results
                bool hasPromotions = promotions != null && promotions.Count > 0;
                setPromotionNotTransferredButton.Enabled = hasPromotions;
                viewPromotionDetailsButton.Enabled = hasPromotions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for promotions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                promotionSearchButton.Enabled = true;
            }
        }

        private void PromotionSearchClearButton_Click(object sender, EventArgs e)
        {
            // Clear search fields
            promotionSearchValueTextBox.Text = string.Empty;

            // Clear results
            promotionsDataGridView.DataSource = null;

            // Disable action buttons
            setPromotionNotTransferredButton.Enabled = false;
            viewPromotionDetailsButton.Enabled = false;
        }

        private async void SetPromotionNotTransferredButton_Click(object sender, EventArgs e)
        {
            if (promotionsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a promotion first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                setPromotionNotTransferredButton.Enabled = false;

                // Get the selected promotion
                var selectedPromotion = (Promotion)promotionsDataGridView.SelectedRows[0].DataBoundItem;

                // Set the promotion as not transferred
                await _promotionsService.SetPromotionNotTransferredAsync(selectedPromotion.Id);

                // Refresh the promotion in the grid
                selectedPromotion.IsTransferredToOper = false;
                selectedPromotion.TransferredDateTime = null;
                promotionsDataGridView.Refresh();

                MessageBox.Show("Promotion marked as not transferred to operative table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting promotion as not transferred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                setPromotionNotTransferredButton.Enabled = true;
            }
        }

        private void ViewPromotionDetailsButton_Click(object sender, EventArgs e)
        {
            if (promotionsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a promotion first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the selected promotion
            var selectedPromotion = (Promotion)promotionsDataGridView.SelectedRows[0].DataBoundItem;

            // Display promotion details
            var details = new StringBuilder();
            details.AppendLine($"ID: {selectedPromotion.Id}");
            details.AppendLine($"Kod: {selectedPromotion.Kod}");
            details.AppendLine($"Item Kod: {selectedPromotion.ItemKod}");
            details.AppendLine($"Name: {selectedPromotion.Nm}");
            details.AppendLine($"From Date: {selectedPromotion.FromDate}");
            details.AppendLine($"To Date: {selectedPromotion.ToDate}");
            details.AppendLine($"Total (Price): {selectedPromotion.Total}");
            details.AppendLine($"Active: {selectedPromotion.SwActive}");
            details.AppendLine($"Transferred to Oper: {selectedPromotion.IsTransferredToOper}");
            details.AppendLine($"Transferred Date: {selectedPromotion.TransferredDateTime}");

            MessageBox.Show(details.ToString(), "Promotion Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        // For the new API tabs
        private List<AllItem> _itemsApiResults = new List<AllItem>();
        private List<Promotion> _promotionsApiResults = new List<Promotion>();
        private List<Promotion> _filteredPromotionsApiResults = new List<Promotion>();

        #region Loading Spinner Methods

        private void LoadingSpinner_Paint(object sender, PaintEventArgs e)
        {
            // Get the graphics object
            Graphics g = e.Graphics;

            // Set up for high quality drawing
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate the center of the control
            int centerX = loadingSpinner.Width / 2;
            int centerY = loadingSpinner.Height / 2;

            // Calculate the radius of the spinner (80% of the control size)
            int radius = (int)(Math.Min(loadingSpinner.Width, loadingSpinner.Height) * 0.4);

            // Draw the spinner spokes
            for (int i = 0; i < 12; i++)
            {
                // Calculate the angle for this spoke (in radians)
                double angle = (i * 30 + _spinnerAngle) * Math.PI / 180;

                // Calculate the start and end points for this spoke
                int startX = centerX + (int)(radius * 0.7 * Math.Cos(angle));
                int startY = centerY + (int)(radius * 0.7 * Math.Sin(angle));
                int endX = centerX + (int)(radius * Math.Cos(angle));
                int endY = centerY + (int)(radius * Math.Sin(angle));

                // Calculate the alpha value for this spoke (fades based on position)
                int alpha = 255 - ((i * 255) / 12);

                // Create a pen with the appropriate color and thickness
                using (Pen pen = new Pen(Color.FromArgb(alpha, 0, 0, 0), 3))
                {
                    // Draw the spoke
                    g.DrawLine(pen, startX, startY, endX, endY);
                }
            }
        }

        private void SpinnerTimer_Tick(object sender, EventArgs e)
        {
            // Increment the angle
            _spinnerAngle = (_spinnerAngle + 30) % 360;

            // Force the spinner to redraw
            loadingSpinner.Invalidate();
        }

        private void ShowLoadingSpinner()
        {
            // Ensure the spinner is properly positioned in the center of the form
            loadingSpinner.Location = new Point(
                (this.ClientSize.Width - loadingSpinner.Width) / 2,
                (this.ClientSize.Height - loadingSpinner.Height) / 2);

            // Make sure it's on top of all other controls
            loadingSpinner.Parent = this; // Set parent to the form
            loadingSpinner.BringToFront();
            loadingSpinner.Visible = true;

            // Start the spinner animation
            _spinnerTimer.Start();

            loadingSpinner.Update(); // Force the control to redraw
            Application.DoEvents(); // Force UI update
        }

        private void HideLoadingSpinner()
        {
            // Stop the spinner animation
            _spinnerTimer.Stop();

            loadingSpinner.Visible = false;
            loadingSpinner.SendToBack();
            Application.DoEvents(); // Force UI update
        }

        #endregion

        #region Items API Tab Event Handlers

        private async void ItemsApiGetItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate branch selection
                if (itemsApiBranchComboBox.SelectedItem == null)
                {
                    MessageBox.Show("אנא בחר סניף תחילה.", "נדרש סניף", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                itemsApiGetItemButton.Enabled = false;
                ShowLoadingSpinner(); // Show loading spinner

                // Get branch and item IDs
                var branch = (Branch)itemsApiBranchComboBox.SelectedItem;
                var itemIdsText = itemsApiItemIdTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(itemIdsText))
                {
                    HideLoadingSpinner(); // Hide loading spinner
                    MessageBox.Show("אנא הזן לפחות מזהה פריט אחד.", "קלט חסר", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse item IDs
                var itemIds = itemIdsText.Split(',').Select(id => id.Trim()).Where(id => !string.IsNullOrWhiteSpace(id)).ToList();

                if (itemIds.Count == 0)
                {
                    HideLoadingSpinner(); // Hide loading spinner
                    MessageBox.Show("אנא הזן לפחות מזהה פריט אחד תקין.", "קלט לא תקין", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a progress reporter that doesn't show message boxes for every item
                var progress = new Progress<string>(s =>
                {
                    // Only log to console or update status bar, don't show message boxes
                    logTextBox.AppendText(s + Environment.NewLine);
                });

                try
                {
                    var guid = Guid.NewGuid();
                    var now = DateTime.Now;

                    logTextBox.AppendText($"מושך פריטים עבור סניף {branch.Description} עבור {itemIds.Count} ברקודים...{Environment.NewLine}");

                    // Try first with itemId parameter
                    string xml = await _comaxApiClient.GetCatalogXmlForBarcodesAsync(branch, itemIds.ToArray(), true);

                    if (string.IsNullOrEmpty(xml) || !xml.Contains("<ClsItems>"))
                    {
                        // If first attempt failed, try with barcode parameter
                        logTextBox.AppendText($"ניסיון ראשון נכשל, מנסה שוב עם פרמטר ברקוד...{Environment.NewLine}");
                        xml = await _comaxApiClient.GetCatalogXmlForBarcodesAsync(branch, itemIds.ToArray(), false);
                    }

                    if (string.IsNullOrEmpty(xml) || !xml.Contains("<ClsItems>"))
                    {
                        logTextBox.AppendText($"נכשל בקבלת פריטים מה-API של קומקס.{Environment.NewLine}");
                        _itemsApiResults = new List<AllItem>();
                    }
                    else
                    {
                        // Save the XML response to a file (optional)
                        // Skip this step as WriteApiResponseToFileAsync is not accessible

                        // Deserialize the XML to get the catalog items
                        var allClsItems = _allItemsService.DeserializeCatalogItems(xml);

                        if (allClsItems == null || !allClsItems.Any())
                        {
                            logTextBox.AppendText($"לא נמצאו פריטים בתגובת ה-XML.{Environment.NewLine}");
                            _itemsApiResults = new List<AllItem>();
                        }
                        else
                        {
                            // Filter the items by the specified barcodes if needed
                            var filteredClsItems = allClsItems.Where(item =>
                                itemIds.Contains(item.Barcode.ToString()) ||
                                (item.AnotherBarkods != null && itemIds.Any(b => item.AnotherBarkods.ToString().Contains(b)))
                            ).ToList();

                            logTextBox.AppendText($"נמצאו {filteredClsItems.Count} פריטים עבור הברקודים שצוינו. ממפה...{Environment.NewLine}");

                            // Map to AllItems but don't insert into database
                            _itemsApiResults = await _allItemsService.MapToAllItems(filteredClsItems, now, branch, guid);

                            logTextBox.AppendText($"מופו {_itemsApiResults.Count} פריטים. מציג בטבלה...{Environment.NewLine}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await _databaseLogger.LogErrorAsync("FORM1", "GetItemsForBarcodesAsync", ex);
                    _itemsApiResults = new List<AllItem>();
                    throw; // Re-throw to be caught by the outer catch block
                }

                // Display results
                itemsApiResultsDataGridView.DataSource = _itemsApiResults;

                // Enable/disable action buttons based on results
                bool hasResults = _itemsApiResults != null && _itemsApiResults.Count > 0;
                itemsApiAddSelectedButton.Enabled = hasResults;
                itemsApiAddAllButton.Enabled = hasResults;

                // Show summary message
                if (hasResults)
                {
                    MessageBox.Show($"נמצאו {_itemsApiResults.Count} פריטים.", "תוצאות חיפוש", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("לא נמצאו פריטים.", "תוצאות חיפוש", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בקבלת פריטים מה-API: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // Hide loading spinner
                itemsApiGetItemButton.Enabled = true;
            }
        }

        private async void ItemsApiAddSelectedButton_Click(object sender, EventArgs e)
        {
            if (itemsApiResultsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one item first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ShowLoadingSpinner(); // ADDED
                itemsApiAddSelectedButton.Enabled = false;

                // Get selected items
                var selectedItems = new List<AllItem>();
                foreach (DataGridViewRow row in itemsApiResultsDataGridView.SelectedRows)
                {
                    selectedItems.Add((AllItem)row.DataBoundItem);
                }

                // Add selected items to the AllItems table
                await _allItemsService.InsertAllItemsAsync(selectedItems);
                MessageBox.Show($"Added {selectedItems.Count} items to the temporary AllItems table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding selected items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // ADDED
                itemsApiAddSelectedButton.Enabled = true;
            }
        }

        private async void ItemsApiAddAllButton_Click(object sender, EventArgs e)
        {
            if (_itemsApiResults == null || _itemsApiResults.Count == 0)
            {
                MessageBox.Show("No items to add.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ShowLoadingSpinner(); // ADDED
                itemsApiAddAllButton.Enabled = false;

                // Add all items to the AllItems table
                await _allItemsService.InsertAllItemsAsync(_itemsApiResults);
                MessageBox.Show($"Added {_itemsApiResults.Count} items to the temporary AllItems table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding all items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // ADDED
                itemsApiAddAllButton.Enabled = true;
            }
        }

        private async void ItemsApiResultsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is on a valid row, not the header
            if (e.RowIndex < 0) return;

            // Ensure a branch is selected for context if needed by the service
            if (itemsApiBranchComboBox.SelectedItem == null)
            {
                MessageBox.Show("אנא בחר סניף תחילה.", "נדרש סניף", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ShowLoadingSpinner();

                var clickedItem = (AllItem)itemsApiResultsDataGridView.Rows[e.RowIndex].DataBoundItem;

                if (clickedItem != null)
                {
                    // The InsertAllItemsAsync expects a List<AllItem>
                    await _allItemsService.InsertAllItemsAsync(new List<AllItem> { clickedItem });
                    MessageBox.Show($"פריט '{clickedItem.Name}' (ברקוד: {clickedItem.Barcode}) הוסף לטבלת AllItems הזמנית.", "פריט הוסף", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בהוספת פריט: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner();
            }
        }


        private async void catalogNewTempJob_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            catalogTempJob.Enabled = false;
            //year ago date
            DateTime yearAgo = DateTime.Now.AddYears(-1);
            var progress = new Progress<string>(s => logTextBox.AppendText(s + Environment.NewLine));
            try
            {
                if (singleBranchCheckBox.Checked)
                {
                    if (branchCatalogCheckBox.Checked)
                    {
                        await _allItemsNewService.InsertCatalogAsync((Branch)branchList.SelectedItem, yearAgo, progress);
                    }
                    else
                    {
                        await _allItemsNewService.InsertCatalogAsync((Branch)branchList.SelectedItem, tempPullDateTime.Value, progress);
                    }

                }
                else
                {
                    foreach (var branch in _branches)
                    {
                        if (branchCatalogCheckBox.Checked)
                        {
                            await _allItemsNewService.InsertCatalogAsync(branch, yearAgo, progress);
                        }
                        else
                        {
                            await _allItemsNewService.InsertCatalogAsync(branch, tempPullDateTime.Value, progress);
                        }
                    }
                    // await _allItemsService.InsertCatalogAsync(null, tempPullDateTime.Value, progress);
                }
                //await _allItemsService.InsertCatalogAsync((Branch)branchList.SelectedItem, tempPullDateTime.Value, progress);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText(ex.Message);
            }
            finally
            {
                catalogTempJob.Enabled = true;
            }
        }

        #endregion

        #region Promotions API Tab Event Handlers

        private async void PromotionsApiGetPromotionsButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate branch selection
                if (promotionsApiBranchComboBox.SelectedItem == null)
                {
                    MessageBox.Show("אנא בחר סניף תחילה.", "נדרש סניף", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                promotionsApiGetPromotionsButton.Enabled = false;
                ShowLoadingSpinner(); // Show loading spinner

                // Get branch
                var branch = (Branch)promotionsApiBranchComboBox.SelectedItem;

                // Get promotions from Comax API (using a date of a year before)
                var yearAgo = DateTime.Now.AddYears(-1);

                // Create a progress reporter that only logs to the logTextBox
                var progress = new Progress<string>(s =>
                {
                    // Only log to logTextBox, don't show message boxes
                    logTextBox.AppendText(s + Environment.NewLine);
                });

                // Show start message
                logTextBox.AppendText($"מתחיל לייבא מבצעים עבור סניף {branch.Description}...{Environment.NewLine}");

                // Use FetchPromotionsWithoutInsertAsync to get promotions from a year ago
                // This will NOT insert them into the temporary promotions table
                _promotionsApiResults = await _promotionsService.FetchPromotionsWithoutInsertAsync(branch, yearAgo, progress);

                // Show end message
                logTextBox.AppendText($"סיים לייבא {_promotionsApiResults.Count} מבצעים עבור סניף {branch.Description}.{Environment.NewLine}");

                // Apply filter if there's a barcode filter
                ApplyPromotionsFilter();

                // Enable/disable action buttons based on results
                bool hasResults = _filteredPromotionsApiResults != null && _filteredPromotionsApiResults.Count > 0;
                promotionsApiAddSelectedButton.Enabled = hasResults;
                promotionsApiAddAllButton.Enabled = hasResults;

                // Show summary message
                if (hasResults)
                {
                    MessageBox.Show($"נמצאו {_filteredPromotionsApiResults.Count} מבצעים.", "תוצאות חיפוש", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("לא נמצאו מבצעים.", "תוצאות חיפוש", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בקבלת מבצעים מה-API: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // Hide loading spinner
                promotionsApiGetPromotionsButton.Enabled = true;
            }
        }

        private void PromotionsApiApplyFilterButton_Click(object sender, EventArgs e)
        {
            ApplyPromotionsFilter();
        }

        private void ApplyPromotionsFilter()
        {
            if (_promotionsApiResults == null || _promotionsApiResults.Count == 0)
            {
                _filteredPromotionsApiResults = new List<Promotion>();
                promotionsApiResultsDataGridView.DataSource = _filteredPromotionsApiResults;
                return;
            }

            var barcodeFilter = promotionsApiBarcodeFilterTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcodeFilter))
            {
                // No filter, show all promotions
                _filteredPromotionsApiResults = _promotionsApiResults;
            }
            else
            {
                // Filter promotions by barcode
                _filteredPromotionsApiResults = _promotionsApiResults
                    .Where(p =>
                        p.ItemKod.Contains(barcodeFilter) ||
                        p.Kod.Contains(barcodeFilter) ||
                        (p.AnotherBarcodes != null && p.AnotherBarcodes.Contains(barcodeFilter)))
                    .ToList();
            }

            // Display filtered results
            promotionsApiResultsDataGridView.DataSource = _filteredPromotionsApiResults;

            // Enable/disable action buttons based on filtered results
            bool hasResults = _filteredPromotionsApiResults != null && _filteredPromotionsApiResults.Count > 0;
            promotionsApiAddSelectedButton.Enabled = hasResults;
            promotionsApiAddAllButton.Enabled = hasResults;

            // Show count of filtered results
            MessageBox.Show($"Found {_filteredPromotionsApiResults.Count} promotions matching the filter.", "Filter Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void PromotionsApiAddSelectedButton_Click(object sender, EventArgs e)
        {
            if (promotionsApiResultsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one promotion first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ShowLoadingSpinner(); // ADDED
                promotionsApiAddSelectedButton.Enabled = false;

                // Get selected promotions
                var selectedPromotions = new List<Promotion>();
                foreach (DataGridViewRow row in promotionsApiResultsDataGridView.SelectedRows)
                {
                    selectedPromotions.Add((Promotion)row.DataBoundItem);
                }

                // Add selected promotions to the Promotions table
                await _promotionsRepository.BulkInsertAsync(selectedPromotions);
                MessageBox.Show($"Added {selectedPromotions.Count} promotions to the temporary Promotions table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding selected promotions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // ADDED
                promotionsApiAddSelectedButton.Enabled = true;
            }
        }

        private async void PromotionsApiAddAllButton_Click(object sender, EventArgs e)
        {
            if (_filteredPromotionsApiResults == null || _filteredPromotionsApiResults.Count == 0)
            {
                MessageBox.Show("No promotions to add.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ShowLoadingSpinner(); // ADDED
                promotionsApiAddAllButton.Enabled = false;

                // Add all filtered promotions to the Promotions table
                await _promotionsRepository.BulkInsertAsync(_filteredPromotionsApiResults);
                MessageBox.Show($"Added {_filteredPromotionsApiResults.Count} promotions to the temporary Promotions table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding all promotions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoadingSpinner(); // ADDED
                promotionsApiAddAllButton.Enabled = true;
            }
        }

        #endregion


        
    }
}
