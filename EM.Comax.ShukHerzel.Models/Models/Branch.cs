﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EM.Comax.ShukHerzel.Models.Models;

public partial class Branch
{
    public long Id { get; set; }

    public long? CompanyId { get; set; }

    public string EslStoreId { get; set; }

    public string Description { get; set; }

    public string BranchName { get; set; }

    public long? ComaxStoreId { get; set; }

    public long? ComaxPriceListId { get; set; }

    public long? SalesPriceArray { get; set; }

    public long? PromotionStoreListId { get; set; }

    public int? Order { get; set; }

    public bool? IsServiceSync { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastRun { get; set; }

    public DateTime? LastCatalogTimeStamp { get; set; }

    public DateTime? LastPromotionTimeStamp { get; set; }

    public DateTime? LastPriceTimeStamp { get; set; }
}