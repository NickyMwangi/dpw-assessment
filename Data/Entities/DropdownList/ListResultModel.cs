using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities.DropdownList
{
    public class ListResultModel
    {
        public string Header { get; set; } = string.Empty;
        public IEnumerable<ListModel>? Items { get; set; }
        public int Count { get; set; }
    }
}
