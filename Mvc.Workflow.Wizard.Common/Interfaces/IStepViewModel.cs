using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvc.Workflow.Wizard.Common.Interfaces
{
    public interface IStepViewModel
    {
        string StepType { get; }
        string BookMark { get; set; }
        string Template { get; set; }
        Boolean HideProgress { get; set; }
        string Instruction { get; set; }
        Boolean DontValidateNextButton { get; set; }
        WizardViewButton[] Buttons { get; set; }

    }
}
