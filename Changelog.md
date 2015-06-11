**Version 1.2**
  * Added support to read/write comments from/to ini files
  * Added object move keys to fine-tune position values
  * Added support to fallback to specified Freelancer DATA directory in case DATA path could not be found relative to opened file
  * Added document tab context menu to easily copy full file path to clipboard and open containing folder
  * Improved 3D editor walk-around/fly mode by increasing move speed by 5x when holding SHIFT
  * Fixed model scale and scale manipulation of zone rings
  * Fixed camera rotation while moving in walk-around/fly mode
  * Fixed selected manipulation mode after 3D editor was closed and then re-opened
  * Fixed hiding 3D editor if no document could be loaded on application start

**Version 1.1.2**
  * Improved recent file handling
    * Fixed adding file to recent files when it is closed instead of opened/saved
    * Fixed recent file shortcut key
  * Improved look-at command
    * Added command to main menu
    * Reverted look-at position of mouse command back to click point instead of center of clicked object
  * Fixed undo if sections which can exist multiple times and sections which can only exist once were added at the same time due to copy/paste
  * Fixed camera reset not ignoring selection/manipulators visuals
  * Fixed an issue with global shortcut handling (regression in 1.1.1)
  * Fixed window layout issue if recent files can't be opened on application start
  * Fixed crash when docking property window or 3D editor into document area
  * [Fixed writing two ini files](IOChanges.md)

**Version 1.1.1**
  * Improved key shortcuts
    * Added special key shortcuts to the 3D editor
    * Improved accessibility of global key shortcuts on property / 3D editor windows
  * Improved walk-around/fly mode in the 3D editor
  * Improved look-at command adding animation and preventing the user from accidently clicking the manipulators
  * Improved table editor adding a separate row color for completely new sections (blocks)
  * Improved block visibility enabling it after changing block from invalid to valid archetype
  * Fixed crash when switching manipulation mode during active manipulation
  * Fixed radius-scale manipulation of zone paths
  * Fixed manipulation if camera look-at position was not on manipulated object
  * Fixed file names under Window menu (regression in 1.0.3)
  * Fixed full screen mode on Windows 7+

**Version 1.1**
  * Added 3D editor object manipulation
    * Translate, rotate and scale
    * Supported on system and universe files
  * Added faster screen space line implementation
  * Added cut/copy/paste entries (options) to table editor right click context menu
  * Improved 3D editor object selection greatly
    * Prioritize selection by type: system objects before paths before zones
  * Improved tracked object line display:
    * Draw it in front of all other objects
  * Improved spherical/elliptical shape model quality
    * Helps align stuff along huge spherical/elliptical objects, however slightly reduces performance
  * Improved camera position of look-at command (double right click on model)
  * Fixed orientation of 3D editor orientation cube

**Version 1.0.6**
  * Added 3D editor model tracking via View->Track selected, displaying distance and angle between selected and tracked model
  * Added selection toggling inside 3D editor by holding CTRL while clicking on a model
  * Fixed using animations when changing system positions in the universe display of the 3D editor

**Version 1.0.5.1**
  * Fixed crash when focus zooming camera to hidden model
  * Fixed minor visual glitches when using higher DPI on the OS

**Version 1.0.5**
  * Improved camera
    * Camera now zooms to fit the selected object into view on "Focus selected"
    * Camera now zooms to fit all objects into view when opening files, switching between files or resetting the camera
  * Improved single model display
    * Added model display in asteroid archetype, ship archetype, equipment and explosions files
    * Added selection box
    * Camera automatically fits object into view
    * Fixed flicker when changing the model
  * Improved performance slighty when modifying object properties with opened 3D editor
  * Fixed display of a few misc models
  * Fixed a 3D editor issue when opening a file with closed 3D editor

**Version 1.0.4**
  * Fixed selection box size of models
  * Fixed sometimes not being able to select the nearest/farthest object
  * Fixed model not being removed when set to invalid archetype after version 1.0.3

**Version 1.0.3**
  * Improved model load speed by caching the models
  * Fixed a crash in the universe 3D editor
  * Fixed memory leak in culture changer

**Version 1.0.2**
  * Added support for legend icons and 3d models in solar archetype files
  * Improved adding/pasting sections (blocks) by making them visible automatically
  * Fixed rotation for cylindrical zones (not paths) and zone rings
  * Fixed selection box not updating when changing object transform values
  * Fixed updating object models when the changed object had the same object type
  * Fixed detecting some exclusion zones (property\_flags | 0x10000)
  * Fixed crash when file could not be saved (for example being read-only) by showing a warning instead

**Version 1.0.1**
  * Improved utf parsing speed
  * Fixed rotation of objects/zones/paths with more than one defined axis value (please recheck your files)
  * Fixed parsing cmp models with single LoD
  * Fixed display of zone cylinders and rings after version 1.0
  * Fixed not being able to save a file using "Save" after version 1.0
  * Fixed 3D editor camera flipping glitch when zooming in too fast
  * Fixed 3D editor camera wiggle glitch when zooming in too far

