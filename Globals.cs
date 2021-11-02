using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LaunchBoxPasteImage
{
    public static class Globals
    {
        public static XmlDocument _settings = null;
        public static XmlDocument settings
        {
            get
            {
                if(_settings == null) {
                    Settings.GetOrCreateSettings();
                }
                
                return _settings;
            }

            set
            {
                _settings = value;
            }
        }

        public static double mouseX;
        public static double mouseY;
    }
}
