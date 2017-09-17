using System;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.IO;

namespace Config
{
    /// <summary>
    ///     <see href="https://github.com/crdx/PortableSettingsProvider">PortableSettingsProvider</see>
    /// </summary>
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string RootNodeName = "settings";
        private const string LocalSettingsNodeName = "localSettings";
        private const string GlobalSettingsNodeName = "globalSettings";
        private const string ClassName = "PortableSettingsProvider";
        private XmlDocument _xmlDocument;

        private string FilePath => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            $"{ApplicationName}.settings");

        private XmlNode LocalSettingsNode
        {
            get
            {
                XmlNode settingsNode = GetSettingsNode(LocalSettingsNodeName);
                // works around parsing problems caused by computer names starting with numerical values
                XmlNode machineNode = settingsNode.SelectSingleNode($"Host-{Environment.MachineName.ToLowerInvariant()}");

                if (machineNode == null)
                {
                    // works around parsing problems caused by computer names starting with numerical values
                    machineNode = RootDocument.CreateElement($"Host-{Environment.MachineName.ToLowerInvariant()}");
                    settingsNode.AppendChild(machineNode);
                }

                return machineNode;
            }
        }

        private XmlNode GlobalSettingsNode => GetSettingsNode(GlobalSettingsNodeName);

        private XmlNode RootNode => RootDocument.SelectSingleNode(RootNodeName);

        private XmlDocument RootDocument
        {
            get
            {
                if (_xmlDocument == null)
                {
                    try
                    {
                        _xmlDocument = new XmlDocument();
                        _xmlDocument.Load(FilePath);
                    }
                    catch (Exception)
                    {

                    }

                    if (_xmlDocument.SelectSingleNode(RootNodeName) != null)
                        return _xmlDocument;

                    _xmlDocument = GetBlankXmlDocument();
                }

                return _xmlDocument;
            }
        }

        public override string ApplicationName
        {
            get { return Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location); }
            set { }
        }

        public override string Name => ClassName;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);

            try
            {
                RootDocument.Save(FilePath);
            }
            catch (Exception)
            {
                /* 
                 * If this is a portable application and the device has been 
                 * removed then this will fail, so don't do anything. It's 
                 * probably better for the application to stop saving settings 
                 * rather than just crashing outright. Probably.
                 */
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }

            return values;
        }

        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
               ? GlobalSettingsNode
               : LocalSettingsNode;

            XmlNode settingNode = targetNode.SelectSingleNode($"setting[@name='{propertyValue.Name}']");

            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = RootDocument.CreateElement("setting");

                XmlAttribute nameAttribute = RootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;

                settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();

                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? GlobalSettingsNode : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode($"setting[@name='{property.Name}']");

            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;

            return settingNode.InnerText;
        }

        private bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute)attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }

            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = RootNode.SelectSingleNode(name);

            if (settingsNode == null)
            {
                settingsNode = RootDocument.CreateElement(name);
                RootNode.AppendChild(settingsNode);
            }

            return settingsNode;
        }

        public XmlDocument GetBlankXmlDocument()
        {
            XmlDocument blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(RootNodeName));

            return blankXmlDocument;
        }

        public void Reset(SettingsContext context)
        {
            LocalSettingsNode.RemoveAll();
            GlobalSettingsNode.RemoveAll();

            _xmlDocument.Save(FilePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }
    }
}