using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace services.Resources.Filters
{
    /**
     * restricts the logged in user to working only with projects related to them.
     */
    public class ProjectAuthAttribute : FilterAttribute, IActionFilter
    {
        private int _permissionType;

        //setup the permission type to check for (default is VIEW. ie: do they have the right to view this project)
        public ProjectAuthAttribute(int permissionType = AuthorizationManager.VIEW_PERMISSION)
        {
            _permissionType = permissionType;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int id = -1;
            if (filterContext.RouteData.Values.ContainsKey("id"))
            {
                id = Convert.ToInt32(filterContext.RouteData.Values["id"]);
            }

            if (id == -1)
                throw new Exception("Project Id was not set!  We cannot verify authorization.");

            //string userName = filterContext.HttpContext.User.Identity.Name;

            if (!AuthorizationManager.IsCurrentUserAuthorized(_permissionType, AuthorizationManager.PROJECT_ENTITY, id))
            {
                filterContext.Result = new HttpNotFoundResult();
            }

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //not yet implemented.
        }
    }

}