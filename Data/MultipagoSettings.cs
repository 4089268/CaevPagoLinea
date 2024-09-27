using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Data
{
    public class MultipagoSettings {
        public int Account {get;set;}
        public int Product {get;set;}
        public int Node {get;set;}
        public string Key {get;set;}
    }
}