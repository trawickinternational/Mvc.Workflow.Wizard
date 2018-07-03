using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mvc.Workflow.Wizard.Common;
using Mvc.Workflow.Wizard.Common.Interfaces;

namespace Mvc.Workflow.Wizard.Activities
{

    public  class WizardStep<T> : NativeActivity<T>
    {
        // Define an activity input argument of type string
        public InOutArgument<IWizardModel> WFContext { get; set; }
        public InArgument<string> BookmarkName { get; set; }
        public InArgument<string> Templatename { get; set; }
        public OutArgument<string> Input { get; set; }
        public InArgument<object> Instance { get; set; }
        public InArgument<string> Instruction { get; set; }
        public InArgument<WizardViewButton[]> Buttons { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        public WizardStep()
            : base()
        {
        }
        protected override void Execute(NativeActivityContext context)
        {

            string bookmarkName = context.GetValue(this.BookmarkName);
            var wfcontext = context.GetValue(this.WFContext);
            string templateName = context.GetValue(this.Templatename);

            string instruction = context.GetValue(this.Instruction);

            var instance = context.GetValue(this.Instance);
            var buttons = context.GetValue(this.Buttons);

            ////refresh from DB
            //wfcontext.Refresh();


            //Set Current Step
            var index = wfcontext.Steps.FindIndex(m => m.BookMark == bookmarkName );
            if (index >= 0)
            {
                wfcontext.CurrentStepIndex = index;
                wfcontext.Steps[index].BookMark = bookmarkName;
            }
            else
            {
                var model = Activator.CreateInstance(typeof(T));
                if (instance != null)
                    model = (IStepViewModel)instance;
                var stepModel = (IStepViewModel)model;
                stepModel.Template = templateName;
                stepModel.Instruction = string.IsNullOrEmpty(stepModel.Instruction) ? instruction : stepModel.Instruction;
                stepModel.Buttons = buttons;

                stepModel.BookMark = bookmarkName;

                wfcontext.Steps.Add(stepModel);
                wfcontext.CurrentStepIndex = wfcontext.Steps.Count - 1;
            }

            //Save to DB
            wfcontext.Id = context.WorkflowInstanceId;
            Mvc.Workflow.Wizard.Common.Providers.WizardModelStoreProviderManager.Provider.SaveByKey(wfcontext.Id.ToString(), wfcontext);

            //Return
            context.CreateBookmark(bookmarkName, new BookmarkCallback(this.Continue));
            context.SetValue(WFContext, wfcontext);
        }

       public void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {


            IWizardModel model = (IWizardModel)obj;

            //if (model.Steps[model.CurrentStepIndex].HideProgress == true)
            //{
            //    model.Steps.Remove(model.Steps[model.CurrentStepIndex]);
            //    model.CurrentStepIndex-=1;
            //}

            context.SetValue(WFContext, model);
            Input.Set(context, model.Command);
            Result.Set(context,model.Steps[model.CurrentStepIndex]);


        }

        protected override  bool CanInduceIdle { get { return true; } }
    }
}
