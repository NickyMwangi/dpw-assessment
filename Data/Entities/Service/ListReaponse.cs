using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities.Service
{
    public class ListReaponse<T>
    {
        public int Total { get; set; }
        public int TotalNotFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }
}
}