**Version 1.0**
  * Added real model mode (CTRL+1 to switch between shapes and models)
  * Added possibility to double click on the view cube to switch to the other side
  * Added support to automatically rename entries (options) to prevent removing them from vanilla bini files
  * Improved 3D editor:
    * Improved performance noticeably
    * Improved mouse bindings
    * Improved handling by preventing to move to newly selected object (use "Focus selected" to manually do that)
    * Fixed selection issue while having objects sorted by name or type
    * Fixed universe connections in special cases
  * Improved shape models:
    * Added type support for tradelane paths (cyan) and changed trade paths color to pink
    * Improved display of rotatable shapes by making them pyramids
    * Improved display by making all shapes a constant size
    * Fixed rotation of pyramid shapes in the 3D editor
    * Fixed parsing some trade paths and some exclusion zones
  * Improved ini editor:
    * Added support to also show the type of objects which are differently displayed in the 3D editor
    * Added new legend icons and rework the old ones
  * [Fixed writing some ini files based on JFLP](IOChanges.md)
  * Fixed reading/writing entries (options) without a value and without an equal sign (=)
  * Fixed showing correct save state after undo/redo
  * Fixed minor display issues of the updater
  * Fixed many small bugs and improved code

**Version 0.9.9 (Beta 1 and 2)**
  * Added legend icon for systems in universe display
  * Added different display color for trade path (blue) and patrol path (white)
  * Added setup option to associate .ini files with Freelancer Mod Studio
  * Fixed selection issue after sorting or removing objects
  * Fixed solid models not being drawn in front of emissive ones resulting in the emissive models to be invisible
  * Fixed model not being updated if archetype of object was changed
  * Fixed pyramid meshes being lit from the inverted direction
  * Fixed crash when dragging the 3D editor to the document pane
  * Fixed a few small bugs

**Version 0.9.8**
  * Added ability to focus the selected object by pressing CTRL+F
  * Added the following settings:
    * Save ini files without spaces around equal sign
    * Save ini files without empty lines between sections (blocks)
    * Show option sorted by category and/or alphabetical
    * Show option description
  * Improved object models by making them light illuminated again - its now also easier to identify the rotation
  * Improved 3D editor selection by not moving the camera to the target
  * Fixed some rotation issues
  * Fixed ring zone models
  * Fixed inverted gradient system connections in universe display
  * Fixed double click on a system in universe not showing the context menu
  * Fixed loading of language from settings

**Version 0.9.7**
  * Added ability to select the farthest object when pressing shift+left mouse button
  * Added flat surface model for system paths and universe connections
  * Improved systems models in universe
  * Improved brightness of zones
  * Fixed exclusion cylinders model
  * Fixed objects models getting too big by adding a maximum size
  * Fixed a few small bugs

**Version 0.9.6**
  * Improved and sped up object handling in the 3D editor
  * Improved paste code so that pasted blocks will be added under selected ones and if there are none selected at the end of the list
  * Improved 3D editor model size to have a minimum size and therefore prevent some objects from getting too small
  * Fixed issues where 3D editor and ini editor ids were not in sync and could crash the application using copy&paste
  * Fixed loading of some archetype files by comparing system archetypes case insensitive and using the last occurrence of duplicate entries (options)
  * Fixed native application image not being installed by installer and added an checkbox to the installer to make it optional

**Version 0.9.5.1**
  * Added small delay for tooltips
  * Fixed issue which prevented systems and universe from loading

**Version 0.9.5**
  * Added ability to load and save sections (blocks) in the original order
  * Added support for section (block) reordering using drag&drop
  * Added support to save entries (options) in a unified order used by Freelancer
  * Added octahedron mesh display for LightSource
  * Fixed loading/saving entries (options) which dont have a value (seperable = )
  * Fixed using the last entry (option) for a single occurrence entry (option) if there are multiple entries (options)
  * Fixed a crash when opening a system in the universe 3D editor when using the english language

**Version 0.9.2**
  * Added 3d system viewer
  * Added 3d universe viewer including display of travel connections using jumpgate (gray), jumphole (red), both (white), none (black) or a combination of those (gradient color)
  * Added system legend icons to table editor
  * Added possibility to hide/show objects in the system editor using the table editor
  * Improved table editor in general
  * Fixed incorrect display of document menu if there was no open file at startup
  * Fixed content and document null bug
  * Fixed bug with open system editor and no open document
  * Fixed minor display bugs
  * Fixed silent update display bug
  * Fixed undo/redo bug where block will not be shown again after redo

**Version 0.8.3**
  * Added support for Copy/Cut/Paste
  * Added support for Undo/Redo
  * Added support for to save/load the window layout
  * Fixed ini save bug where some line breaks were not written
  * Fixed minor bugs in dock panel suite

**Version 0.8.2**
  * Added global shortcuts depending on active content
  * Fixed editing of nested entries (options) (like faction under encounter)
  * Fixed displaying error messages on silent update
  * Fixed bug where modified color of block would not be changed if it was the last in the file

**Version 0.8.1**
  * Added support for freelancer.ini
  * Improved editing of multiple options in property window
  * Improved application startup speed
  * Fixed bug if section (block) was commented out
  * Fixed bug where semicolon (;) of comment would be shown at the end of the option

**Version 0.8**
  * First release