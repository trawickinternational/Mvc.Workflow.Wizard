using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Activities;
using System.Threading;
using System.Activities.DurableInstancing;
using Mvc.Workflow.Wizard.Common;
using Mvc.Workflow.Wizard.Common.Interfaces;
using System.Configuration;

namespace Mvc.Workflow.Wizard.Common
{
    public class WizardHostHelper
    {
        WorkflowApplication _workflowApplication;
        AutoResetEvent _instanceUnloaded = new AutoResetEvent(false);
        bool _isCompleted = false;

        public Boolean IsCompleted { get { return _isCompleted; } }
        string InstanceStoreConnectionString { get { return System.Configuration.ConfigurationManager.AppSettings["InstanceStoreConnectionString"]; } }


        public WizardHostHelper(System.Activities.Activity workflow) : this(workflow, null) { }

        public WizardHostHelper(System.Activities.Activity workflow, IDictionary<string, object> inputs)
        {
            if (inputs.Count > 0)
                _workflowApplication = new WorkflowApplication(workflow, inputs);
            else
                _workflowApplication = new WorkflowApplication(workflow);
            

            //Tracking Participant
            SqlTracking.SqlTrackingBehavior beh = new SqlTracking.SqlTrackingBehavior(InstanceStoreConnectionString, "Troubleshooting Profile");

            SqlTracking.SqlTrackingParticipant track = new SqlTracking.SqlTrackingParticipant()
            {
                ConnectionString = InstanceStoreConnectionString,
                TrackingProfile = beh.GetProfile("Troubleshooting Profile","TroubleShooting")
            };

            _workflowApplication.InstanceStore = new SqlWorkflowInstanceStore(InstanceStoreConnectionString);

            _workflowApplication.Extensions.Add(track);
 
            _workflowApplication.PersistableIdle = (e) =>
            {
                return PersistableIdleAction.Persist;
            };
            _workflowApplication.Completed = (e) =>
            {
                _isCompleted = true;
                _instanceUnloaded.Set();

            };
            _workflowApplication.Idle = (e) =>
            {
                _instanceUnloaded.Set();
            };
        }



        public Guid StartWizard()
        {

            _workflowApplication.Run();

            _instanceUnloaded.WaitOne();

            return _workflowApplication.Id;
        }

        public void Unload()
        {
            if (_workflowApplication != null)
            {
                _workflowApplication.Unload();
            }
        }

        public string ResumeWizard(IWizardModel model)
        {
            _workflowApplication.Load(model.Id);

            string bookmarkName = "final";
            System.Collections.ObjectModel.ReadOnlyCollection<System.Activities.Hosting.BookmarkInfo> bookmarks = _workflowApplication.GetBookmarks();
            if (bookmarks != null && bookmarks.Count > 0)
            {
                bookmarkName = bookmarks[0].BookmarkName;
            }

            return bookmarkName;
        }


        public string RunWorkflow(IWizardModel model)
        {
            string bookmarkName = string.IsNullOrEmpty(model.Steps[model.CurrentStepIndex].BookMark) ? "final" : model.Steps[model.CurrentStepIndex].BookMark;

            //string bookmarkName = _workflowApplication.GetBookmarks()[0].BookmarkName;
            _workflowApplication.ResumeBookmark(bookmarkName, model);
            _instanceUnloaded.WaitOne();
            if (!_isCompleted)
            {
                bookmarkName = _workflowApplication.GetBookmarks()[0].BookmarkName;
                return bookmarkName;
            }
            else
            {
                return "final";
            }
        }
        public string RunWorkflowWithBookmark(string Bookmark, IWizardModel model)
        {

            _workflowApplication.ResumeBookmark(Bookmark, model);
            _instanceUnloaded.WaitOne();

            if (!_isCompleted)
            {
                Bookmark = _workflowApplication.GetBookmarks()[0].BookmarkName;
                return Bookmark;
            }
            else
            {
                return "final";
            }
        }

    }
}