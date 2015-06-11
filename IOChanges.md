# Critical Changes #

Those are all changes related to file load/save.

The <font color='#0a0'>green color</font> means that sections (blocks) or entries (options) where added and the <font color='#a00'>red color</font> means that they were removed.

**Version 1.1.2**

General fixes:
  * Fixed saving Rtc Slider file (scripts\rtcslider.ini) and Petal Db file (petaldb.ini)

**Version 1.0**

General fixes:
  * <font color='#0a0'>Added options of ale effects into Effects (fx\<code>*</code>effects<code>*</code>.ini + fx\<code>*</code>\<code>*</code>ale.ini)</font>
  * Merged Effect Weapons (fx\weapons\`*`.ini) into Effects (fx\`*`effects`*`.ini + fx\`*`\`*`ale.ini)
  * Split Sounds (audio\`*`sounds.ini) into Audio Sounds (audio\`*`sounds.ini + audio\music.ini) and Audio Voices (audio\voices`*`.ini)
  * Fixed option order of many files

Removed support for unused FL files:
  * Game Concave (concave.ini)
  * Mission Ship Properties (missions\mshipprops.ini)
  * Interface Hud Target (interface\hud\hudtarget.ini)

Added or removed options or blocks:
  * System (universe\systems\`*`\`*`.ini)
    * <font color='#0a0'><code>[</code>SystemInfo] rpop_solar_detection=</font>
    * <font color='#a00'><code>[</code>SystemInfo] name=</font>
    * <font color='#a00'><code>[</code>EncounterParameters] faction=</font>
    * <font color='#0a0'><code>[</code>zone] drag_modifier=</font>
    * <font color='#a00'><code>[</code>zone] difficulty=</font>
    * <font color='#a00'><code>[</code>zone] zone_creation_distance=</font>
    * <font color='#a00'><code>[</code>zone] faction_weight=</font>
  * Audio Sound Configuration (audio\soundcfg.ini)
    * <font color='#a00'><code>[</code>CFG] default_loop_style=</font>
    * <font color='#a00'><code>[</code>CFG] default_reverb=</font>
  * Characters New Charakter (characters\newcharacter.ini)
    * <font color='#a00'><code>[</code>Pilot] thumb=</font>
  * Equipment (equipment\`*`equip`*`)
    * <font color='#0a0'><code>[</code>Power] hit_pts=</font>
    * <font color='#0a0'><code>[</code>ShieldGenerator] hp_type=</font>
    * <font color='#0a0'><code>[</code>ShieldGenerator] lootable=</font>
    * <font color='#0a0'><code>[</code>CloakingDevice] debris_type=</font>
  * Game Cameras (cameras.ini)
    * <font color='#0a0'><code>[</code>RearViewCamera]</font>
    * <font color='#0a0'><code>[</code>TurretCamera]</font>
  * Game Constants (constants.ini)
    * <font color='#a00'><code>[</code>AsteroidConsts]</font>
  * Interface Hud (interface\hud.ini)
    * <font color='#a00'><code>[</code>Receiver] object=</font>
  * Interface Nav Bar (interface\baseside\navbar.ini)
    * <font color='#a00'><code>[</code>NavBar] IDS_HOTSPOT_EXIT=</font>
    * <font color='#a00'><code>[</code>NavBar] IDS_NN_SHIP_VIEW=</font>
    * <font color='#a00'><code>[</code>NavBar] IDS_PACKAGE_ONE=</font>
    * <font color='#a00'><code>[</code>NavBar] IDS_PACKAGE_TWO=</font>
    * <font color='#a00'><code>[</code>NavBar] IDS_PACKAGE_THREE=</font>
    * <font color='#a00'><code>[</code>NavBar] Palace=</font>
  * Mission Bases (missions\mbases.ini)
    * <font color='#a00'><code>[</code>BaseFaction] offers_missions=</font>
  * Mission Encounters (missions\encounters\`*`.ini)
    * <font color='#a00'><code>[</code>EncounterFormation] explicit_group=</font>
    * <font color='#a00'><code>[</code>EncounterFormation] feeling_to_formation=</font>
    * <font color='#a00'><code>[</code>EncounterFormation] formation=</font>
    * <font color='#a00'><code>[</code>EncounterFormation] longevity=</font>
    * <font color='#a00'><code>[</code>EncounterFormation] ship_by_npc_arch=</font>
  * Mission News (missions\news.ini)
    * <font color='#a00'><code>[</code>NewsItem] audio=</font>
  * Solar Asteroids (solar\asteroids\`*`.ini)
    * <font color='#a00'><code>[</code>Shape]</font>
    * <font color='#a00'><code>[</code>Texture] name=</font>
  * Solar Blackholes (solar\blackhole\`*`.ini)
    * <font color='#a00'><code>[</code>CloseChunks]</font>
    * <font color='#a00'><code>[</code>FadeChunks]</font>
    * <font color='#a00'><code>[</code>Sound]</font>