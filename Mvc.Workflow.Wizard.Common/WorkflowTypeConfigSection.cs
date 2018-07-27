using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Mvc.Workflow.Wizard.Common
{
    public class WorkflowTypeConfigElement : ConfigurationElement
    {
        public WorkflowTypeConfigElement() { }

        [ConfigurationProperty("Name", DefaultValue = "")]
        public string Name
        {
            get { return this["Name"].ToString(); }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("WorkflowType", DefaultValue = "")]
        public string WorkflowType
        {
            get { return this["WorkflowType"].ToString(); }
            set { this["WorkflowType"] = value; }
        }

        [ConfigurationProperty("ContextModelType", DefaultValue = "")]
        public string ContextModelType
        {
            get { return this["ContextModelType"].ToString(); }
            set { this["ContextModelType"] = value; }
        }
    }

    [ConfigurationCollection(typeof(WorkflowTypeConfigElement))]
    public class WorkflowTypeElementCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "WorkflowType";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }
        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool IsReadOnly()
        {
            return false;
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkflowTypeConfigElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkflowTypeConfigElement)(element)).Name ;
        }
        public WorkflowTypeConfigElement this[int idx]
        {
            get
            {
                return (WorkflowTypeConfigElement)BaseGet(idx);
            }
        }
    }


    public class WorkflowTypeConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("WorkflowTypes")]
        public WorkflowTypeElementCollection WorkflowType
        {
            get { return ((WorkflowTypeElementCollection)(base["WorkflowTypes"])); }
            set { base["WorkflowTypes"] = value; }
        }
    }

    public static class ConfigurationHelper
    {
        public static WorkflowTypeConfigElement GetWorfklowTypeConfig(string name)
        {
            foreach (WorkflowTypeConfigElement e in GetWorkflowTypeSection().WorkflowType)
            {
                if (e.Name.ToLower() == name.ToLower()) return e;
            }
            return new WorkflowTypeConfigElement();
        }

        public static WorkflowTypeConfigSection GetWorkflowTypeSection()
        {
            return (WorkflowTypeConfigSection)ConfigurationManager.GetSection("WorkflowTypesSection");
        }
    }
}

