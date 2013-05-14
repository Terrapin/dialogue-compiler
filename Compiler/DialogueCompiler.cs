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

		private DialogFile MainFile;
		private TextWriter Out;

		public DialogueCompiler(Options opts) {
			MainFile = new DialogFile(opts.InputFile);

			if (opts.Output == null) {
				Out = Console.Out;
			} else {
				Out = new StreamWriter(opts.Output);
			}
		}

		public void Run() {
			InterpretFile(MainFile);
		}

		internal void InterpretFile(DialogFile reader) {
			var line = reader.ReadLine();

			while (line != null) {
				line = line.Trim();
				if (line.Length > 0) {
					var t = new DialogLine(line);

					Out.WriteLine(t);
				}

				line = reader.ReadLine();
			}
		}
	}
}

