namespace Em.Comax.ShukHerzel.Client
{
    /// <summary>
    /// Grid row for price-update Oper-transfer approval.
    /// </summary>
    public class PriceUpdateApprovalRow
    {
        public bool IsSelected { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// CreatedDateTime truncated to the hour (minutes ignored in display).
        /// </summary>
        public string CreatedHourDisplay { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Barcode { get; set; } = string.Empty;

        public string NetPrice { get; set; } = string.Empty;
    }
}
