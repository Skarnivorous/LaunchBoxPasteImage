using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace LaunchBoxPasteImage
{
    /// <summary>
    /// Interaction logic for SubMenu.xaml
    /// </summary>
    public partial class SubMenu : Window
    {
        public IGame gameTarget = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument settings = Globals.settings;
            string deleteOtherImages = Settings.GetSetting("DeleteExtraImages") ?? "False";
            string ignoreRegion = Settings.GetSetting("IgnoreRegion") ?? "False";

            try { 
                string imageType = null;

                ListBoxItem bt = e.Source as ListBoxItem;
                string parameter = bt.Name;
                if(parameter == "Cancel")
                {
                    this.Close();
                    return;
                }


                if(parameter == "BoxFront")
                {
                    imageType = "Box - Front";
                }

                if (parameter == "BoxBack")
                {
                    imageType = "Box - Back";
                }

                string region = (gameTarget.Region == null || gameTarget.Region == "") ? "None" : gameTarget.Region;

                ImageDetails[] images = gameTarget.GetAllImagesWithDetails();

                ArrayList existing01Images = new ArrayList();
                Dictionary<string, int?> regionCount = new Dictionary<string, int?>();

                //collect images to be renamed, or delete images if options are set
                for (int i = 0; i < images.Length; i++)
                {
                    ImageDetails imageDetails = images[i];
                    string imageRegion = imageDetails.Region == null ? "None" : imageDetails.Region;
                    int imageRegionCount = regionCount.GetValueOrDefault(imageRegion) ?? 0;
                    regionCount[imageRegion] = imageRegionCount + 1;

                    if (imageDetails.ImageType == imageType)
                    {
                        if(ignoreRegion == "True")
                        {
                            if(deleteOtherImages == "False")
                            {
                                if(imageRegion == "None")
                                {
                                    string imageFilename = System.IO.Path.GetFileNameWithoutExtension(imageDetails.FilePath);
                                    if (imageFilename.Substring(imageFilename.Length - 3) == "-01")
                                    {
                                        existing01Images.Add(imageDetails);
                                    }
                                }
                            }
                            
                            if(deleteOtherImages == "True")
                            {
                                System.IO.File.Delete(imageDetails.FilePath);
                            }
                        }
                        
                        if(ignoreRegion == "False")
                        {
                            if (deleteOtherImages != "True")
                            {
                                if (imageRegion == region)
                                {
                                    string imageFilename = System.IO.Path.GetFileNameWithoutExtension(imageDetails.FilePath);
                                    if (imageFilename.Substring(imageFilename.Length - 3) == "-01")
                                    {
                                        existing01Images.Add(imageDetails);
                                    }
                                }
                            }
                            
                            if(deleteOtherImages == "True")
                            {
                                if (imageRegion == region)
                                {
                                    System.IO.File.Delete(imageDetails.FilePath);
                                }
                            }
                        }
                        if (deleteOtherImages == "True")
                        {
                            System.IO.File.Delete(imageDetails.FilePath);
                        }
                    }
                }

                //rename existing 01 images
                if (existing01Images.Count > 0)
                { 
                    for(int idx = 0; idx< existing01Images.Count; idx++)
                    {
                        ImageDetails imageDetails = (ImageDetails)existing01Images[idx];
                        string image01Path = imageDetails.FilePath;
                        string image01Region = imageDetails.Region ?? "None";
                        int? imageRegionCount = regionCount[image01Region];

                        string existingImageFilename = System.IO.Path.GetFileNameWithoutExtension(image01Path);
                        string existingImageFolderPath = System.IO.Path.GetDirectoryName(image01Path);
                        string existingImageExtension = System.IO.Path.GetExtension(image01Path);

                        string backupImagePath;
                        backupImagePath = existingImageFolderPath + "\\" + existingImageFilename.Substring(0, existingImageFilename.Length - 3) + "-" + ((imageRegionCount + 1).ToString()).PadLeft(2, "0".ToCharArray()[0]) + existingImageExtension;
                        
                        System.IO.File.Copy(image01Path, backupImagePath);
                        System.IO.File.Delete(image01Path);
                    }
                }

                // ** create the new image

                // calculate the destination
                string destinationPath;
                if (ignoreRegion == "True" || region == "None")
                {
                    destinationPath = System.IO.Directory.GetCurrentDirectory() + "\\Images\\" + gameTarget.Platform + "\\" + imageType + "\\" + gameTarget.Title + "-01.png"; ;
                }
                else
                {
                    destinationPath = System.IO.Directory.GetCurrentDirectory() + "\\Images\\" + gameTarget.Platform + "\\" + imageType + "\\" + region + "\\" + gameTarget.Title + "-01.png"; ;
                }

                string filename = System.IO.Path.GetFileNameWithoutExtension(destinationPath);
                string folder = System.IO.Path.GetDirectoryName(destinationPath);

                //create folders if required
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                }

                //output the png
                BitmapSource bs = Clipboard.GetImage();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)bs));
                using (FileStream stream = new FileStream(destinationPath, FileMode.Create))
                encoder.Save(stream);

                //delete any cache images
                string cacheFolderPath = System.IO.Directory.GetCurrentDirectory() + "\\Images\\Cache-LB\\";
                string[] cacheImages = System.IO.Directory.GetFiles(cacheFolderPath, "*" + gameTarget.Id + "*");
                for(int idx=0; idx<cacheImages.Length; idx++)
                {
                    string cacheImagePath = cacheImages[idx];
                    System.IO.File.Delete(cacheImagePath);
                }

                //refresh the data - the UI may still not show the newly added image if deleteOtherImages is not set
                PluginHelper.LaunchBoxMainViewModel.RefreshData(); 
                this.Close();
            }
            catch
            {
                MessageBox.Show("Sorry, an error occurred trying to add the image.");
                this.Close();
                return;
            }
        }

        public SubMenu(IGame game)
        {
            InitializeComponent();
            gameTarget = game;

            if (!Clipboard.ContainsImage())
            {
                MessageBox.Show("Clipboard does not contain an image.");
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
   
        }
    }
}
