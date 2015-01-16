using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
	static string PapyrusCompilerName = "PapyrusCompiler - Original.exe";
	static TextWriter twOut = Console.Out;
	static TextWriter twError = Console.Error;

    static void Main(string[] args)
    {
        string[] configs = new string[2];
        configs[0] = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Advanced Papyrus.ini";
        configs[1] = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SublimePapyrus.ini";
        foreach (string config in configs) 
        {
            if (File.Exists(config))
            {
                twOut.WriteLine("Advanced Papyrus: Modifying arguments to include settings from " + config.Substring(config.LastIndexOf("\\") + 1) + "...");
                args = ModifyArguments(args, config);
                RunCompiler(args);
                return;
            }
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
			twError.WriteLine("ERROR: Unable to find \"" + PapyrusCompilerName + "\" in \"" + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\".");
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
