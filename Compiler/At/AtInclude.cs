using System;
using System.Collections.Generic;
using System.IO;
using Compiler;

namespace At {
	class AtInclude : AtStatement {
		private DialogueFile File;

		public AtInclude(string param) {
			File = new DialogueFile(param);
		}

		public override void Run(DialogueFile file) {
			file.AddLines(File.Lines);
		}
	}
}