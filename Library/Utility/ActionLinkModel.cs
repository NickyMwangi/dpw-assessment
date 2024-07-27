using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utility;

public class ActionLinkModel
{
    public ActionLinkEnum LinkType { get; set; }

    public string LinkAction { get; set; } = string.Empty;

    public string LinkId { get; set; } = string.Empty;

    public string LinkName { get; set; } = string.Empty;

    public string LinkIcon { get; set; } = string.Empty;

    public string LinkArea { get; set; } = string.Empty;

    public string LinkController { get; set; } = string.Empty;

    public string LinkClass { get; set; } = string.Empty;

    public Dictionary<string, string> LinkRoute { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> LinkDataAttr { get; set; } = new Dictionary<string, string>();
}
