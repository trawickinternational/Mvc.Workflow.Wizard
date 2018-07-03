using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvc.Workflow.Wizard.Common.ViewModels
{
    [Serializable]
    public class InputTextValueViewModel :BaseStepViewModel 
    {
        public string InputText { get; set; }
        public string InputTextClass { get; set; }

        public string InstructionClass { get; set; }

    }
}
