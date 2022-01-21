using System.Collections.Generic;
using System.Linq;

namespace Compilador.Clases
{
    /// <summary>
    /// This class has the main lexical analyzer, among other pertinent functions.
    /// </summary>
    class LexicalAnalyzer
    {
        /// <summary>
        /// A char array that lists the accepted Spanish characters not found in the English alphabet.
        /// </summary>
        private static char[] SPANISH_CHARACTERS =
            { 'á', 'Á', 'é', 'É', 'í', 'Í', 'ñ', 'Ñ', 'ó', 'Ó', 'ú', 'Ú', 'ü', 'Ü' };
        /// <summary>
        /// A char array that maps the accepted Spanish characters to their English alphabet counterparts.
        /// </summary>
        private static readonly char[] ENGLISH_MAP = 
            { 'a', 'A', 'e', 'E', 'i', 'I', 'n', 'N', 'o', 'O', 'u', 'U', 'u', 'U' };

        /// <summary>
        /// Used to indicate if a special Spanish character has been read or not.
        /// -True: A Spanish character has been read.  Remapping to English alphabet required.
        /// -False: A Spanish character has not yet been read.
        /// </summary>
        private static bool readSpanishCharacter;
        /// <summary>
        /// The current start index of the lexeme being analyzed in the source code.
        /// </summary>
        private static int lexemeBegin;
        /// <summary>
        /// The current line being analyzed of the source code.
        /// </summary>
        private static int line;
        /// <summary>
        /// The source code.
        /// </summary>
        private static string source;
        /// <summary>
        /// The automaton in memory for state transitions.
        /// </summary>
        private static Automaton auto;
        /// <summary>
        /// Indicates whether or not a line was just read, and if a new line token (}) should be generated or not.
        /// </summary>
        private static bool readLine;
        /// <summary>
        /// Indicates whether or not signed literals are being accepted as such, or as separate lexemes.
        /// -True: +1 (no spaces) counts as one lexeme literal, positive one
        /// -False: +1 (no spaces) counts as two lexemes: the addition operator and the literal one
        /// By default, compilations are done with this value as true;
        /// however, this can lead to errors during the syntax analysis.
        /// If an error is detected, another compilation will be done, trying this value as false.
        /// </summary>
        private static bool signedLiterals;
        

        /// <summary>
        /// Resets the lexical analyzer to default, initial values.
        /// </summary>
        /// <param name="source">The source code to analyze.</param>
        public static void Reset(string source)
        {
            lexemeBegin = 0;
            line = 1;
            readLine = false;
            signedLiterals = true;
            LexicalAnalyzer.source = source;
        }

        /// <summary>
        /// Removes comments, tabs, carriage return characters (/r, not /n), and unneeded spaces.
        /// This allows for a slightly faster compilation.
        /// </summary>
        /// <param name="source">The source code to optimize.</param>
        /// <returns>Returns an optimized source code for faster lexical scanning.</returns>
        public static string StripTabsComments(string source)
        {
            while (source.Contains("//")) //Removed comments
            {
                int num = source.IndexOf('\n', source.IndexOf("//"));
                source = source.Remove(source.IndexOf("//"), num - source.IndexOf("//"));
            }
            List<char> characters = source.ToList();
            while (characters.Remove('\t')) ; //Removes tabs
            while (characters.Remove('\r')) ; //Removes carriage returns (/r)
            source = new string(characters.ToArray());
            string newSource = source.Replace("  ", " "); //Removes contiguous spaces
            while (source != newSource)
            {
                source = newSource;
                newSource = source.Replace("  ", " ");
            }
            source = newSource;
            source = source.Replace(" \n", "\n"); //Removes spaces at end of line
            source = source.Replace("\n ", "\n"); //Removes spaces at beginning of line
            return source;
        }

