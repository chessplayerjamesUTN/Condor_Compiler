# Condor_Compiler
## Brief Description
Condor Compiler is a Windows Forms project that converts code written in a new, simple, multimedia programming language, into C# code, that later is compiled by .NET and run on the device.  The new programming language is simple for students to learn, and the reserved words are in Spanish to better accommodate learning in another language.

## Key Features
- **Simple programming language** that is easy to learn

![image](https://user-images.githubusercontent.com/57734444/190204209-1ecab296-8c1e-443d-bcd9-920f0e2c0df1.png)

- **Automatic error detection** - lexical errors are highlighted - all detected errors are shown in the error table at the bottom

![image](https://user-images.githubusercontent.com/57734444/190204634-71de9153-47d6-4914-aad3-5cf68a61c599.png)

- **Rudimentary error recovery** - most lexical errors are automatically corrected - some unambiguous syntactic errors are automatically corrected

![image](https://user-images.githubusercontent.com/57734444/190205291-3eb7f378-3f57-4dd1-8512-92ae7cf76a73.png)

- **Ability to load, save, and edit code** - recent files shown

![image](https://user-images.githubusercontent.com/57734444/190205376-bd830d5e-4354-4e5d-88c9-d0d007af1b3a.png)

![image](https://user-images.githubusercontent.com/57734444/190205422-b0af2f89-aba1-4a1b-ae61-7dc6cafba2e5.png)

- **Simple debugger** - shows all values, in order, of the selected variables during the execution of the program

![image](https://user-images.githubusercontent.com/57734444/190206033-74114d04-9ff9-46eb-a815-b978d126c749.png)

![image](https://user-images.githubusercontent.com/57734444/190205959-e0187fb9-7782-43f9-aab7-41301932ad37.png)

- **Multimedia programming language** - sounds, images, and colors

![image](https://user-images.githubusercontent.com/57734444/190206527-e707e209-960b-4e5b-9035-c281eb7bea45.png)

![image](https://user-images.githubusercontent.com/57734444/190207002-f500e7d7-e56b-4293-9686-51d3d2f4e482.png)

![image](https://user-images.githubusercontent.com/57734444/190207104-682eaa48-67e9-4414-9d6a-d19125172ec9.png)

![image](https://user-images.githubusercontent.com/57734444/190207223-7353fb66-322f-4f1d-ac92-b2912dcf6b90.png)

![image](https://user-images.githubusercontent.com/57734444/190207346-0baa2ba8-b5aa-47bf-82fd-346f1d6dcd88.png)


## Compilation Process
Code is compiled in a single pass.  A bottom-up LALR parser is used.
1. Syntax analyzer begins.  Needs lexical tokens to continue.
2. Lexical analysis begins.  The first lexeme is detected and converted to a token.  This is sent back to the syntax analyzer.
3. The syntax analyzer consults the LALR grammar table, and determines the next action: Shift, Reduce, Accept, Error.
4. A syntax node is created, and passed to the semantic analyzer.  The semantic analyzer generates the correct C# code for the current node, and ensures correct types and sizes are used.
5. Upon complete analysis of the code: the lexemes used, the grammatical structure, and the types of data, if there are no unrecoverable errors, the code, already translated to C#, is written to a .cs file, and compiled by the .NET compiler.
6. The compiled .exe is run for the user to test.

## Future Improvements
- Translation of reserved words for other natural languages (Italian, German, French, etc.)
- Inclusion of mouse and keyboard detection for quick game development
