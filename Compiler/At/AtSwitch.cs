using System;

namespace Compiler.At {
	public class AtSwitch : AtCommand {
		private string target = null;

		public AtSwitch(string name, params string[] args) {
			if (args.Length > 0) {
				target = args[0];
			}
		}

		public override string Run(DialogueFile file, DialogueLine line) {
			if (target == null) {
				DialogueCompiler.Instance.Error(line, "No chapter specified");
				return null;
			}

			line.Options.SetSet("__sdtdc_ch", target);
			return "";
		}
	}
}

