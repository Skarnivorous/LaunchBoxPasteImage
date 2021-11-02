using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using System.Windows.Input;
using System.Collections;
using System.Diagnostics;

namespace LaunchBoxPasteImage
{
    public class launchBoxPasteImage : IGameMenuItemPlugin
    {
        public IGame selectedGame = null;

        public bool SupportsMultipleGames
        {
            get
            {
                return false;
            }
        }

        public string Caption
        {
            get
            {
                return "Paste Image";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("LaunchBoxPasteImage.paste.png");
                Bitmap bmp = new Bitmap(myStream);
                return bmp;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                return false;
            }
        }

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return false;
        }

        public void ShowMenu()
        {
            
        }
        public void OnSelected(IGame game)
        {
            Globals.mouseX = Mouse.GetPosition(Application.Current.MainWindow).X;
            Globals.mouseY = Mouse.GetPosition(Application.Current.MainWindow).Y;
            selectedGame = game;
            SubMenu submenu = new SubMenu(game);
            submenu.Show();
        }

        public void OnSelected(IGame[] selectedGames)
        {
            //do nothing
        }
    }

    public class pluginSettings : ISystemMenuItemPlugin
    {
        public string Caption
        {
            get
            {
                return "Paste Image Settings";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("LaunchBoxPasteImage.paste.png");
                Bitmap bmp = new Bitmap(myStream);
                return bmp;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                return false;
            }
        }

        public bool AllowInBigBoxWhenLocked
        {
            get
            {
                return false;
            }
        }

        public void OnSelected()
        {
            Settings settings = new Settings();
            settings.Open();
        }
    }

    public class launchBoxSearchImage : IGameMenuItemPlugin
    {
        public IGame selectedGame = null;

        public bool SupportsMultipleGames
        {
            get
            {
                return false;
            }
        }

        public string Caption
        {
            get
            {
                return "Search Cover";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("LaunchBoxPasteImage.search.png");
                Bitmap bmp = new Bitmap(myStream);
                return bmp;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                return false;
            }
        }

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return false;
        }

        public void ShowMenu()
        {

        }
        public void OnSelected(IGame game)
        {
            ArrayList terms = new ArrayList();
            if(game.Platform != null)
            {
                terms.Add(game.Platform);
            }

            terms.Add(@"""""" + game.Title + @"""""");
            terms.Add("Game Cover");

            string term = String.Join("+", terms.ToArray());

            System.Diagnostics.Process.Start("explorer.exe", @"""http://www.google.com/search?q=" + term + @"&tbm=isch""");
        }

        public void OnSelected(IGame[] selectedGames)
        {
            //do nothing
        }
    }
}