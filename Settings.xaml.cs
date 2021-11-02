using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.IO;

namespace LaunchBoxPasteImage
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            GetOrCreateSettings();
        }

        public void Open()
        {
            var w = new Settings();
            w.DeleteExtraImages.IsChecked = GetSetting("DeleteExtraImages") == "True" ? true : false;
            w.IgnoreRegion.IsChecked = GetSetting("IgnoreRegion") == "True" ? true : false;
            w.Show();
        }

        public static string GetSetting(string setting)
        {
            GetOrCreateSettings();
            string value = Globals.settings.DocumentElement.Attributes[setting].Value;
            return value;
        }

        public static void SetSetting(string setting, string value)
        {
            GetOrCreateSettings();
            XmlNode settingNode = Globals.settings.DocumentElement;
            XmlAttribute settingAttribute = Globals.settings.CreateAttribute(setting);
            settingAttribute.Value = value;
            settingNode.Attributes.SetNamedItem(settingAttribute);

            SaveSettings();
        }

        public static void SaveSettings()
        {
            GetOrCreateSettings();
            string settingsDirectoryPath = System.IO.Directory.GetCurrentDirectory() + "\\Data\\";
            string settingsFilePath = settingsDirectoryPath + "PasteImageSettings.xml";
            Globals.settings.Save(settingsFilePath);
        }

        public static XmlDocument GetOrCreateSettings()
        {
            if(Globals._settings == null)
            {
                Globals._settings = new XmlDocument();
                string settingsDirectoryPath = System.IO.Directory.GetCurrentDirectory() + "\\Data\\";
                string settingsFilePath = settingsDirectoryPath + "PasteImageSettings.xml";

                if (!System.IO.File.Exists(settingsFilePath))
                {
                    StreamWriter settingsFileToWrite = System.IO.File.CreateText(settingsFilePath);
                    settingsFileToWrite.Write(@"<?xml version=""1.0"" standalone=""yes""?>
                    <Settings xmlns=""http://www.skarn-does-not-exist.com"" 
                        IgnoreRegion=""False"" 
                        DeleteExtraImages=""False"">
                    </Settings>");
                    settingsFileToWrite.Close();
                }

                StreamReader settingsFileToRead = new StreamReader(settingsFilePath);
                Globals._settings.Load(settingsFileToRead);
                settingsFileToRead.Close();
                settingsFileToRead.Dispose();
            }

            return Globals._settings;
        }

        private void DeleteExtraImages_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = this.DeleteExtraImages.IsChecked;
            SetSetting("DeleteExtraImages", isChecked == true ? "True" : "False");
        }

        private void IgnoreRegion_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = this.IgnoreRegion.IsChecked;
            SetSetting("IgnoreRegion", isChecked == true ? "True" : "False");
        }
    }
}
