using System;
using System.IO;
using System.Collections.Generic;

namespace Compiler {
	public class DialogueFile {
		private TextReader file;
		private int lineNumber;
		private IList<DialogueLine> contents;

		public string ChapterName { get; set; }

		public IEnumerable<DialogueLine> Lines {
			get {
				Parse();
				return contents;
			}
		}

		public string FileName { get { return Path.GetFileNameWithoutExtension(FilePath); } }

		public string FilePath { get; private set; }

		public string FileNameWithExt { get { return Path.GetFileName(FilePath); } }

		public static readonly DialogueFile NullFile = new DialogueFile();
		private static Dictionary<string, DialogueFile> cache = new Dictionary<string, DialogueFile>();

		private DialogueFile() {
			lineNumber = 0;
			contents = new List<DialogueLine>().AsReadOnly();
			FilePath = "{Null file}";
		}

		private DialogueFile(string path) {
			lineNumber = 0;

			FilePath = path;
			file = new StreamReader(path);
		}

		public static DialogueFile Open(string path) {
			var f = path = Path.Combine(DialogueCompiler.Instance.BasePath, path);
			if (!File.Exists(f)) {
				f = path + ".txt";
				if (!File.Exists(f)) {
					f = path + ".dlg";
					if (!File.Exists(f)) {
						f = path + ".ch";
					}
				}
			}

			if (!cache.ContainsKey(f)) {
				cache[f] = new DialogueFile(f);
			}
			
			return cache[f];
		}

		public void AddLines(IEnumerable<DialogueLine> lines) {
			foreach (var line in lines) {
				contents.Add(line);
			}
		}

		public void Parse() {
			if (contents != null) {
				return;
			}

			contents = new List<DialogueLine>();

			var line = file.ReadLine();
			while (line != null) {
				lineNumber += 1;

				line = line.Trim();
				if (line.Length > 0) {
					var dlgLine = new DialogueLine(line, this, lineNumber); 
					if (dlgLine.InterpretAtSigns()) {
						contents.Add(dlgLine);
					}
				}

				line = file.ReadLine();
			}

			if (ChapterName != null) {
				foreach (var l in Lines) {
					l.Options.SetCheck("__sdtc_ch", ChapterName);
				}
			}
		}
	}
}

