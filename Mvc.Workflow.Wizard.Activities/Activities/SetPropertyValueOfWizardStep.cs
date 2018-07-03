using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mvc.Workflow.Wizard.Common;
using Mvc.Workflow.Wizard.Common.Interfaces;
using System.Reflection;

namespace Mvc.Workflow.Wizard.Activities
{

    public  class SetPropertyValueOfWizardStep<T> : NativeActivity<T>
    {

        // Define an activity input argument of type string
        public InOutArgument<IWizardModel> WFContext { get; set; }
        public InArgument<string> PropertyName { get; set; }
        public InArgument<object> PropertyValue { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
          
            var wfcontext = context.GetValue(this.WFContext);
            string propertyName = context.GetValue(this.PropertyName);
            object propertyValue = context.GetValue(this.PropertyValue);

            var index = wfcontext.Steps.FindIndex(m => m.GetType() == typeof(T));
            if (index >= 0)
            {
                try
                {
                    PropertyInfo propertyInfo = wfcontext.Steps[index].GetType().GetProperty(propertyName);
                    propertyInfo.SetValue(wfcontext.Steps[index], propertyValue, null);
                }
                catch
                {
                    //throw ex
                }

            }
        }
    }
}
