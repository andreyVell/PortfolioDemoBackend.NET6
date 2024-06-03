using DataCore.Enums;
using DataProvider;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Services.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CheckAccessToEntityActionAttribute : AuthorizeAttribute
    {
        //TODOsecondary попробовать сделать, если не получится написать почему не получилось разработать
        private readonly IAvetonDbContext _avetonDbContext;     
        
        public string EntityName { get; set; }
        public EntityAction EntityAction { get; set; }

        public CheckAccessToEntityActionAttribute(IAvetonDbContext avetonDbContext)
        {
            _avetonDbContext = avetonDbContext;
        }



        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //точка входа
            if (IsAuthorized(actionContext))
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        

        // Handle unauthorized requests
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "This action is not allowed");
        }
    }
}
