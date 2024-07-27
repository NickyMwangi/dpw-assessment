using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enums
{
    public enum FileTypesEnum
    {
        [field: Description("application/force-download"), Category(".xlsx")]
        Excel = 0,
        [field: Description("application/pdf"), Category(".pdf")]
        PDF = 2,
        [field: Description("application/force-download"), Category(".docx")]
        Word = 3,
        [field: Description("application/force-download"), Category(".pdf")]
        Download = 5
    }
}
