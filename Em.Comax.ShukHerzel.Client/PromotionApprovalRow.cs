namespace Em.Comax.ShukHerzel.Client
{
    /// <summary>
    /// Grid row for promotion (תגמול) Oper-transfer approval / broadcast.
    /// </summary>
    public class PromotionApprovalRow
    {
        public bool IsSelected { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// CreatedDateTime truncated to the hour (minutes ignored in display).
        /// </summary>
        public string CreatedHourDisplay { get; set; } = string.Empty;

        public string Kod { get; set; } = string.Empty;

        public string ItemKod { get; set; } = string.Empty;

        public string Nm { get; set; } = string.Empty;

        public string DateRangeDisplay { get; set; } = string.Empty;

        public string Total { get; set; } = string.Empty;
    }
}
