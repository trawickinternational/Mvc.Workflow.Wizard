using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Workflow.Wizard.Common.ModelBinders
{

    public class StepViewModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var stepTypeValue = bindingContext.ValueProvider.GetValue("StepType");
            var stepType = Type.GetType((string)stepTypeValue.ConvertTo(typeof(string)), true);
            var step = Activator.CreateInstance(stepType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => step, stepType);
            return step;
        }


    }


    public class WizardModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var wizardModelTypeValue = bindingContext.ValueProvider.GetValue("WizardModelType");
            var wizardModelType = Type.GetType((string)wizardModelTypeValue.ConvertTo(typeof(string)), true);
            var wizardModel = Activator.CreateInstance(modelType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => wizardModel, wizardModelType);
            return wizardModel;
        }


    }
}
