using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvc.Workflow.Wizard.Common.Interfaces;

namespace Mvc.Workflow.Wizard.Common
{

    [Serializable]
    public class WizardViewButton
    {
        public string ButtonText
        {
            get;
            set;
        }
        public Boolean CausesValidation { get; set; }
       
    }

    [Serializable]
    public class BaseStepViewModel : IStepViewModel

    {
        public virtual string StepType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public virtual string BookMark
        {
            get;
            set;
        }

        public virtual string Template
        {
            get;
            set;
        }
        
        public virtual bool HideProgress { get; set; }

        public virtual string Instruction { get; set; }

        public virtual bool DontValidateNextButton { get; set; }

        public virtual WizardViewButton[] Buttons
        {
            get;
            set;
        }
    }
}
