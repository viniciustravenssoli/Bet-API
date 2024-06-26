﻿using System.ComponentModel.DataAnnotations;

namespace Bet.Communication.Request;
public class PageQuery
{
    [Range(0, 50)]
    public int Top { get; set; } = 10;

    [Range(0, int.MaxValue)]
    public int Skip { get; set; } = 0;

}
