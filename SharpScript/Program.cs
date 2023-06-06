 using SharpScript.Lexer;

 var tokenizer = new Tokenizer();
 
 var tokens = tokenizer.Process("const a = 5; const b = a; let c; c = a;"); // const a = 5; let b; b = a; a = 3; // a(1, 2, 3); 

 foreach (var token in tokens)
 {
     Console.WriteLine($"{token.Type.ToString()}: {token.Value}");
 }

 var parser = new Parser(tokens);
 var tree = parser.ParseTokens();

 Console.WriteLine(tree);
 
 var evaluator = new Evaluator();
 evaluator.Evaluate(tree);