using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Data.Entities.DropdownList
{
    public class ListModel
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Display { get; set; } = string.Empty;
        public string Search { get; set; } = string.Empty;
    }

    public class ListInputModel
    {
        public string DataUrl { get; set; }
        public IEnumerable<SelectListItem> SelectedItem { get; set; }
        public bool ReadOnly { get; set; } = false;
    }
}
