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

		public string DialogueName { get; set; }

		public string BasePath { get; private set; }

		public static DialogueCompiler Instance { get; private set; }

		private DialogueFile MainFile;
		private TextWriter Out;
		private bool hadError = false;

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
			MainFile.Parse();

			if (hadError) {
				return;
			}

			if (DialogueName != null) {
				Out.WriteLine(new DialogueLine("dialogue_name", DialogueName, null));
			}

			foreach (var line in MainFile.Lines) {
				Out.WriteLine("{2}", line.File.FileName, line.LineNumber, line);
			}
		}

		public void PrintWarning(DialogueLine source, string message, params string[] extras) {
			message = String.Format(message, extras);
			Console.Error.WriteLine("WARNING: {0} (at location {1}:{2})", message, source.File.FileNameWithExt, source.LineNumber);
		}

		public void Error(DialogueLine source, string message, params string[] extras) {
			message = String.Format(message, extras);
			Console.Error.WriteLine("ERROR: {0} (at location {1}:{2})", message, source.File.FileNameWithExt, source.LineNumber);
			hadError = true;
		}
	}
}

