using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using System.IO;
using System.Collections.Generic;

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

		private LineOptions initialSettings = new LineOptions();

		public static DialogueCompiler Instance { get; private set; }

		private IList<DialogueFile> files = new List<DialogueFile>();
		private TextWriter Out;
		private bool hadError = false;

		public DialogueCompiler(Options opts) {
			if (Instance == null) {
				Instance = this;
			}

			BasePath = Path.GetDirectoryName(Path.GetFullPath(opts.InputFile));

			ImportFile(DialogueFile.Open(opts.InputFile));

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
			if (hadError) {
				return;
			}

			if (files[0].ChapterName != null) {
				initialSettings["__sdtc_ch"] = files[0].ChapterName;
			}

			EmitPreamble();

			foreach (var f in files) {
				foreach (var line in f.Lines) {
					Out.WriteLine("{2}", line.File.FileName, line.LineNumber, line);
				}
			}
		}

		void EmitPreamble() {
			if (DialogueName != null) {
				Out.WriteLine(new DialogueLine("dialogue_name", DialogueName, null));
			}
			if (initialSettings.Count > 0) {
				Out.WriteLine(new DialogueLine("initial_settings", initialSettings));
			}
		}

		public void ImportFile(DialogueFile file) {
			file.Parse();
			files.Add(file);
		}

		public bool SetInitialValue(string key, object value) {
			if (initialSettings.ContainsKey(key) && !initialSettings[key].Equals(value)) {
				return true;
			}

			initialSettings[key] = value;
			return false;
		}

		public IList<string> SetInitialSettings(IDictionary<string, object> values) {
			return SetInitialSettingsInner(values).ToList();
		}

		private IEnumerable<string> SetInitialSettingsInner(IDictionary<string, object> values) {
			foreach (var val in values) {
				if (SetInitialValue(val.Key, val.Value)) {
					yield return val.Key;
				}
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

