# Advanced Papyrus
A wrapper program that allows for more advanced use of the Papyrus compiler from within Creation Kit.

## Installing
- Copy *Advanced Papyrus.exe* to *"\Skyrim\Papyrus Compiler\"*.
- Rename *PapyrusCompiler.exe* to *PapyrusCompiler - Original.exe*.
- Rename *Advanced Papyrus.exe* to *PapyrusCompiler.exe*.

If you've installed Advanced Papyrus correctly, then you should see the output of the Papyrus compiler in Creation Kit start with:

```
Advanced Papyrus: MESSAGE
```

*MESSAGE* will be about passing through unmodified arguments or modifying arguments to take into account an INI file.

If you've forgotten to rename the old Papyrus compiler executable to *PapyrusCompiler - Original.exe*, then an error message will be printed in the output.

See the Features section for information on how to use the various features of Advanced Papyrus.

## Features
#### Argument modification
Arguments sent by Creation Kit to the Papyrus compiler can be modified by copying *Advanced Papyrus.ini* to *"\Skyrim\Papyrus Compiler"* and modifying it to suit your needs. If you don't copy *Advanced Papyrus.ini* or use SublimePapyrus (see below for more information), then arguments sent from Creation Kit will just pass through unmodified.

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

So your INI file might look something like this:
```
[Skyrim]
scripts=H:\Mod Organizer\Skyrim\mods\Creation Kit - 1 - Skyrim\Scripts\Source
output=H:\Mod Organizer\Skyrim\overwrite\Scripts
flags=TESV_Papyrus_Flags.flg
[Import]
path0=H:\Mod Organizer\Skyrim\overwrite\Scripts\Source
path1=H:\Mod Organizer\Skyrim\mods\FISS\scripts\source
path2=H:\Mod Organizer\Skyrim\mods\JContainers\scripts\source
path3=H:\Mod Organizer\Skyrim\mods\Net Immerse Override\scripts\source
path4=H:\Mod Organizer\Skyrim\mods\SkyUILib\scripts\source
path5=H:\Mod Organizer\Skyrim\mods\SkyUI SDK\scripts\source
path6=H:\Mod Organizer\Skyrim\mods\SKSE\Scripts\Source
path7=H:\Mod Organizer\Skyrim\mods\Creation Kit - 2 - Dawnguard\Scripts\Source
path8=H:\Mod Organizer\Skyrim\mods\Creation Kit - 3 - Hearthfire\Scripts\Source
path9=H:\Mod Organizer\Skyrim\mods\Creation Kit - 4 - Dragonborn\Scripts\Source
```

All paths specified in the INI file should be absolute.
The *scripts* option should point at your copy of the vanilla Skyrim source files (.psc).
The *output* option should point at the folder you want the compiled scripts (.pex) to be placed. 
The *path* options can point at folders containing source files.

Advanced Papyrus will read through the *Import* section first and then the *Skyrim* section. *scripts* and *path* options will be parsed into a single *"-i="* argument that is passed to the Papyrus compiler so that the argument looks something like this:

```
-i=path0;path1;path2;...;pathN;scripts
```

So if you have SKSE's source files in a separate folder and put its path into option *pathN*, then the Papyrus compiler will use SKSE's sources instead of the Skyrim's source files.

If you use [SublimePapyrus](https://github.com/Kapiainen/SublimePapyrus), then you might have noticed that the contents of *Advanced Papyrus.ini* look like the contents of SublimePapyrus' INI file. That is because they are the same and you can actually force Advanced Papyrus to use your copy of *SublimePapyrus.ini*, but only if it exists in *My Documents*, by not copying *Advanced Papyrus.ini* into *"\Skyrim\Papyrus Compiler"*. This way you can have the same settings for Advanced Papyrus and SublimePapyrus.

**Most people will not need to use any of the arguments listed below and the arguments should only be used if you know what you are doing!**

You can use arguments like *optimize*, *debug*, and *keepasm* which are intended for advanced users. These can be added to the INI file in a section labeled *Debug* and each argument should be added as the value to an option starting with *arg*:

```
[Debug]
arg0=debug
arg1=keepasm
.
.
.
argN=optimize
```

The full list of supported arguments is as follows:

```
// Argument = Description
all = Invokes the compiler against all psc files in the specified directory.
debug = Turns on compiler debugging, outputting dev information to the screen.
optimize = Turns on optimization of scripts.
quiet = Does not report progress or success (only failures).
noasm = Does not generate an assembly file and does not run the assembler.
keepasm = Keeps the assembly file after running the assembler.
asmonly = Generates an assembly file but does not run the assembler.
```


## Mod Organizer
Advanced Papyrus is useful when running [Creation Kit](http://www.creationkit.com/Main_Page) via [Mod Organizer](http://www.nexusmods.com/skyrim/mods/1334/) since you don't need to modify the Papyrus compiler and you can keep all of your script source files separated.

You should modify *Advanced Papyrus.ini* (or *SublimePapyrus.ini*, if you use SublimePapyrus) so that the *output* option points to the *"\Mod Organizer\overwrite\Scripts"*:

```
output=Path\To\Mod Organizer\overwrite\scripts
```

You should also have a *path* option that points to *"\Mod Organizer\Scripts\Source"*:
```
path0=Path\To\Mod Organizer\overwrite\scripts\source
```
