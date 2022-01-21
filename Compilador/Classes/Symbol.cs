using System;
using System.Collections.Generic;

namespace Compilador.Clases
{
    /// <summary>
    /// The class that stores the required information for symbols (variables, arrays, and methods).
    /// The complete collection of symbols is the symbolTable.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// A counter that allows the addition of an ID to each symbol.
        /// </summary>
        private static int COUNTER;

        /// <summary>
        /// A list that contains all of the found symbols during compilation.
        /// </summary>
        public static List<Symbol> symbolTable;

        /// <summary>
        /// Represents the data type.
        /// 1: int
        /// 2: double
        /// 3: string
        /// 4: char
        /// 5: bool
        /// 6: texture
        /// 7: Color
        /// 8: void
        /// 9: convert
        /// </summary>
        public sbyte dataType;
        /// <summary>
        /// Represents the number of dimensions.  If a regular variable, 0.  Arrays are 1 or greater.
        /// Not used for methods.
        /// </summary>
        public sbyte dimensions;
        /// <summary>
        /// Stores the symbol's ID.
        /// </summary>
        public int id;
        /// <summary>
        /// Stores the symbol's name (identifier).
        /// </summary>
        public string identifier;
        /// <summary>
        /// A list of the data types, in case of a method.
        /// See Symbol.dataType for more information on the allowed data types.
        /// </summary>
        public List<sbyte> methodDataTypes;


        /// <summary>
        /// Creates a new symbol, assigns the ID, and creates a new empty method data type list.
        /// </summary>
        public Symbol()
        {
            id = COUNTER++;
            methodDataTypes = new List<sbyte>();
        }

        /// <summary>
        /// Clears the symbol table and resets the counter, making way for a new compilation.
        /// </summary>
        public static void ResetSymbolTable()
        {
            symbolTable = new List<Symbol>(15);
            COUNTER = 0;
        }

        /// <summary>
        /// Adds a new symbol to the Symbol Table.
        /// </summary>
        /// <param name="symbol">A new Symbol object to add to the Symbol Table.</param>
        public static void AddSymbol(Symbol symbol)
        {
            symbolTable.Add(symbol);
        }

        /// <summary>
        /// Obtains a symbol based off of its name.
        /// </summary>
        /// <param name="name">The name used to search in the Symbol Table.</param>
        /// <returns>Returns the Symbol object that has the same name as the entered argument.</returns>
        public static Symbol GetSymbol(string name)
        {
            foreach(Symbol symbol in symbolTable)
            {
                if (symbol.identifier == name)
                {
                    return symbol;
                }
            }
            throw new Exception("Símbolo no agregado.");
        }

        /// <summary>
        /// In case of list declarations (i.e.: ent a, b, c), assigns the correct type to the specified number of symbols.
        /// </summary>
        /// <param name="type">The data type to assign to the last entered symbols (refer to Symbol.dataType).</param>
        /// <param name="counter">The number of symbols to specify data type with.</param>
        public static void AddSymbolType(sbyte type, int counter)
        {
            for (int i = counter - 1; i >= 0; i--)
            {
                symbolTable[symbolTable.Count - i - 1].dataType = type;
            }
        }

    }
}
