using EM.Comax.ShukHerzel.Models.Models;

namespace Em.Comax.ShukHerzel.Client
{
    /// <summary>
    /// ComboBox item showing branch name and Comax store number.
    /// </summary>
    public class BranchListItem
    {
        public Branch Branch { get; }

        public BranchListItem(Branch branch)
        {
            Branch = branch;
        }

        public long Id => Branch.Id;

        public string DisplayText => $"{Branch.BranchName} ({Branch.ComaxStoreId})";
    }
}
