﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
