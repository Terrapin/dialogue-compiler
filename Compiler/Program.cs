using System;
using CommandLine;

namespace Compiler {
	class MainClass {
		public static void Main(string[] args) {
			var opts = Parser.Default.ParseArguments<DialogueCompiler.Options>(args);

			DialogueCompiler compiler = new DialogueCompiler(opts.Value);
			compiler.Run();
		}
	}
}
