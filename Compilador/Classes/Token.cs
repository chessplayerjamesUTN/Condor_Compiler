namespace Compilador.Clases
{
    /// <summary>
    /// The class that represents tokens scanned by the lexical analyzer.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The line number where the token was found.
        /// </summary>
        public int line;
        /// <summary>
        /// The symbol that represents the token.
        /// </summary>
        public char symbol;
        /// <summary>
        /// The text associated with the token (in case of an attribute).
        /// This is null if the token does not require an attribute.
        /// </summary>
        public string text;
        /// <summary>
        /// The lexeme associated with the token.
        /// </summary>
        public Lexeme lexeme;


        /// <summary>
        /// The constructor used when there is no attribute for a token.
        /// </summary>
        /// <param name="line">The line number where the token was found.</param>
        /// <param name="lexeme">The lexeme associated with the token.</param>
        public Token(int line, Lexeme lexeme)
        {
            this.lexeme = lexeme;
            this.line = line;
            text = "";
            this.symbol = lexeme.symbol;
        }

        /// <summary>
        /// The constructor used when an attribute is required for a token.
        /// </summary>
        /// <param name="line">The line number where the token was found.</param>
        /// <param name="lexeme">The lexeme associated with the token.</param>
        /// <param name="text">The attribute of the token (for identifiers and literals).</param>
        public Token(int line, Lexeme lexeme, string text)
        {
            this.lexeme = lexeme;
            this.line = line;
            this.text = text;
            this.symbol = lexeme.symbol;
        }
    }
}
