using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using System.Web.Routing;
using Mvc.Workflow.Wizard.Common;
using Mvc.Workflow.Wizard.Common.Interfaces;
using System.Collections.Specialized;
using System.Configuration;

namespace Mvc.Workflow.Wizard.Common.Controllers
{
    public  class WizardBaseController : Controller 
    {

        private WorkflowTypeConfigElement _workflowType;


        public IWorkflowService WorkflowService { get; set; }

        protected virtual ActionResult Begin(string workflowType, System.Web.HttpContext Context,IStepViewModel step)
        {
            this.InitializeWorkflowType(workflowType);

            var model = (IWizardModel)GetNewModelContextInstance();
            string returnUrl = Request.UrlReferrer == null ? "/" : Request.UrlReferrer.PathAndQuery;
           
            model.ReturnUrl = returnUrl;
            model.WorkflowType = workflowType;
            model.UserPrincipal = Context.User;
            model.SetUrlHelper(Context.Request.RequestContext);

            if (step != null) model.Steps.Add(step);
            if (WorkflowService == null) { WorkflowService = new WorkflowService((System.Activities.Activity)GetNewWorfklowActivityIntance(), new Dictionary<string, object>() { { "Context", model} }); }

            Guid workflowKey = WorkflowService.StartWorkflow();

            model.Id = workflowKey;
            Providers.WizardModelStoreProviderManager.Provider.SaveByKey(workflowKey.ToString(), model);
            WorkflowService.Unload();

            return RedirectToAction("Resume", "Wizard", new { workflowKey = workflowKey, workflowType = workflowType });
        }

        protected virtual ActionResult Resume(Guid workflowKey, System.Web.HttpContext Context)
        {
            //Load Store from Provider
            IWizardModel item = (IWizardModel)Providers.WizardModelStoreProviderManager.Provider.LoadFromKey(workflowKey.ToString());

            this.InitializeWorkflowType(item.WorkflowType);
            WorkflowService = new WorkflowService((System.Activities.Activity)GetNewWorfklowActivityIntance());

           
            item.UserPrincipal = Context.User;
            item.SetUrlHelper(Context.Request.RequestContext);

            // Determine which workflow step wizard is at, this would be were persisted data would be loaded from model
            String step = WorkflowService.ResumeWorkflow(item);

            WorkflowService.Unload();

            if (WorkflowService.IsCompleted || !string.IsNullOrEmpty(item.ReturnUrl))
            {
                return Redirect(item.ReturnUrl);
            }
            else

            return View("Index",item);
        }

        protected virtual ActionResult Resume(string submitButton, Guid workflowKey, [DeserializeAttribute] IWizardModel Wizard, IStepViewModel stepModel)
        {
            this.InitializeWorkflowType(Wizard.WorkflowType);
            WorkflowService = new WorkflowService((System.Activities.Activity)GetNewWorfklowActivityIntance());
            if (ModelState.IsValid || submitButton != "Next" || stepModel.DontValidateNextButton)
            {
                //Persist Models
                Wizard.Steps[Wizard.CurrentStepIndex] = stepModel;
                Wizard.Command = submitButton;
                //Wizard.Save();

                Wizard.SetUrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
                //Resume Workflow
                WorkflowService.ResumeWorkflow(Wizard);

                WorkflowService.RunWorkflow(Wizard,string.Empty);

                WorkflowService.Unload();

                Providers.WizardModelStoreProviderManager.Provider.SaveByKey(Wizard.Id.ToString(), Wizard);
                //Wizard.Save();
                ModelState.Clear();

                if (WorkflowService.IsCompleted || !string.IsNullOrEmpty(Wizard.ReturnUrl))
                {
                    return Redirect(Wizard.ReturnUrl);
                }
                else
                    return View("Index",Wizard);

            }

            return View("Index",Wizard);
        }

        protected virtual ActionResult ResumeBookMark(Guid workflowKey, string bookMark, System.Web.HttpContext Context)
        {
            var Wizard = (IWizardModel)Providers.WizardModelStoreProviderManager.Provider.LoadFromKey(workflowKey.ToString());

            this.InitializeWorkflowType(Wizard.WorkflowType);
            WorkflowService = new WorkflowService((System.Activities.Activity)GetNewWorfklowActivityIntance());
           
            Wizard.UserPrincipal = Context.User;
            Wizard.SetUrlHelper(Context.Request.RequestContext);          
                //Resume Workflow
            WorkflowService.ResumeWorkflow(Wizard);
            WorkflowService.RunWorkflow(Wizard, bookMark);

            WorkflowService.Unload();

            Providers.WizardModelStoreProviderManager.Provider.SaveByKey(Wizard.Id.ToString(), Wizard);
                //Wizard.Save();
            ModelState.Clear();

            if (WorkflowService.IsCompleted || !string.IsNullOrEmpty(Wizard.ReturnUrl))
            {
                return Redirect(Wizard.ReturnUrl);
            }
            else
                return View("Index", Wizard);

            

            return View("Index", Wizard);
        }

        private void InitializeWorkflowType(string name)
        {
            _workflowType = ConfigurationHelper.GetWorfklowTypeConfig(name);
        }

        private object GetNewWorfklowActivityIntance()
        {

            if (!string.IsNullOrEmpty(_workflowType.WorkflowType))
                return Activator.CreateInstance(Type.GetType(_workflowType.WorkflowType));
            else
                return null;
        }

        private IWizardModel GetNewModelContextInstance()
        {

            if (!string.IsNullOrEmpty(_workflowType.ContextModelType))
                return (IWizardModel)Activator.CreateInstance(Type.GetType(_workflowType.ContextModelType));
            else
                return null;
        }
    }

}

