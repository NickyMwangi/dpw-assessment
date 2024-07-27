using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities.Service
{
    public class ListRequest
    {
        public int OffSet { get; set; }
        public int Limit { get; set; }
        public string Search { get; set; }
        public string Searchable { get; set; }
    }
}
