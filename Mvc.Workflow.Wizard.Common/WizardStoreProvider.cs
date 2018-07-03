using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Configuration;
using Mvc.Workflow.Wizard.Common.Interfaces;

namespace Mvc.Workflow.Wizard.Common.Providers
{
    public abstract class WizardModelStoreProvider : ProviderBase
    {
        public abstract object LoadFromKey(string key);
        public abstract void SaveByKey(string key,IWizardModel item);
    }

    public class WizardModelStoreProviderCollection : ProviderCollection
    {

        new public WizardModelStoreProvider this[string name]
        {
            get { return (WizardModelStoreProvider)base[name]; }
        }
    }

    public class WizardModelStoreProviderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base["providers"];
            }
        }

        [ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }
    }

    public class WizardModelStoreProviderManager
    {
        private static WizardModelStoreProvider defaultProvider;
        private static WizardModelStoreProviderCollection providers;

        static WizardModelStoreProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            WizardModelStoreProviderConfiguration configuration =
                (WizardModelStoreProviderConfiguration)
                ConfigurationManager.GetSection("WizardModelStoreProvider");

            if (configuration == null)
                throw new ConfigurationErrorsException
                    ("WizardModelStoreProvider configuration section is not set correctly.");

            providers = new WizardModelStoreProviderCollection();

            ProvidersHelper.InstantiateProviders(configuration.Providers
                , providers, typeof(WizardModelStoreProvider));

            providers.SetReadOnly();

            defaultProvider = providers[configuration.Default];

            if (defaultProvider == null)
                throw new Exception("defaultProvider");
        }

        public static WizardModelStoreProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static WizardModelStoreProviderCollection Providers
        {
            get
            {
                return providers;
            }
        }
    }
}