        /// <summary>
        /// Obtains the next token and returns it to the syntax analyzer
        /// </summary>
        /// <returns>Returns the next token object.</returns>
        public static Token NextToken()
        {
            int forward = 0, previous_state, startState = auto.states[0], currentState = startState;
            char currentChar;
            readSpanishCharacter = false;
            string attribute;
            while (lexemeBegin + forward <= source.Length) //Scans code up to end of code (one character past end)
            {
                if (readLine) //If a new line has been detected, a new line token will be returned
                {
                    readLine = false;
                    Token t = new Token(line, Lexeme.lexemes[Lexeme.lexemes.Count - 1]);
                    return t;
                }
                previous_state = currentState;
                if (lexemeBegin + forward < source.Length) //Scans within source code, up to end
                {
                    currentChar = source[lexemeBegin + forward];
                    currentState = GetNextState(currentChar, currentState);
                }
                else //Indicates the end of the code.  Enters a space (null transition) to finish current lexeme.
                {
                    currentChar = ' ';
                    currentState = auto.transitions[currentState][(int)auto.hash['λ']];
                }
                if (currentState < 0) //A lexeme has been recognized.
                {
                    currentState = -currentState;
                    Token t;
                    if (Lexeme.lexemes[currentState - 1].requiresAttribute) //Obtains attribute if required
                    {
                        if ((currentState != 28) && (currentState != 29)) //A regular lexeme
                        {
                            attribute = source.Substring(lexemeBegin, forward);
                        }
                        else //A string or character literal that requires the final character
                        {
                            attribute = source.Substring(lexemeBegin, forward + 1);
                        }
                        //If signed literals is false, and a number (decimal or integer) lexeme has been detected
                        if (!signedLiterals && ((currentState == 27) || (currentState == 26)))
                        {
                            //Only send the sign in case of a signed literal
                            if (attribute[0] == '+' || attribute[0] == '-') 
                            {
                                lexemeBegin++;
                                if (attribute[0] == '+')
                                {
                                    t = new Token(line, Lexeme.lexemes[1]);
                                }
                                else
                                {
                                    t = new Token(line, Lexeme.lexemes[2]);
                                }
                                return ChangeKeywordIdentifier(t);
                            }
                        }
                        //Convert attribute to English alphabet accepted characters in case of Spanish
                        if (readSpanishCharacter) 
                        {
                            attribute = ConvertEnglishAlphabet(attribute);
                        }
                        t = new Token(line, Lexeme.lexemes[currentState - 1], attribute);
                    }
                    else //Create token without attribute
                    {
                        t = new Token(line, Lexeme.lexemes[currentState - 1]);
                    }
                    if (currentChar == '\n')
                    {
                        readLine = true;
                    }
                    lexemeBegin += forward + 1;
                    return ChangeKeywordIdentifier(t);
                }
                else if (currentState == 0)
                {
                    if ((previous_state == 0) && ((currentChar == ' ') || (currentChar == '\n')))
                    {
                        lexemeBegin++;
                        if ((currentChar == '\n') && !readLine)
                        {
                            Token t;
                            t = new Token(line, Lexeme.lexemes[Lexeme.lexemes.Count - 1]);
                            return ChangeKeywordIdentifier(t);
                        }
                        else forward = 0;
                    }
                    else if (auto.transitions[previous_state][(int)auto.hash['λ']] < 0)
                    {
                        currentState = -auto.transitions[previous_state][(int)auto.hash['λ']];
                        Token t;
                        if (Lexeme.lexemes[currentState - 1].requiresAttribute)
                        {
                            attribute = source.Substring(lexemeBegin, forward);
                            if (!signedLiterals && (currentState == 27))
                            {
                                if (attribute[0] == '+' || attribute[0] == '-')
                                {
                                    lexemeBegin++;
                                    if (attribute[0] == '+')
                                    {
                                        t = new Token(line, Lexeme.lexemes[1]);
                                    }
                                    else
                                    {
                                        t = new Token(line, Lexeme.lexemes[2]);
                                    }
                                    return ChangeKeywordIdentifier(t);
                                }
                            }
                            if (readSpanishCharacter)
                            {
                                attribute = ConvertEnglishAlphabet(attribute);
                            }
                            t = new Token(line, Lexeme.lexemes[currentState - 1], attribute);
                        }
                        else
                        {
                            t = new Token(line, Lexeme.lexemes[currentState - 1]);
                        }
                        lexemeBegin += forward;
                        return ChangeKeywordIdentifier(t);
                    }
                    else //In case of an error
                    {
                        Error error = Error.GetLexicalError(previous_state),
                            newError = new Error(error.id, error.description);
                        newError.lineNum = line;
                        if (currentChar == '\n')
                        {
                            newError.lineNum--;
                        }
                        newError.incorrectText = source.Substring(lexemeBegin, forward + 1);
                        newError.type = 1;
                        newError.length = forward + 1;
                        Error.errorsFound.Add(newError);
                        lexemeBegin = lexemeBegin + forward + 1;
                    }
                }
                else forward++;
            }
            //Once end of code has been reached, final terminal token is returned.
            return new Token(line, Lexeme.GetTerminalLexeme());
        }

