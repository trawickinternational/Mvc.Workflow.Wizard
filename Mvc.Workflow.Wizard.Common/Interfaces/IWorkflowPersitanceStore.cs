using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvc.Workflow.Wizard.Common.Interfaces
{
    public interface IWorkflowPersistStore
    {
        //Boolean Save();
        //Boolean Refresh();
        Guid Id { get; set; }
    }
}
