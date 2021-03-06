﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbot.Models
{
    public class StallDto
    {
        public int Level { get; set; }
        public int Capacity { get; set; }
        public int Jackpot { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public string Name { get; set; }
        public int MaxOutput { get; set; }
        public int MaxPot { get; set; }
    }
}
