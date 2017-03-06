import std.stdio, std.process, std.json, std.file, std.array;

int main(string[] args) {
	if (args.length >= 5) {
		// Arguments
		// 1 = File name or path to folder
		// 2 = Name of flags file
		// 3 = Input folders
		// 4 = Output folder
		string working_directory = getcwd();
		string compiler_path = working_directory ~ "\\Backup\\PapyrusCompiler.exe";
		if (exists(compiler_path)) {
			string settings_path = working_directory ~ "\\AdvancedPapyrus.json";
			if (exists(settings_path)) {
				writeln("Advanced Papyrus: Modifying arguments.");
				JSONValue settings = parseJSON(readFile(settings_path));
				// Flags file
				string flags_file = getStringSetting(settings, "flags");
				if (flags_file == null) {
					flags_file = args[2];
				}

				// Input folders
				string[] input_folders;
				auto input_folders_array = getArraySetting(settings, "input");
				if (input_folders_array.length == 0) {
					string input_folder_temp = getStringSetting(settings, "input");
					if (input_folder_temp != null) {
						input_folders.insertInPlace(0, input_folder_temp);
					}
				} else {
					for (int i = 0; i < input_folders_array.length; i++) {
						input_folders.insertInPlace(i, input_folders_array[i].str);
					}
				}
				if (input_folders.length == 0) {
					input_folders.insertInPlace(0, args[3]);
				}

				// Output folder
				string output_folder = getStringSetting(settings, "output");
				if (output_folder == null) {
					output_folder = args[4];
				}

				// Flags
				string[] flags;
				auto flags_array = getArraySetting(settings, "arguments");
				if (flags_array.length == 0) {

				} else {
					for (int i = 0; i < flags_array.length; i++) {
						flags.insertInPlace(i, flags_array[i].str);
					}
				}
				runCompiler(compiler_path, args[1], flags_file, input_folders, output_folder, flags);
			} else {
				writeln("Advanced Papyrus: Could not find \"AdvancedPapyrus.json\". Passing arguments to compiler in unmodified state.");
				runCompiler(compiler_path, args[1], args[2], [args[3]], args[4], null);
			}
		} else {
			writeln("Advanced Papyrus: \"" ~ compiler_path ~ "\" does not exist. Aborting compilation...");
		}
	} else if (args.length == 1) {
		string programVersion = "2.0.0";
		writeln("Advanced Papyrus - version " ~ programVersion ~ ".");
	} else {
		writeln("Expected at least four arguments (filename, flags file, input folders, and output folder).");
	}
	return 0;
}

int runCompiler(string compiler, string fileOrFolder, string flags, string[] inputFolders, string outputFolder, string[] arguments) {
	flags = "-f=" ~ flags;
	outputFolder = "-o=" ~ outputFolder;
	string inputs = "-i=";
	for (int i = 0; i < inputFolders.length; i++) {
		inputs = inputs ~ inputFolders[i];
		if (i < (inputFolders.length - 1)) {
			inputs = inputs ~ ";";
		}
	}
	string[] args = [compiler, fileOrFolder, flags, inputs, outputFolder];
	if (arguments != null) {
		foreach (string arg; arguments) {
			args.insertInPlace(args.length, arg);
		}
	}
	return wait(spawnProcess(args, std.stdio.stdin, std.stdio.stdout, std.stdio.stderr));
}

string readFile(string filepath) {
	if (exists(filepath)) {
		string contents = cast(string) read(filepath);
		if (contents != null) {
			return contents;
		}
	}
	return null;
}

bool settingExists(JSONValue object, string key) {
	int* key_in_json = cast(int*) (key in object);
	if (key_in_json != null) {
		return true;
	}
	return false;
}

string getStringSetting(JSONValue object, string key) {
	if (settingExists(object, key)) {
		if (object[key].type == JSON_TYPE.STRING) {
			return object[key].str;
		}
	}
	return null;
}

auto getArraySetting(JSONValue object, string key) {
	if (settingExists(object, key)) {
		if (object[key].type == JSON_TYPE.ARRAY) {
			return object[key].array;
		}
	}
	return null;
}
