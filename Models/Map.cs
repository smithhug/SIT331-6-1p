﻿using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class Map
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Columns { get; set; }

    public int Rows { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool? IsSquare { get; set; }
}
