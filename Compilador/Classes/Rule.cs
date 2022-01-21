namespace Compilador.Clases
{
    /// <summary>
    /// This class represents grammar rules loaded from disk, to use with the syntax analyzer.
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// The counter used to assign a number to each rule.
        /// </summary>
        private static int COUNTER = 0;

        /// <summary>
        /// The number (or id) of the rule.
        /// </summary>
        public int num;
        /// <summary>
        /// The left-hand side of a rule (one character).
        /// </summary>
        public char left;
        /// <summary>
        /// The right-hand side of a rule (one or more characters).
        /// </summary>
        public char[] right;

        /// <summary>
        /// The default rule constructor.
        /// </summary>
        public Rule()
        {
            num = ++COUNTER;
        }
    }
}
