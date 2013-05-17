using System;
using System.Collections.Generic;
using System.IO;
using Compiler;

namespace Compiler.At {
	class AtInclude : AtStatement {
		private DialogueFile File;

		public AtInclude(DialogueLine param) {
			File = DialogueFile.Open(param.Content);
		}

		public override bool Run(DialogueFile file) {
			file.AddLines(File.Lines);
			return false;
		}
	}
}