Freelancer Mod Studio is a well-polished and powerful IDE for experienced as well as beginning modders. You can easily create and edit your mods by using the **visual ini/bini-editor** with **unlimited undo/redo**, **powerful copy/cut/paste**, **multi-editing** and **3D model view** support. The implemented **3D system editor** shows **real Freelancer models** and supports **position/rotation/scale manipulation** inside the editor. It is currently the best tool around to build the universe of your dreams.


&lt;hr&gt;



![http://img27.imageshack.us/img27/2803/c3op.jpg](http://img27.imageshack.us/img27/2803/c3op.jpg)
![http://s9.postimage.org/tnzcel78v/showcase2.png](http://s9.postimage.org/tnzcel78v/showcase2.png)
![http://img547.imageshack.us/img547/3218/iawc.jpg](http://img547.imageshack.us/img547/3218/iawc.jpg)
![http://img607.imageshack.us/img607/269/li01models.png](http://img607.imageshack.us/img607/269/li01models.png)
![http://img204.imageshack.us/img204/8100/li01full.png](http://img204.imageshack.us/img204/8100/li01full.png)
![http://img844.imageshack.us/img844/5976/li01universe.jpg](http://img844.imageshack.us/img844/5976/li01universe.jpg)
![http://img33.imageshack.us/img33/3493/unbenannt1nd.jpg](http://img33.imageshack.us/img33/3493/unbenannt1nd.jpg)
![http://img132.imageshack.us/img132/5527/teq7.jpg](http://img132.imageshack.us/img132/5527/teq7.jpg)



&lt;hr&gt;



**Q: Is it finished?**

Yes, the last version is considered to be the final one. Unfortunately there was not enough time due to the lack of any additional contributors to develop an all-in-one modding solution. However the existing functionality has been polished very well and mostly even exceeds that of the original vision. [Check it out](http://code.google.com/p/freelancermodstudio/downloads/list?can=3)!

**Q: Where can I ask questions, suggest features and report issues?**

If you have an issue please file an issue report [here](http://code.google.com/p/freelancermodstudio/issues). For everything else use the [official thread](http://the-starport.net/freelancer/forum/viewtopic.php?topic_id=2174).

**Q: Any known issues?**

  1. Saving files with ini-entries which require custom order within the same ini-section are not yet supported. This includes the following files:
    * Mission Encounters (missions\encounters\`*`.ini)
    * Missions (missions\`*`\m`*`.ini)
    * Solar Asteroids (solar\asteroids\`*`.ini)
    * Solar Nebulae (solar\nebula\`*`.ini)
  1. All ini sections (blocks) or entries (options) with invalid names will be ignored and therefore are not written on save. See the shipped [Template.xml file](http://freelancermodstudio.googlecode.com/svn/trunk/FreelancerModStudio/Template.xml) for a complete list. However you can easily edit this file which is located in the same directory as the .exe to change the behavior. This will also allow you to edit and work with them just like the normal ones.

**Q: Where can I see the changelog? Are there any critical changes?**

You can check out the [changelog](Changelog.md) under the wiki section. Critical changes are only related to Input/Output changes like changing the template of ini files resulting in no longer writing some sections (blocks) or entries (options) or adding support of some. Those can be seen [here](IOChanges.md).

**Q: What are the mouse/key bindings for the 3D editor?**
| **Action** | **Binding** |
|:-----------|:------------|
| Select nearest object | Click left mouse button |
| Select farthest object | Click left mouse button + SHIFT |
| Toggle object selection | Hold CTRL while selecting nearest/farthest object |
| Rotate camera | Hold right mouse button |
| Rotate camera (around point) | Hold left mouse button + ALT |
| Zoom camera (smooth) | Hold right mouse button + ALT |
| Zoom camera (fast) | Scroll mouse-wheel |
| Pan camera | Hold right mouse button + SHIFT |
|            | Hold middle mouse button |
| Look-at click point | Double-click right mouse button |
| Reset camera | Double-click right mouse button + SHIFT |
|            | Double-click middle mouse button |
| Move camera forward | W (faster: + SHIFT) |
| Move camera backward | S (faster: + SHIFT) |
| Move camera left | A (faster: + SHIFT) |
| Move camera right | D (faster: + SHIFT) |
| Move camera up | SPACE (faster: + SHIFT) |
| Move camera down | E (faster: + SHIFT) |
| Move object up | CAPSLOCK + W (single unit: + SHIFT) |
| Move object down | CAPSLOCK + S (single unit: + SHIFT) |
| Move object left | CAPSLOCK + A (single unit: + SHIFT) |
| Move object right | CAPSLOCK + D (single unit: + SHIFT) |
| Focus selected object | F           |
| Look-at selected object| SHIFT + F   |
| Track selected object | T           |

Those key bindings can be used after clicking with the mouse into the 3D editor window. They work additionally to the global shortcuts seen under the main menu.

**Q: Why are the models not showing up in the 3D editor for me?**

This is where Freelancer Mod Studio thinks the model files are located:

First Freelancer Mod Studio tries to find the solararch.ini file relative to your opened system/universe file, meaning if the system file is located at "C:\FL\DATA\UNIVERSE\Li01\Li01.ini" it would think the solararch file is located at "C:\FL\DATA\SOLAR\solararch.ini" (case insensitive). If it can't find it it will look for the file relative to the default Freelancer DATA path specified in the application settings. If Freelancer Mod Studio still can't find it it asks you for its location.

Now after the solararch file was found it remembers the DATA path where the solararch file is located ("C:\FL\DATA"). As all model paths are being defined relative to the DATA path it then combines the DATA path with the specific model path. For example jumpgate in solararch.ini has DA\_archetype = "solar\dockable\jump\_gateL.cmp" so it is being loaded from "C:\FL\DATA\SOLAR\DOCKABLE\jump\_gateL.cmp"

Also remember to enable model mode via _View -> Show models_.

**Q: How can I edit position/rotation/scale of objects?**

Either use the property window to set absolute values of all selected objects or use the position/rotation/scale manipulation handles via _View -> Position handles_, _View -> Rotation handles_ or _View -> Scale handles_. You can use the object move keys to fine tune positions down to a single unit (see key bindings above in the FAQ).

**Q: How does object tracking work?**

Object tracking shows info of your selected and tracked object like distance and angle between both. This can be incredibly helpful to correctly position and rotate many system objects like trade lanes, docking rings, docking fixtures, buoys and jump gates. To track the currently selected object use _View -> Track selected_ or its keyboard shortcut. To no longer track the selected object use _View -> Track selected_ again.

**Q: Do you have some secret workflow tricks?**

Sure, here are some things which you might not know to improve your workflow:

  * Hide by object type
    1. Sort by type by clicking on the _Type_ column in the ini-editor
    1. Select all objects of the same type which are now below each other
    1. _View -> Change visibility_
  * Automatically fit column size
    1. Double click on row separator right of the column in the ini-editor
  * Rotate object towards another object
    1. Select object A
    1. _View -> Track selected_
    1. Select object B
    1. Set rotation of object B so that it points towards object A by comparing the angles info of the tracked object (top-left in 3D editor window)
  * Set object distance from another object
    1. Rotate object B towards object A (see trick above)
    1. _View -> Position handles_
    1. Move object B away/towards object A using manipulator which points towards object A (should be the blue axis-z one) until distance info of tracked object has your preferred value
    1. Set rotation of object B back to original values
  * Select small zone through large zone
    * If the edge of the small zone is in front of the edge of the large zone you can select the nearest object (see mouse bindings above in the FAQ)
    * If it is the other way round you can select the farthest object (see mouse bindings above in the FAQ)
    * If there are additional zones in your way you can also select them and hide them using _View -> Change visibility_ before attempting to select
  * Reorder section order
    * Option A:
      1. Make sure that objects in the ini-editor are sorted by ID by clicking on the _#_ column
      1. Drag selected objects in the ini-editor into another place
    * Option B:
      1. Cut selected objects to the clipboard: _Edit -> Cut_
      1. Select object which should be just on top of where you want to move them to
      1. Paste selected objects from the clipboard: _Edit -> Paste_
  * Create pre-filled objects from templates
    1. Create a new system file which contains the pre-filled objects
    1. Select the objects you need
    1. _Edit -> Copy_
    1. Switch to your current system
    1. _Edit -> Paste_
  * Open system from universe file
    1. Select a system
    1. Click on selected system
    1. Click on the menu with the name of the system file which just appeared
  * Pan camera along axis
    1. Click on any faces of the orientation cube in the bottom right corner of the 3D editor to orientate the camera
    1. Pan camera (see mouse bindings above in the FAQ)
  * Offset object position along axis
    1. Click on any faces of the orientation cube in the bottom right corner of the 3D editor to orientate the camera
    1. Move object (see key bindings above in the FAQ)
  * Flip camera orientation
    1. Double click on any faces of the orientation cube in the bottom right corner of the 3D editor
  * Change walk-around/fly camera movement speed
    1. The longer you hold the movement keys the faster you get (see key bindings above in the FAQ)
  * Quickly add new objects
    * You can use the ALT menu shortcuts. Those are the underlined letters that you see when you click a menu while holding ALT. They depend on the current application language.
    * For English language:
      1. Hold ALT
      1. Press E (for _Edit_) followed by A (for _Add_)
      1. Press the first letter of the object you want to add
  * Remove an entry out of a list of entries of the selected objects
    1. Clear the value of the entry in the property grid
    1. Press RETURN
  * Open file with drag&drop
    1. Drag&drop the file from your OS into the title bar or the main menu
  * Reload current file
    1. _File -> Close_
    1. _File -> Open -> Most recent file_
  * Find out full path of a file
    1. See tooltip of document tab by moving the mouse over it
  * Copy full path of a file to clipboard
    1. Right click document tab of a file
    1. Select _Copy full path_
  * Open containing folder of a file
    1. Right click document tab of a file
    1. Select _Open containing folder_
  * Close all files
    1. _Window -> Close All Documents_
  * Close all files but one
    1. Right click document tab of a file
    1. Select _Close other_

**Q: I love this tool. Can I contribute to make it better?**

I will gladly accept any kind of help or like to hear your appreciation of the hard work :)