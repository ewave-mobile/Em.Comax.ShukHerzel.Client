using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.CustomModels
{
    public class OutputSettings
    {
        public string ApiResponseDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Delete API response files older than this many days (by creation time).
        /// </summary>
        public int ApiResponseRetentionDays { get; set; } = 7;
    }
}
