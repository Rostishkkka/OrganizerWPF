﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Organizer.Models;

public partial class Statuses
{
    public int Id { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}