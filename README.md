
# LaunchBox - Paste Image Plugin
## Installation

 1. Close LaunchBox. 
 2. Download the LaunchBoxPasteImage.dll from the releases folder  
 3. Copy to LaunchBox/Plugins folder. 
 4. Open LaunchBox

## What is it?
This LaunchBox plugin introduces two context menu items for games: Search Cover and Paste Image.

> Search Cover

Simply opens a web browser window to google image search with the game platform and title as search terms.

> Paste Image

Save an image in the system clipboard to the game's platform images folder. The user is prompted to select what type of LaunchBox image should be saved.

## Settings
In the LaunchBox Tools menu "Paste Image Settings" has been added which will open a tool window where options affecting the way "Paste Image" behaves can be changed.

#### 

> Ignore Region(s)

When toggled off (default) the "Paste Image" command will save the image to the /Images/Platform/Region folder.

When toggled on the "Paste Image" command will save the image to the /Images/Platform folder

> Delete other images for the game Image Type selected

**WARNING**: Turning this option on will cause deletion of images for games the "Paste Image" command is used on.

When toggled off (default) the "Paste Image" command will add to the game's existing images for the LaunchBox Image Type selected.

When toggled on the "Paste Image" command will delete all other images of the same selected LaunchBox Image Type either within the game's region sub folder, or across **all regions** if the "Delete other images" options is toggled on.

Both options toggled on together results in the pasted image becoming the only image assigned for the selected Image Type and will result in forcing LaunchBox to use the image.

## Why was this plugin created
I wanted a quick way from the main LaunchBox UI to search for a game cover, and then force it as the cover within the UI.

The process of manually searching and then opening the game edit window and adding a downloaded image, and then deleting all the other images of the same type to force them to be used was tedious.

## TODO:

1. Add more Image Types
2. Allow region selection

