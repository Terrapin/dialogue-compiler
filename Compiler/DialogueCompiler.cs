using System;
using CommandLine;
using CommandLine.Text;
using System.IO;

namespace Compiler {
	public class DialogueCompiler {
		public class Options {
			[Value(1, Required=true)]
			public string InputFile { get; set; }

			[Option('o', "out", HelpText="Place output into <file>. Default: Standard output")]
			public string Output { get; set; }

			public string GetUsage() {
				HelpText t = new HelpText("SDT Dialogue Compiler");
				t.AddPreOptionsLine("Usage: compiler <main dialogue file> [-o <output file>]");
				t.AddDashesToOption = true;
				t.AddOptions(this);
				return t.ToString();
			}
		}

		public string BasePath { get; private set; }

		public static DialogueCompiler Instance { get; private set; }

		private DialogueFile MainFile;
		private TextWriter Out;

		public DialogueCompiler(Options opts) {
			if (Instance == null) {
				Instance = this;
			}

			BasePath = Path.GetDirectoryName(Path.GetFullPath(opts.InputFile));

			MainFile = new DialogueFile(opts.InputFile);

			if (opts.Output == null) {
				Out = Console.Out;
			} else {
				Out = new StreamWriter(opts.Output);
			}
		}

		public void Run() {
			InterpretFile();
		}

		internal void InterpretFile() {
			foreach (var line in MainFile.Lines) {
				Out.WriteLine("{2}", line.File.FileName, line.LineNumber, line);
			}
		}
	}
}

