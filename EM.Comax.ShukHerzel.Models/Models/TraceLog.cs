﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EM.Comax.ShukHerzel.Models.Models;

public partial class TraceLog
{
    public long Id { get; set; }

    public string Url { get; set; }

    public string Request { get; set; }

    public string Response { get; set; }

    public string ResponseStatus { get; set; }

    public DateTime? CreateDate { get; set; }
}