﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EM.Comax.ShukHerzel.Models.Models;

public partial class PriceUpdate
{
    public long Id { get; set; }

    public long CompanyId { get; set; }

    public long BranchId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public bool IsTransferredToOper { get; set; }

    public DateTime? TransferredDateTime { get; set; }

    public Guid OperationGuid { get; set; }

    public string Name { get; set; }

    public string Size { get; set; }

    public string XmlId { get; set; }

    public string Barcode { get; set; }

    public string AlternateId { get; set; }

    public string PriceListId { get; set; }

    public string Currency { get; set; }

    public string IsIncludeVat { get; set; }

    public string Price { get; set; }

    public string NetPrice { get; set; }

    public string ShekelPrice { get; set; }

    public string ShekelNetPrice { get; set; }

    public string SalePrice { get; set; }

    public string OperationEndDate { get; set; }
}