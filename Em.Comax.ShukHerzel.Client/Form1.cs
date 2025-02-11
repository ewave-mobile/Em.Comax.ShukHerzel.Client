using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Bl.services;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Em.Comax.ShukHerzel.Client
{
    public partial class Form1 : Form
    {
        private readonly IBranchService _branchService;
        private readonly IAllItemsService _allItemsService;
        private List<Branch> _branches = null;
        private readonly IPromotionsService _promotionsService;
        private readonly IOperativeService _operativeService;
        private readonly IApiClientService _apiClientService;
        private readonly IPriceUpdateService _priceUpdateService;
        //private const long SHUK_HERZEL_COMPANY_ID = 1;

        public Form1(IBranchService branchService, IAllItemsService allItemsService, IPromotionsService promotionsService, IOperativeService operativeService, IApiClientService apiClientService, IPriceUpdateService priceUpdateService)
        {
            _branchService = branchService;
            _allItemsService = allItemsService;
            _promotionsService = promotionsService;
            _operativeService = operativeService;
            _apiClientService = apiClientService;
            InitializeComponent();
            _ = initForm();
            _priceUpdateService = priceUpdateService;
        }
        private async Task<bool> initForm()
        {
            branchList.Enabled = false;
            tempPullDateTime.Enabled = true;
            _branches = await _branchService.GetAllBranchesByCompany(Constants.SHUK_HERZEL_COMPANY_ID);
            branchList.DataSource = _branches;
            branchList.DisplayMember = "BranchName";
            branchList.ValueMember = "Id";
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
    }
}