        /// <summary>
        /// Determines if an identifier is really a keyword, and converts it to the correct format if necessary.
        /// </summary>
        /// <param name="token">The identifier token to check if it's a keyword.</param>
        /// <returns>Returns the token in the correct format (changed if it's a keyword).</returns>
        public static Token ChangeKeywordIdentifier(Token token)
        {
            if ((token.lexeme.id == Lexeme.IDENTIFIER) && Keyword.FindKeyword(token.text))
            {
                string reference = (string)Keyword.hashIndex[token.text];
                if (reference.Length == 1)
                {
                    token.symbol = reference.ToCharArray()[0];
                }
                else
                {
                    token.text = reference;
                }
            }
            return token;
        }

        /// <summary>
        /// If Spanish characters have been read, convert to English
        /// </summary>
        /// <param name="text">The text to map to English alphabet equivalent.
        /// -For example, "género" is changed to "genero".</param>
        /// <returns>The remapped text.</returns>
        public static string ConvertEnglishAlphabet(string text)
        {
            for (int i = 0; i < SPANISH_CHARACTERS.Length; i++)
            {
                text = text.Replace(SPANISH_CHARACTERS[i], ENGLISH_MAP[i]);
            }
            return text;
        }

        /// <summary>
        /// Loads and stores the automaton data, which is only required within this class.
        /// </summary>
        /// <param name="auto">The automaton loaded from disk.</param>
        public static void SetAutomaton(Automaton auto)
        {
            LexicalAnalyzer.auto = auto;
        }

        /// <summary>
        /// Disables signed literals.  This should only be run upon unsuccessful compilation due to a syntax error.
        /// </summary>
        public static void DisableSignedLiterals()
        {
            LexicalAnalyzer.signedLiterals = false;
        }

        /// <summary>
        /// Obtains the next state in the automaton, per the current character.
        /// </summary>
        /// <param name="currentChar">The current character being analyzed.</param>
        /// <param name="currentState">The current state in the automaton.</param>
        /// <returns>The state to transition to in the automaton.</returns>
        private static int GetNextState(char currentChar, int currentState)
        {
            //ifs are not ordered by number, rather, by theoretical best performance

            //Letter in the English alphabet
            if (((currentChar >= 65) && (currentChar <= 90)) || ((currentChar >= 97) && (currentChar <= 122)))
            {
                currentState = auto.transitions[currentState][(int)auto.hash['l']];
            }
            //Numbers (not including zero)
            else if ((currentChar > 48) && (currentChar <= 57))
            {
                currentState = auto.transitions[currentState][(int)auto.hash['d']];
            }
            //The number zero
            else if (currentChar == 48)
            {
                currentState = auto.transitions[currentState][(int)auto.hash['c']];
            }
            //Characters from the Spanish alphabet
            else if (SPANISH_CHARACTERS.Contains(currentChar))
            {
                currentState = auto.transitions[currentState][(int)auto.hash['l']];
                readSpanishCharacter = true;
            }
            //Newline character
            else if (currentChar == '\n')
            {
                currentState = auto.transitions[currentState][(int)auto.hash['λ']];
                line++;
            }
            //Determines if symbol read is contained in alphabet
            else if (auto.hash.ContainsKey(currentChar))
            {
                currentState = auto.transitions[currentState][(int)auto.hash[currentChar]];
            }
            //Applies a null transition because character was not recognized in normal accepted alphabet.
            else
            {
                currentState = auto.transitions[currentState][(int)auto.hash['λ']];
            }
            return currentState;
        }
    }
}
