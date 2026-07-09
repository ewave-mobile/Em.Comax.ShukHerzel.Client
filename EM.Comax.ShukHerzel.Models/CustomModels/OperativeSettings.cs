namespace EM.Comax.ShukHerzel.Models.CustomModels
{
    public class OperativeSettings
    {
        /// <summary>
        /// When true, SyncAllItemsAndPromotionsAsync calculates ContentPrice / PromoContentPrice.
        /// SyncAllItemsNewAndPromotionsAsync always calculates unit prices regardless of this setting.
        /// </summary>
        public bool CalculateUnitPrice { get; set; } = false;
    }
}
