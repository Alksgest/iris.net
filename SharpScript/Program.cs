 using SharpScript.Lexer;

 var tokenizer = new Tokenizer();
 
 var tokens = tokenizer.Process("const a = 5; let s;");

 foreach (var token in tokens)
 {
     Console.WriteLine($"{token.Type.ToString()}: {token.Value}");
 }

 var parser = new Parser(tokens);
 var tree = parser.ParseProgram();

 Console.WriteLine(tree);