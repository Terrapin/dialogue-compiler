using System;
using CommandLine;
using System.IO;

namespace Compiler {
	public class DialogueCompiler {
		public class Options {
			[Value(1)]
			public string InputFile { get; set; }

			[Option('o', "out")]
			public string Output { get; set; }
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
				Out.WriteLine("{2}", line.FileName, line.LineNumber, line);
			}
		}
	}
}

