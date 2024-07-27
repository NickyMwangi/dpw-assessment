using Library.Utility;
using System.Collections.Generic;

namespace WebUI.Extensions.Helpers
{
    public static class ActionLinkHelper
    {
        public static ActionLinkModel CreateActionLink(this ActionLinkEnum linkEnum, string id)
        {
            string action;
            string name;
            string icon="";
            string actionClass = null;
            Dictionary<string, string> route = new Dictionary<string, string>();
            Dictionary<string, string> dataAttr = new Dictionary<string, string>();
            switch (linkEnum)
            {
                case ActionLinkEnum.RowEdit:
                    action = "AddEdit";
                    name = "Edit";
                    icon = "far fa-edit";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDetail:
                    action = "AddEdit";
                    name = "Detail";
                    icon = "far fa-newspaper";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowView:
                    action = "AddEdit";
                    name = "View";
                    icon = "far fa-folder-open";
                    route.Add("viewOnly", true.ToString());
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDelete:
                    action = "Delete";
                    name = "Delete";
                    icon = "far fa-trash-alt";
                    actionClass ??= "confirm-delete";
                    break;
                case ActionLinkEnum.CardReport:
                    action = "Report";
                    name = "Report Name";
                    icon = "far fa-file-pdf";
                    break;
                default:
                    action = "AddEdit";
                    name = "Custom";
                    icon = "fas fa-cog";
                    break;
            }
            return new ActionLinkModel()
            {
                LinkType = linkEnum,
                LinkAction = action,
                LinkId = id,
                LinkRoute = route,
                LinkName = name,
                LinkIcon = icon,
                LinkArea = null,
                LinkController = null,
                LinkClass = actionClass,
                LinkDataAttr = dataAttr
            };
        }

        public static ActionLinkModel CreateActionLink(this ActionLinkEnum linkEnum, string id, string name, 
            string action, string icon=null)
        {
            Dictionary<string, string> route = new Dictionary<string, string>();
            Dictionary<string, string> dataAttr = new Dictionary<string, string>();
            string actionClass = null;
            switch (linkEnum)
            {
                case ActionLinkEnum.RowEdit:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-edit";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDetail:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-newspaper";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowView:
                    action ??= "AddEdit";
                    name ??= "View";
                    icon ??= "far fa-folder-open";
                    route.Add("viewOnly", true.ToString());
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDelete:
                    action ??= "Delete";
                    name ??= "Delete";
                    icon ??= "far fa-trash-alt";
                    actionClass ??= "confirm-delete";
                    break;
                case ActionLinkEnum.CardReport:                    
                    action = "Report";
                    name ??= "Report";
                    icon ??= "far fa-file-pdf";
                    break;
                case ActionLinkEnum.CardAction:
                    action = "PageAction";
                    name ??= "Page Action";
                    icon ??= "fas fa-cog";
                    break;
                default:
                    action ??= "AddEdit";
                    name ??= "Custom";
                    icon ??= "fas fa-cog";
                    break;
            }
            return new ActionLinkModel()
            {
                LinkType = linkEnum,
                LinkAction = action,
                LinkId = id,
                LinkRoute = route,
                LinkName = name,
                LinkIcon = icon,
                LinkArea = null,
                LinkController = null,
                LinkClass = actionClass,
                LinkDataAttr = dataAttr
            };
        }

