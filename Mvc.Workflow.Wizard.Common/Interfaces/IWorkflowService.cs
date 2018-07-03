using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvc.Workflow.Wizard.Common.Interfaces;
namespace Mvc.Workflow.Wizard.Common
{
   

    public interface IWorkflowService
    {
        Guid StartWorkflow();
        String ResumeWorkflow(IWizardModel model);
        String RunWorkflow(IWizardModel model,string Bookmark);
        void Unload();
        Boolean IsCompleted { get; }
    }
}



