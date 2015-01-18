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
        int import = -1;
        int skyrim = -1;
        int debug = -1;
        for (int i = 0; i < lines.Length; i++) 
        {
            if ((import == -1) && (lines[i].StartsWith("[Import]")))
            {
                import = i + 1;
            }
            else if ((skyrim == -1) && (lines[i].StartsWith("[Skyrim]")))
            {
                skyrim = i + 1;
            }
            else if ((debug == -1) && (lines[i].StartsWith("[Debug]")))
            {
                debug = i + 1;
            }
        }
        if (import >= 0) 
        {
            for (int i = import; i < lines.Length; i++) 
            {
                if (lines[i].StartsWith("path"))
                {
                    if (input.Length > 0)
                    {
                        input += ";";
                    }
                    input += "\"" + lines[i].Substring(lines[i].LastIndexOf("=") + 1) + "\"";
                }
                else if (lines[i].StartsWith("["))
                {
                    break;
                }
            }
        }
        if (skyrim >= 0)
        {
            for (int i = skyrim; i < lines.Length; i++) 
            {
                if (lines[i].StartsWith("scripts="))
                {
                    if (input.Length > 0)
                    {
                        input += ";";
                    }
                    input += "\"" + lines[i].Substring(8) + "\"";
                }
                else if (lines[i].StartsWith("output="))
                {
                    args[3] = "-o=\"" + lines[i].Substring(7) + "\"";
                }
                else if (lines[i].StartsWith("flags="))
                {
                    args[1] = "-f=" + lines[i].Substring(6);
                }
                else if (lines[i].StartsWith("["))
                {
                    break;
                }
            }
        }
        args[2] = "-i=" + input;
        if (debug >= 0)
        {
            List<string> arguments = new List<string>();
            arguments.AddRange(args);
            string[] validargs = new string[11] {"all", "a", "debug", "d", "optimize", "op", "quiet", "q", "noasm", "keepasm", "asmonly"};
            for (int i = debug; i < lines.Length; i++) 
            {
                if (lines[i].StartsWith("arg"))
                {
                    lines[i] = lines[i].Substring(lines[i].LastIndexOf("=") + 1);
                    foreach (string validarg in validargs) 
                    {
                        if (validarg.Equals(lines[i])) 
                        {
                            arguments.Add("-" + lines[i]);
                            break;
                        }
                    }
                }
                else if (lines[i].StartsWith("["))
                {
                    break;
                }
            }
            args = arguments.ToArray();
        }
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
        proc.EnableRaisingEvents = true;
        proc.OutputDataReceived += new DataReceivedEventHandler(OutputWriter);
        proc.ErrorDataReceived += new DataReceivedEventHandler(OutputWriter);
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit();
    }

    static void OutputWriter(object sender, DataReceivedEventArgs args)
    {
        if (args.Data != null)
        {
            twOut.WriteLine(args.Data);
        }
    }

    static void ErrorWriter(object sender, DataReceivedEventArgs args)
    {
        if (args.Data != null)
        {
            twError.WriteLine(args.Data);
        }
    }
}
