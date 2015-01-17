using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
	static string PapyrusCompilerName = "PapyrusCompiler - Original.exe";
	static TextWriter twOut = Console.Out;
	static TextWriter twError = Console.Error;
    static int minimumArgumentCount = 4;

    static void Main(string[] args)
    {
        if (args.Length >= minimumArgumentCount) 
        {
            string[] configs = new string[2];
            configs[0] = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Advanced Papyrus.ini";
            configs[1] = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SublimePapyrus.ini";
            foreach (string config in configs) 
            {
                if (File.Exists(config))
                {
                    twError.WriteLine("Advanced Papyrus: Modifying arguments to include settings from " + config.Substring(config.LastIndexOf("\\") + 1) + "...");
                    args = ModifyArguments(args, config);
                    for (int i = 1; i < args.Length; i++) 
                    {
                        if (args[i].Substring(3).Equals(""))
                        {
                            twError.WriteLine("Advanced Papyrus: ERROR! Encountered an empty argument. Please check the contents of \"" + config + "\"!");
                            return;
                        }
                    }
                    RunCompiler(args);
                    return;
                }
            }
        }
        else
        {
            twError.WriteLine("Advanced Papyrus: ERROR! Expecting at least " + minimumArgumentCount + " arguments, but received only " + args.Length + "!");
            return;
        }
		twOut.WriteLine("Advanced Papyrus: Passing unmodified arguments through to compiler...");
		RunCompiler(args);
    }

    static string[] ModifyArguments(string[] args, string filepath)
    {
    	string input = "";
	 	string[] lines = File.ReadAllLines(filepath);
	 	for (int i = 0; i < lines.Length; i++) 
	 	{
	 		if (lines[i].Contains("[Import]"))
 			{
 				for (int j = i + 1; j < lines.Length; j++) 
 				{
 					if (lines[j].StartsWith("path"))
 					{
 						if (input.Length > 0)
 						{
 							input += ";";
 						}
 						input += "\"" + lines[j].Substring(lines[j].LastIndexOf("=") + 1) + "\"";
 					}
 					else if (lines[j].StartsWith("["))
 					{
 						break;
 					}
 				}
 				break;
 			}
	 	}
 		for (int i = 0; i < lines.Length; i++)
 		{
 			if (lines[i].Contains("[Skyrim]")) 
 			{
 				for (int j = i + 1; j < lines.Length; j++) 
 				{
 					if (lines[j].StartsWith("scripts="))
 					{
 						if (input.Length > 0)
 						{
 							input += ";";
 						}
 						input += "\"" + lines[j].Substring(8) + "\"";
 					}
 					else if (lines[j].StartsWith("output="))
 					{
 						args[3] = "-o=\"" + lines[j].Substring(7) + "\"";
 					}
 					else if (lines[j].StartsWith("flags="))
 					{
 						args[1] = "-f=" + lines[j].Substring(6);
 					}
 					else if (lines[j].StartsWith("["))
 					{
 						break;
 					}
 				}
 				break;
 			}
 		}
 		args[2] = "-i=" + input;
 		return args;
    }

    static void RunCompiler(string[] args)
    {
    	string arguments = "";
    	foreach (string arg in args) 
    	{
    		arguments += arg + " ";
    	}
    	string PapyrusCompilerEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + PapyrusCompilerName;
		if (!File.Exists(PapyrusCompilerEXE)) 
		{
			twError.WriteLine("Advanced Papyrus: ERROR! Unable to find \"" + PapyrusCompilerName + "\" in \"" + PapyrusCompilerEXE.Substring(PapyrusCompilerEXE.LastIndexOf("\\") + 1) + "\"!");
			return;
		}
    	var proc = new Process {
        	StartInfo = new ProcessStartInfo {
        		FileName = PapyrusCompilerEXE,
        		Arguments = arguments,
        		UseShellExecute = false,
        		RedirectStandardOutput = true,
        		RedirectStandardError = true,
        		CreateNoWindow = true
        	}
        };
        proc.Start();
        twOut.WriteLine(proc.StandardOutput.ReadLine());
        while((!proc.StandardOutput.EndOfStream) || (!proc.StandardError.EndOfStream)) {
        	if (!proc.StandardOutput.EndOfStream) 
        	{
        		twOut.WriteLine(proc.StandardOutput.ReadLine());
        	}
        	if (!proc.StandardError.EndOfStream) 
        	{
        		twError.WriteLine(proc.StandardError.ReadLine());
        	}
        }
    }
}
