using Compilador.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Compilador.Clases
{
    /// <summary>
    /// This class performs the syntactic analysis on code.  This step is executed first in the compilation process.
    /// </summary>
    public class LALR
    {
        /// <summary>
        /// Indicates the number of terminal or nonterminal columns in the push-down automaton.
        /// This number must be correct.
        /// </summary>
        private const int NUMNONTERMINALS = 40, NUMTERMINALS = 55;
        /// <summary>
        /// Indicates the number of state rows in the push-down automaton.  This number must be correct.
        /// </summary>
        private const int NUMSTATES = 234;

        /// <summary>
        /// A char array of the terminal or nonterminal character representations used for the push-down automaton.
        /// </summary>
        private static char[] nonterminals, terminals;
        /// <summary>
        /// An array of the list of grammar rules.
        /// </summary>
        private static Rule[] rules;
        /// <summary>
        /// A two-dimensional table that contains the entries in the push-down automaton transition table.
        /// </summary>
        private static List<List<string>> LALRTable;
        /// <summary>
        /// A hashtable that allows quick searching of a terminal's or nonterminal's index by the char representation.
        /// </summary>
        private static Hashtable nonterminalIndexes, terminalIndexes;

        /// <summary>
        /// The error message generated (if present) in case of a syntax analysis failure.
        /// </summary>
        public static string errorMessage;


        /// <summary>
        /// Reads the push-down automaton transitions with terminal columns from disk into memory.
        /// </summary>
        /// <param name="path">The path of the CSV file.  The .csv extension is not required.</param>
        public static void ReadLALR1CSV(string path)
        {
            StreamReader read = new StreamReader(path + ".csv");
            string[] firstline = read.ReadLine().Split(',');
            terminals = new char[firstline.Length];
            terminalIndexes = new Hashtable(terminals.Length);
            for (int i = 0; i < firstline.Length; i++)
            {
                terminals[i] = firstline[i].ToCharArray()[0];
                terminalIndexes.Add(terminals[i], i);
            }
            string all = read.ReadToEnd();
            read.Close();
            LALRTable = new List<List<string>>(NUMSTATES);
            string[] lines = all.Replace("\r", "").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                List<string> tempList = new List<string>(NUMTERMINALS + NUMNONTERMINALS);
                tempList.AddRange(lines[i].Split(','));
                LALRTable.Add(tempList);
            }
        }

        /// <summary>
        /// Reads the push-down automaton transitions with nonterminal columns from disk into memory.
        /// </summary>
        /// <param name="path">The path of the CSV file.  The .csv extension is not required.</param>
        public static void ReadLALR2CSV(string path)
        {
            StreamReader read = new StreamReader(path + ".csv");
            string[] firstline = read.ReadLine().Split(',');
            nonterminals = new char[firstline.Length];
            nonterminalIndexes = new Hashtable(nonterminals.Length);
            for (int i = 1; i < firstline.Length; i++)
            {
                nonterminals[i] = firstline[i].ToCharArray()[0];
                nonterminalIndexes.Add(nonterminals[i], i);
            }
            string all = read.ReadToEnd();
            read.Close();
            string[] lines = all.Replace("\r", "").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                LALRTable[i].InsertRange(terminals.Length, lines[i].Split(','));
            }
        }

        /// <summary>
        /// Reads the list of grammar rules into memory from disk.
        /// </summary>
        /// <param name="path">The path of the CSV file.  The .csv extension is not required.</param>
        public static void ReadRulesCSV(string path)
        {
            StreamReader read = new StreamReader(path + ".csv");
            string all = read.ReadToEnd();
            read.Close();
            string[] lines = all.Replace("\r", "").Split('\n');
            rules = new Rule[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                rules[i] = new Rule();
                rules[i].left = line[0].ToCharArray()[0];
                string[] rightArray = line[1].Split(' ');
                rules[i].right = new char[rightArray.Length];
                for (int j = 0; j < rightArray.Length; j++)
                {
                    rules[i].right[j] = rightArray[j].ToCharArray()[0];
                }
            }
        }

        /// <summary>
        /// Performs a grammar analysis on input code.  This process is invoked before any other main compilation process.
        /// </summary>
        /// <param name="errorRecovery">Indicates whether or not try-catch error recovery is activated.
        /// -True: This is on when during a first/inital compilation.  If it doesn't work, it is turned off.
        /// -False: This is off after a failed compilation; this represents a second attempt.</param>
        /// <returns>Returns whether or not the syntactic analysis was successful.
        /// -True: Successful syntactic analysis.
        /// -False: Unrecoverable grammar errors were found.</returns>
        public static bool SyntaxAnalysis(bool errorRecovery)
        {
            //Initialization sequence
            errorMessage = "";
            List<Token> tokens = new List<Token>();
            Stack stack = new Stack(50);
            Semantic.Initialize(errorRecovery);
            stack.Push(0);
            Token token = LexicalAnalyzer.NextToken();
            Token tempToken = new Token(token.line, Lexeme.lexemes[Lexeme.lexemes.Count - 1]);
            tokens.Add(token);
            bool errorFound = false, needToken = false;
            Symbol.ResetSymbolTable();
            Semantic node;
            //Loop that iterates over all transitions in the push-down automaton.  Return statements are the only exit.
            while (true) 
            {
                int state = (int)stack.Peek();
                char symbol;
                if (errorFound)
                {
                    token = tempToken;
                    needToken = false;
                    errorFound = false;
                }
                else
                {
                    while (needToken)
                    {
                        token = LexicalAnalyzer.NextToken();
                        if ((token.symbol == '}') && (state == 65))
                        {
                            continue;
                        }
                        tokens.Add(token);
                        needToken = false;
                    }
                }
                while (true)
                {
                    symbol = token.symbol;
                    string move = LALRTable[state][(int)terminalIndexes[symbol]];
                    if (move.Length == 0 || move[0] == '-') //Error found
                    {
                        if (((state == 3) || (state == 220)) && (symbol == '}'))
                        {
                            needToken = true;
                        }
                        else if (move.Length > 0)
                        {
                            errorFound = true;
                            int ruleNum = int.Parse(move.Substring(1));
                            Error error = Error.GetSyntaxisError(ruleNum), newError;
                            if (ruleNum != 5 && ruleNum != 47)
                            {
                                if (symbol == '}')
                                {
                                    needToken = true;
                                    errorFound = false;
                                    break;
                                }
                                error = Error.GetSyntaxisError(ruleNum);
                                newError = new Error(error.id, error.description);
                                newError.lineNum = token.line;
                                newError.incorrectText = token.text;
                                newError.type = 2;
                                Error.errorsFound.Add(newError);
                            }
                            tempToken = token;
                            tokens.RemoveAt(tokens.Count - 1);
                            switch (ruleNum) //Attempts to fix the error
                            {
                                case 2://fin de código
                                    token = new Token(token.line, Lexeme.GetTerminalLexeme());
                                    break;
                                case 4://sección dibujar
                                    token = new Token(token.line, Lexeme.lexemes[0], "dibujar");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 5://\n
                                case 13:
                                case 15:
                                case 47:
                                    token = new Token(token.line, Lexeme.lexemes[Lexeme.lexemes.Count - 1]);
                                    break;
                                case 6://tipo de dato (ent)
                                    token = new Token(token.line, Lexeme.lexemes[0], "ent");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 9://(
                                    token = new Token(token.line, Lexeme.lexemes[23]);
                                    break;
                                case 10://:
                                    token = new Token(token.line, Lexeme.lexemes[22]);
                                    break;
                                case 27://)
                                case 34:
                                case 41:
                                    token = new Token(token.line, Lexeme.lexemes[24]);
                                    break;
                                case 32://;
                                    token = new Token(token.line, Lexeme.lexemes[20]);
                                    break;
                                case 29://]
                                    token = new Token(token.line, Lexeme.lexemes[19]);
                                    break;
                                case 36://|
                                    token = new Token(token.line, Lexeme.lexemes[7]);
                                    break;
                                case 39://mientras
                                    token = new Token(token.line, Lexeme.lexemes[0], "mientras");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 38://=
                                    token = new Token(token.line, Lexeme.lexemes[10]);
                                    break;
                                case 40://error
                                    token = new Token(token.line, Lexeme.lexemes[0], "error");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 46://caso
                                    token = new Token(token.line, Lexeme.lexemes[0], "caso");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 48://fin
                                    token = new Token(token.line, Lexeme.lexemes[0], "fin");
                                    token = LexicalAnalyzer.ChangeKeywordIdentifier(token);
                                    break;
                                case 50://0
                                    token = new Token(token.line, Lexeme.lexemes[26], "0");
                                    break;
                                default:
                                    if (!error.description.Contains("irrecuperable"))
                                    {
                                        error.description += "  Error irrecuperable.";
                                    }
                                    return false;

                            }
                            tokens.Add(token);
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (move[0] == 's') //Shift
                    {
                        stack.Push(symbol);
                        stack.Push(int.Parse(move.Substring(1)));
                        needToken = true;
                        node = new Semantic();
                        node.token = token;
                        Semantic.AddTerminal(node);
                    }
                    else if (move[0] == 'r') //Reduce
                    {
                        int ruleNum = int.Parse(move.Substring(1));
                        Rule rule = rules[ruleNum];
                        for (int j = rule.right.Length; j > 0; j--)
                        {
                            stack.Pop();
                            if ((char)stack.Pop() != rule.right[j - 1])
                            {
                                return false;
                            }
                        }
                        state = (int)stack.Peek();
                        stack.Push(rule.left);
                        state = int.Parse(LALRTable[state][(int)nonterminalIndexes[rule.left] + terminals.Length]);
                        stack.Push(state);
                        node = new Semantic();
                        node.rule = rule;
                        node.token = token;
                        if (!Semantic.SemanticAnalysis(node))
                        {
                            return false;
                        }
                    }
                    else //Code accepted
                    {
                        string code = Semantic.stack.Pop().code;
                        Semantic.EliminateGlobalVariableErrors();
                        if (Semantic.compile)
                        {
                            Executor.SetErrorRecovery(errorRecovery);
                            Executor.Execute(code);
                        }
                        return true;
                    }
                    break;
                }
            }
        }
    }
}
