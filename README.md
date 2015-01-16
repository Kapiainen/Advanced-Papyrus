# Advanced-Papyrus
A wrapper program that allows for more advanced use of the Papyrus compiler from within Creation Kit.

##Installing
- Copy *Advanced Papyrus.exe* to *\Skyrim\Papyrus Compiler\*.
- Rename *PapyrusCompiler.exe* to *PapyrusCompiler - Original.exe*.
- Rename *Advanced Papyrus.exe* to *PapyrusCompiler.exe*.

See the Features section for information on how to use the various features of Advanced Papyrus.

##Features
####Argument modification
Arguments sent by Creation Kit to the Papyrus compiler can be modified by copying *Advanced Papyrus.ini* to *\Skyrim\Papyrus Compiler*`and modifying it to suit your needs.

```
[Skyrim]
scripts=
output=
flags=TESV_Papyrus_Flags.flg
[Import]
path0=
path1=
path2=
.
.
.
pathN=
```

All paths specified in the INI file should be absolute.
The *scripts* option should point at your copy of the vanilla Skyrim source files (.psc).
The *output* option should point at the folder you want the compiled scripts (.pex) to be placed. 
The *path* options can point at folders containing source files.

Advanced Papyrus will read through the *Import* section first and then the *Skyrim* section. *scripts* and *path* options will be parsed into a single *-i=* argument that is passed to the Papyrus compiler so that the argument looks something like this:

```
-i=path0;path1;path2;...;pathN;scripts
```

So if you have SKSE's source files in a separate folder and put its path into option *pathN*, then the Papyrus compiler will use SKSE's sources instead of the Skyrim's source files.

If you use [SublimePapyrus](https://github.com/Kapiainen/SublimePapyrus), then you might have noticed that the contents of *Advanced Papyrus.ini* look like the contents of SublimePapyrus' INI file. That is because they are the same and you can actually force Advanced Papyrus to use your copy of *SublimePapyrus.ini* by not copying *Advanced Papyrus.ini* into *\Skyrim\Papyrus Compiler*. This way you can have the same settings for Advanced Papyrus and SublimePapyrus.

##Mod Organizer
Advanced Papyrus is useful when running [Creation Kit](http://www.creationkit.com/Main_Page) via [Mod Organizer](http://www.nexusmods.com/skyrim/mods/1334/) since you don't need to modify the Papyrus compiler and you can keep all of your script source files separated. You should modify *Advanced Papyrus.ini* (or *SublimePapyrus.ini*, if you use SublimePapyrus) so that the *output* option points to the *\Mod Organizer\overwrite\Scripts*

```
output=PathToModOrganizer\overwrite\scripts\source
```