using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvc.Workflow.Wizard.Common;
using Mvc.Workflow.Wizard.Common.Interfaces;

namespace Mvc.Workflow.Wizard.Common
{

    public class WorkflowService : IWorkflowService
    {
        WizardHostHelper _wizardHostHelper;


        public WorkflowService(System.Activities.Activity workflow) : this(workflow, new Dictionary<string, object> { }) { }



        public WorkflowService(System.Activities.Activity workflow, IDictionary<string, object> inputs)
        {
            _wizardHostHelper = new WizardHostHelper(workflow, inputs);
        }

        #region IWorkflowService Members

        public Guid StartWorkflow()
        {
            return _wizardHostHelper.StartWizard();
        }

        public String ResumeWorkflow(IWizardModel model)
        {
            String bookmarkName = "Final";
            try
            {
                bookmarkName = _wizardHostHelper.ResumeWizard(model);
            }
            catch (Exception ex)
            {
                // TODO get complete exception
            }
            return bookmarkName;
        }

        public string RunWorkflow(IWizardModel model, string Bookmark)
        {
            if (string.IsNullOrEmpty(Bookmark))
            return _wizardHostHelper.RunWorkflow(model);
            else
                return GoTo(Bookmark,model);
        }

        //public string Back()
        //{
        //    return _wizardHostHelper.RunWorkflow("Back");
        //}

        public string GoTo(string BookMark, IWizardModel model)
        {
            return _wizardHostHelper.RunWorkflowWithBookmark(BookMark, model);
        }

        public void Unload()
        {
            try
            {
                _wizardHostHelper.Unload();
            }
            catch
            {
                // TODO get complete exception
            }
        }

        #endregion




        public bool IsCompleted
        {
            get { return _wizardHostHelper.IsCompleted; }
        }
    }
}
