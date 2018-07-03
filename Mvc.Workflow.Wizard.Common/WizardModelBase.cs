using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvc.Workflow.Wizard.Common.Interfaces;

namespace Mvc.Workflow.Wizard.Common.Interfaces
{

    public interface  IWizardModel : IWorkflowPersistStore
    {
        //WizardBaseModel() : base() { Steps = new List<IStepViewModel>(); }
         int CurrentStepIndex { get; set; }
         List<IStepViewModel> Steps { get; set; }
         Guid Id { get; set; }
         string Command { get; set; }
         string Title { get; set; }
         string ReturnUrl { get; set; }
         string WorkflowType { get; set; }
         System.Security.Principal.IPrincipal UserPrincipal { get; set; }
         string Layout { get; set; }


         System.Web.Mvc.UrlHelper UrlHelper();
         void SetUrlHelper(System.Web.Routing.RequestContext  helper);
     }
}


