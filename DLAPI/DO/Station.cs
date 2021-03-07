﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public bool Active { get; set; }
        public override string ToString()
        {
            return this.Code + " " + this.Name;
        }
    }
}