        public static ActionLinkModel CreateActionLink(this ActionLinkEnum linkEnum, string id, Dictionary<string, string> route, 
            string name = null, string action=null, string icon =null, Dictionary<string, string> dataAttr=null)
        {
            string actionClass = null;
            route ??= new Dictionary<string, string>();
            dataAttr ??= new Dictionary<string, string>();
            string controller = null;
            string area = null;
            switch (linkEnum)
            {
                case ActionLinkEnum.RowEdit:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon = "far fa-edit";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDetail:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon = "far fa-newspaper";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowView:
                    action ??= "AddEdit";
                    name ??= "View";
                    icon = "far fa-folder-open";
                    actionClass ??= "show-modal";
                    route.Add("viewOnly", "true");
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDelete:
                    action ??= "Delete";
                    name ??= "Delete";
                    icon = "far fa-trash-alt";
                    actionClass ??= "confirm-delete";
                    break;
                case ActionLinkEnum.CardReport:
                    controller = "Report";
                    area = "";
                    action ??= "Report";
                    name ??= "Report";
                    icon = "far fa-file-pdf";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.CardAction:
                    action = "PageAction";
                    name ??= "Page Action";
                    icon ??= "fas fa-cog";
                    break;
                case ActionLinkEnum.ApprovalAction:
                    action = "ApprovalAction";
                    name ??= "Approval Action";
                    icon ??= "fas fa-cog";
                    break;
                default:
                    name = "Custom";
                    icon = "fas fa-cog";
                    break;
            }
            return new ActionLinkModel()
            {
                LinkType = linkEnum,
                LinkAction = action,
                LinkId = id,
                LinkRoute = route,
                LinkName = name,
                LinkIcon = icon,
                LinkController = controller,
                LinkArea = area,
                LinkClass = actionClass,
                LinkDataAttr = dataAttr
            };
        }

        public static ActionLinkModel CreateActionLink(this ActionLinkEnum linkEnum, string id, string action,
            string controller, string name, Dictionary<string, string> route,  string actionClass = null, Dictionary<string, string> dataAttr = null)
        {
            string icon = null;
            route ??= new Dictionary<string, string>();
            dataAttr = new Dictionary<string, string>();
            string area = null;
            switch (linkEnum)
            {
                case ActionLinkEnum.RowEdit:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-edit";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDetail:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-newspaper";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowView:
                    action ??= "AddEdit";
                    name ??= "View";
                    icon ??= "far fa-folder-open";
                    actionClass ??= "show-modal";
                    route.Add("viewOnly", "true");
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDelete:
                    action ??= "Delete";
                    name ??= "Delete";
                    icon ??= "far fa-trash-alt";
                    actionClass ??= "confirm-delete";
                    break;
                case ActionLinkEnum.CardReport:
                    controller = "Report";
                    area = "";
                    action ??= "Report";
                    name ??= "Report";
                    icon = "far fa-file-pdf";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.CardAction:
                    action = "PageAction";
                    name ??= "Report";
                    icon ??= "fas fa-cog";
                    break;
                default:
                    name ??= "Custom";
                    icon ??= "fas fa-cog";
                    break;
            }
            return new ActionLinkModel()
            {
                LinkType = linkEnum,
                LinkAction = action,
                LinkId = id,
                LinkRoute = route,
                LinkName = name,
                LinkIcon = icon,
                LinkController = controller,
                LinkArea = area,
                LinkClass = actionClass,
                LinkDataAttr = dataAttr
            };
        }

        public static ActionLinkModel CreateActionLink(this ActionLinkEnum linkEnum, string id, string action, string name, string icon,
            string controller = null, string area = null, Dictionary<string, string> route = null, string actionClass = null,Dictionary<string, string> dataAttr=null)
        {
            route ??= new Dictionary<string, string>();
            dataAttr ??= new Dictionary<string, string>();
            switch (linkEnum)
            {
                case ActionLinkEnum.RowEdit:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-edit";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDetail:
                    action ??= "AddEdit";
                    name ??= "Edit";
                    icon ??= "far fa-newspaper";
                    actionClass ??= "show-modal";
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowView:
                    action ??= "AddEdit";
                    name ??= "View";
                    icon ??= "far fa-folder-open";
                    actionClass ??= "show-modal";
                    route.Add("viewOnly", "true");
                    dataAttr.Add("data-load", " ");
                    break;
                case ActionLinkEnum.RowDelete:
                    action ??= "Delete";
                    name ??= "Delete";
                    icon ??= "far fa-trash-alt";
                    actionClass ??= "confirm-delete";
                    break;
                default:
                    name ??= "Custom";
                    icon ??= "fas fa-cog";
                    break;
            }
            return new ActionLinkModel()
            {
                LinkType = linkEnum,
                LinkAction = action,
                LinkId = id,
                LinkRoute = route,
                LinkName = name,
                LinkIcon = icon,
                LinkArea = area,
                LinkController = controller,
                LinkClass = actionClass,
                LinkDataAttr = dataAttr
            };
        }       
    }
}
