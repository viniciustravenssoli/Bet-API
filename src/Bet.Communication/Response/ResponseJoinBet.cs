﻿using Bet.Domain.Entities;

namespace Bet.Communication.Response;
public class ResponseJoinBet
{
    public double Value { get; set; }
    public double PossibleReturn { get; set; }
    public long Chose { get; set; }
}
