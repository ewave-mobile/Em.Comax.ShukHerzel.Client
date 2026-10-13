using EM.Comax.ShukHerzl.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EM.Comax.ShukHerzel.Bl
{
    public class CatalogPriceApprovalBlockedException : Exception
    {
        public IReadOnlyList<string> Barcodes { get; }

        public CatalogPriceApprovalBlockedException(IEnumerable<string> barcodes)
            : base(BuildMessage(barcodes))
        {
            Barcodes = barcodes?
                .Where(barcode => !string.IsNullOrWhiteSpace(barcode))
                .Distinct()
                .ToList()
                ?? new List<string>();
        }

        private static string BuildMessage(IEnumerable<string> barcodes)
        {
            var list = barcodes?
                .Where(barcode => !string.IsNullOrWhiteSpace(barcode))
                .Distinct()
                .ToList()
                ?? new List<string>();
            if (list.Count == 0)
            {
                return Constants.CatalogPricePendingMessage;
            }

            return Constants.CatalogPricePendingMessage + " (" + string.Join(", ", list) + ")";
        }
    }
}
