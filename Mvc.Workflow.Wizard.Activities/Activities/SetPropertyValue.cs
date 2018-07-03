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
    public sealed class SetPropertyValue<T> : NativeActivity<T>
    {
        // Define an activity input argument of type string
        public InArgument<object> Object { get; set; }
        public InArgument<string> PropertyName { get; set; }
        public InArgument<string> PropertyValue { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {

            var obj = context.GetValue(this.Object);
            string propertyName = context.GetValue(this.PropertyName);
            object propertyValue = context.GetValue(this.PropertyValue);
            // Obtain the runtime value of the Text input argument
            try
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(obj, propertyValue, null);
                Result.Set(context, obj);
            }
            catch
            {
                //throw ex
            }
        }
    }
}
