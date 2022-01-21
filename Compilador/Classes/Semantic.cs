using Compilador.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compilador.Classes
{
    public class Semantic
    {
        private static readonly string[] TYPEMAP = { "", "int", "double", "string", "char", "bool", "texture", "Color", "void", "convert" };

        private static byte possibleError;
        private static sbyte declareType, recursiveMethodType, returnType, tempMethodType;
        private static int createdVariables, foundCounter, repeatVariable, scopeCounter, temporaryCounter;
        private static bool assignmentUsed, needGraphics, randUsed;
        private static string recursiveMethodName, tempForVariable, tempMethodName, tempMethodTreeCode, tempVarName1, tempVarName2, undeclaredVariable;
        private static List<sbyte> dataTypes, methodDataTypes;
        private static List<string> classVariableNames, classVariablesCode, graphics, undeclaredFunctions;
        private static Stack<int> backpatcher;

        public static int errorCount;
        public static bool compile, debugging, XNAMode, errorRecovery;
        public static List<string> debugVariableList;
        public static Stack<Semantic> stack;

        public sbyte type;
        public int num = 5;
        public string code;
        public Rule rule;
        public Token token;
        public List<int> scopeList;
        public List<string> variableList;
        
        public static void Initialize(bool errorRecovery)
        {
            possibleError = 0;
            declareType = 0;
            recursiveMethodType = 0;
            returnType = 0;
            tempMethodType = 0;
            createdVariables = 0;
            foundCounter = 0;
            repeatVariable = 0;
            scopeCounter = 1;
            temporaryCounter = 100;
            errorCount = 0;
            assignmentUsed = false;
            randUsed = false;
            if (compile == false) debugging = false;
            XNAMode = false;
            Semantic.errorRecovery = errorRecovery;
            recursiveMethodName = "";
            tempMethodName = "";
            tempMethodTreeCode = "";
            tempVarName1 = "";
            tempVarName2 = "";
            undeclaredVariable = "";
            dataTypes = new List<sbyte>(5);
            methodDataTypes = new List<sbyte>(5);
            classVariableNames = new List<string>();
            classVariablesCode = new List<string>();
            graphics = new List<string>();
            undeclaredFunctions = new List<string>();
            backpatcher = new Stack<int>(20);
            stack = new Stack<Semantic>(50);
        }
        public static bool SemanticAnalysis(Semantic node)
        {
            Semantic tempNode1 = new Semantic(), tempNode2 = new Semantic(), tempNode3 = new Semantic(), tempNode4 = new Semantic();
            Symbol symbol;
            string allCode;
            switch (node.rule.num)
            {
                case 1: //P'->P
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    for (int i = 0; i < undeclaredFunctions.Count; i++)
                    {
                        Error error = Error.GetSemanticError(-15), newError = new Error(error.id, error.description);
                        newError.incorrectText = undeclaredFunctions[i];
                        newError.type = 3;
                        Error.errorsFound.Add(newError);
                        errorCount++;
                    }
                    break;
                case 49: //Q->X
                case 52: //X->v
                case 53: //X->q
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    break;
                case 2: //P->FSD
                    if (debugging) node.type = -18;
                    tempNode3 = stack.Pop();
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    if (compile)
                    {
                        node.code = tempNode1.code + BackPatcher(tempNode2.code + tempNode3.code);
                        string initialCode = "";
                        while (tempNode2.code.Contains('δ'))
                        {
                            tempNode2.code = tempNode2.code.Substring(0, tempNode2.code.IndexOf('δ'))
                                + tempNode2.code.Substring(tempNode2.code.IndexOf('δ', tempNode2.code.IndexOf('δ') + 1) + 1);
                        }
                        while (tempNode1.code.Contains('δ'))
                        {
                            initialCode += tempNode1.code.Substring(tempNode1.code.IndexOf(' ', tempNode1.code.IndexOf('δ')),
                                tempNode1.code.IndexOf('δ', tempNode1.code.IndexOf('δ') + 1) - tempNode1.code.IndexOf(' ', tempNode1.code.IndexOf('δ')))
                                + "\r\n";
                            tempNode1.code = tempNode1.code.Substring(0, tempNode1.code.IndexOf('δ'))
                                + tempNode1.code.Substring(tempNode1.code.IndexOf('δ', tempNode1.code.IndexOf('δ') + 1) + 1);
                        }
                        while (tempNode3.code.Contains('δ'))
                        {
                            tempNode3.code = tempNode3.code.Substring(0, tempNode3.code.IndexOf('δ'))
                                + tempNode3.code.Substring(tempNode3.code.IndexOf('δ', tempNode3.code.IndexOf('δ') + 1) + 1);
                        }
                        tempNode2.code = initialCode + tempNode2.code;
                        allCode = CreateGraphicsCode(classVariablesCode, tempNode2.code, tempNode3.code, tempNode1.code);
                        node.code = allCode;
                        GraphicsContentWriter();
                    }
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, MergeScopeLists(tempNode2.scopeList,
                        MergeScopeLists(tempNode3.scopeList, scopeCounter++)));
                    break;
                case 3: //P->FS
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    if (compile)
                    {
                        tempNode1.code = tempNode1.code.Replace("δ", "");
                        tempNode2.code = BackPatcher(tempNode2.code);
                        tempNode2.code = tempNode2.code.Replace("δ", "");
                        if (debugging) node.code = CreateDebugCode(new List<string>(), tempNode2.code, tempNode1.code);
                        else node.code = "using System;\nusing System.Drawing;\r\nusing System.Collections.Generic;\nclass Program\n{\n"
                                + tempNode1.code + "\nstatic void Main(string[] args)\n{\n" + tempNode2.code + "}";
                    }
                        node.scopeList = MergeScopeLists(tempNode1.scopeList, MergeScopeLists(tempNode2.scopeList, scopeCounter++));
                    break;
                case 4: //P->SD
                    if (debugging) node.type = -18;
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    if (compile)
                    {
                        tempNode1.code = BackPatcher(tempNode1.code);
                        while (tempNode1.code.Contains('δ'))
                        {
                            tempNode1.code = tempNode1.code.Substring(0, tempNode1.code.IndexOf('δ')) +
                                tempNode1.code.Substring(tempNode1.code.IndexOf('δ', tempNode1.code.IndexOf('δ') + 1) + 1);
                        }
                        while (tempNode2.code.Contains('δ'))
                        {
                            tempNode2.code = tempNode2.code.Substring(0, tempNode2.code.IndexOf('δ'))
                                + tempNode2.code.Substring(tempNode2.code.IndexOf('δ', tempNode2.code.IndexOf('δ') + 1) + 1);
                        }
                        allCode = CreateGraphicsCode(classVariablesCode, tempNode1.code, tempNode2.code, "");
                        node.code = allCode;
                        GraphicsContentWriter();
                    }
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, MergeScopeLists(tempNode2.scopeList, scopeCounter++));
                    break;
                case 5: //P->S
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    if (compile)
                    {
                        node.code = BackPatcher(node.code);
                        if (debugging) node.code = CreateDebugCode(new List<string>(), node.code, "");
                        else node.code = "using System;\nusing System.Drawing;\r\nusing System.Collections.Generic;\nclass Program\n{\n"
                                + "static void Main(string[] args)\n{\n" + node.code + "}";
                        node.code = node.code.Replace("δ", "");
                    }
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, scopeCounter++);
                    break;
                case 6: //F->MF
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code;
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, tempNode2.scopeList);
                    break;
                case 7: //F->M
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.scopeList = tempNode1.scopeList;
                    break;
                case 8: //M->{Ti(B)~}S
                    tempNode4 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode3 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    tempNode4.code = tempMethodTreeCode + tempNode4.code;
                    tempMethodTreeCode = "";
                    node.code = "static " + tempNode1.code + tempNode2.code + "(" + tempNode3.code + ")\n{\n" + tempNode4.code;
                    AddTDSMethod(tempNode1.type, tempNode2.token.text, methodDataTypes);
                    methodDataTypes.Clear();
                    dataTypes = new List<sbyte>(5);
                    stack.Pop();
                    node.scopeList = tempNode4.scopeList;
                    if (tempNode4.variableList == null) tempNode4.variableList = new List<string>();
                    if (FindMultipleDeclarations(tempNode3.variableList, tempNode4.variableList) != "")
                    {
                        //error, variable declared multiple times
                    }
                    tempNode3.variableList.AddRange(tempNode4.variableList);
                    node.variableList = tempNode3.variableList;
                    node.code = BackPatcher(node.code);
                    while (undeclaredFunctions.Remove(tempNode2.code)) ;
                    assignmentUsed = false;
                    break;
                case 9: //D->ö~}S
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.scopeList = tempNode1.scopeList;
                    node.variableList = tempNode1.variableList;
                    XNAMode = true;
                    break;
                case 10: //T->e
                case 11: //T->b
                case 12: //T->t
                case 13: //T->g
                case 14: //T->h
                case 15: //T->r
                case 16: //T->k
                case 17: //T->ë
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = tempNode1.type;
                    tempMethodType = node.type;
                    break;
                case 56: //E->J
                case 60: //J->N
                case 64: //N->l
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = tempNode1.type;
                    break;
                case 65: //N->`
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = tempNode1.type;
                    break;
                case 18: //B->ÉcB
                    recursiveMethodType = tempMethodType;
                    recursiveMethodName = tempMethodName;
                    tempNode2 = stack.Pop();
                    node.code = tempNode2.code;
                    stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "," + node.code;
                    if (tempNode1.type == tempNode2.type) node.type = tempNode1.type;
                    else node.type = 20;
                    if (FindMultipleDeclarations(tempNode1.variableList, tempNode2.variableList) != "")
                    {
                        //error, variable declared multiple times
                    }
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 38: //L->UcL
                    tempNode2 = stack.Pop();
                    node.code = tempNode2.code;
                    stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "," + node.code;
                    if (tempNode1.type == tempNode2.type) node.type = tempNode1.type;
                    else node.type = 20;
                    if (FindMultipleDeclarations(tempNode1.variableList, tempNode2.variableList) != "")
                    {
                        //error, variable declared multiple times
                    }
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 19: //B->É
                    recursiveMethodType = tempMethodType;
                    recursiveMethodName = tempMethodName;
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.variableList = tempNode1.variableList;
                    break;
                case 39: //L->U
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.variableList = tempNode1.variableList;
                    break;
                case 102: //Á->EcÁ
                    tempNode2 = stack.Pop();
                    node.code = tempNode2.code;
                    stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "," + node.code;
                    if (tempNode1.type == tempNode2.type) node.type = tempNode1.type;
                    else node.type = 20;
                    dataTypes.Add(tempNode1.type);
                    break;
                case 20: //É->Ti
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code;
                    node.type = tempNode1.type;
                    methodDataTypes.Add(node.type);
                    symbol = new Symbol();
                    symbol.dimensions = 0;
                    symbol.identifier = tempNode2.code;
                    //symbol.scope = scopeCounter;
                    symbol.dataType = node.type;
                    Symbol.AddSymbol(symbol);
                    node.variableList = new List<string>();
                    node.variableList.Add(tempNode2.code);
                    if (debugging && debugVariableList.Contains(symbol.identifier))
                    {
                        tempMethodTreeCode += "\nbool _found" + foundCounter + " = false;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + symbol.identifier + "\") _found" + foundCounter + " = true; }"
                                    + "if (!_found" + foundCounter++ + ") treeView1.Nodes.Add(\"" + symbol.identifier + "\");";
                    }
                    break;
                case 21: //S->I}S
                    tempNode2 = stack.Pop();
                    stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "\n" + tempNode2.code;
                    if (tempNode1.scopeList == null) tempNode1.scopeList = new List<int>();
                    if (tempNode2.scopeList == null) tempNode2.scopeList = new List<int>();
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, tempNode2.scopeList);
                    if (tempNode1.variableList == null) tempNode1.variableList = new List<string>();
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    if (FindMultipleDeclarations(tempNode1.variableList, tempNode2.variableList) != "")
                    {
                        //error, variable declared multiple times
                    }
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 22: //S->I}f
                    for(int i = 0; i< 3; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "\n}\n";
                    if (tempNode1.scopeList == null) tempNode1.scopeList = new List<int>();
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, scopeCounter++);
                    node.variableList = tempNode1.variableList;
                    break;
                case 23: //I->G
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    node.variableList = tempNode1.variableList;
                    if (!assignmentUsed)
                    {
                        classVariablesCode.Add(node.code);
                        string[] variableNames = node.code.Substring(node.code.IndexOf(' ') + 1).Split(',');
                        foreach(string varName in variableNames)
                        {
                            if (varName.Contains(';')) classVariableNames.Add(varName.Substring(0, varName.Length - 1));
                            else classVariableNames.Add(varName);
                        }
                        node.code = "δ" + node.code + "δ";
                    }
                    assignmentUsed = false;
                    break;
                case 29: //I->H
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    node.variableList = tempNode1.variableList;
                    break;
                case 25: //I->C
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    if (debugging)
                    {
                        Symbol st = Symbol.GetSymbol(tempVarName1);
                        if (debugVariableList.Contains(st.identifier))
                        {
                            //node.code += "\ntreeView1.Nodes[" + st.id + "].Nodes.Add((" + tempVarName1 + ").ToString());";
                            node.code += "\nint _found" + foundCounter + " = -1;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + st.identifier + "\") _found" + foundCounter + " = _i1; }"
                                    + "if (_found" + foundCounter + " >= 0) treeView1.Nodes[_found" + foundCounter++ + "].Nodes.Add((" + tempVarName1 + ").ToString());";
                        }
                    }
                    if (possibleError != 0) node.code = SurroundTryCatch(node.code, possibleError);
                    break;
                case 106://I->Ï
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    if (debugging)
                    {
                        Symbol st = Symbol.GetSymbol(tempVarName1.Substring(0, tempVarName1.IndexOf('[')));
                        if (debugVariableList.Contains(st.identifier))
                        {
                            //node.code += "\ntreeView1.Nodes[" + st.id + "].Nodes[" + tempVarName2 + "].Nodes.Add(("
                            //   + tempVarName1 + ").ToString());";
                            node.code += "\nint _found" + foundCounter + " = -1;\r\n"
                                   + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                   + "if (treeView1.Nodes[_i1].Text == \"" + st.identifier + "\") _found" + foundCounter + " = _i1; }"
                                   + "if (_found" + foundCounter + " >= 0) treeView1.Nodes[_found" + foundCounter++ + "].Nodes[" + tempVarName2 + "].Nodes.Add((" + tempVarName1 + ").ToString());";
                        }
                    }
                    if (possibleError != 0) node.code = SurroundTryCatch(node.code, possibleError);
                    break;
                case 32: //I->\
                case 34: //I->s
                case 35: //I->`
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    if (possibleError != 0) node.code = SurroundTryCatch(node.code, possibleError);
                    break;
                case 24: //I->A
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    if (undeclaredVariable != "")
                    {
                        node.type = -11;
                        node.code = "\n";
                    }
                    if (debugging)
                    {
                        Symbol st = Symbol.GetSymbol(tempVarName1);
                        if (debugVariableList.Contains(st.identifier))
                        {
                            //node.code += "\ntreeView1.Nodes[" + st.id + "].Nodes.Add((" + tempVarName1 + ").ToString());";
                            node.code += "\nint _found" + foundCounter + " = -1;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + st.identifier + "\") _found" + foundCounter + " = _i1; }"
                                    + "if (_found" + foundCounter + " >= 0) treeView1.Nodes[_found" + foundCounter++ + "].Nodes.Add((" + tempVarName1 + ").ToString());";
                        }
                    }
                    if (possibleError != 0) node.code = SurroundTryCatch(node.code, possibleError);
                    break;
                case 26: //I->Í
                case 27: //I->Ó
                case 28: //I->W
                case 30: //I->Y
                case 31: //I->Z
                case 33: //I->_
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.scopeList = tempNode1.scopeList;
                    node.variableList = tempNode1.variableList;
                    break;
                case 36: //G->TL
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code;
                    Symbol.AddSymbolType(tempNode1.type, createdVariables);
                    createdVariables = 0;
                    node.variableList = tempNode2.variableList;
                    if (debugging && !tempNode2.code.Contains('('))
                    {
                        string[] strippedCode = tempNode2.code.Split(',');
                        foreach (string code in strippedCode)
                        {

                            if (code.Contains('=') && debugVariableList.Contains(code.Substring(0, code.IndexOf('='))))
                            {
                                node.code += ";\r\nbool _found" + foundCounter + " = false;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + code.Substring(0, code.IndexOf('=')) + "\") _found" + foundCounter + " = true; }"
                                    + "if (!_found" + foundCounter + ") treeView1.Nodes.Add(\"" + code.Substring(0, code.IndexOf('=')) + "\")";
                            }
                            else if (debugVariableList.Contains(code))
                            {
                                node.code += ";\r\nbool _found" + foundCounter + " = false;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + code + "\") _found" + foundCounter + " = true; }"
                                    + "if (!_found" + foundCounter + ") treeView1.Nodes.Add(\"" + code + "\")";
                            }
                            foundCounter++;
                        }
                    }
                    break;
                case 37: //G->Ti[Á]
                    for (int i = 0; i < 2; i++) tempNode3 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "[" + PrintMatrixDimensions(tempNode3.code) + "]"
                        + tempNode2.code + " = new " + TYPEMAP[tempNode1.type] + "[" + tempNode3.code + "]";
                    int arrayNum = 0;
                    if (CheckIntDataInput(dataTypes)) arrayNum = InsertTDSArray(tempNode1.type, tempNode2.code, tempNode3.code);
                    else//Error 410
                    {
                        node.type = -10;
                    }
                    dataTypes.Clear();
                    node.variableList = new List<string>();
                    node.variableList.Add(tempNode2.code);
                    for (int i = 0; i < tempNode3.code.Length; i++)
                    {
                        if (tempNode3.code[i] > 57)
                        {
                            assignmentUsed = true;
                            break;
                        }
                    }
                    if (debugging && debugVariableList.Contains(tempNode2.code))
                    {
                        node.code += "\n;treeView1.Nodes.Add(\""
                            + tempNode2.code + "\");\r\nfor (int _i = 0; _i < " + tempNode3.code
                            + "; _i++) treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(\"[\" + _i.ToString() + \"]\")";
                    }
                    break;
                case 40: //U->i
                    tempNode1 = stack.Pop();
                    symbol = new Symbol();
                    symbol.dimensions = 0;
                    symbol.identifier = tempNode1.code;
                    //symbol.scope = scopeCounter;
                    Symbol.AddSymbol(symbol);
                    node.code = tempNode1.code;
                    createdVariables++;
                    node.variableList = new List<string>();
                    node.variableList.Add(tempNode2.code);
                    break;
                case 41: //U->A
                    tempNode1 = stack.Pop();
                    symbol = new Symbol();
                    symbol.dimensions = 0;
                    symbol.identifier = tempNode1.code.Substring(0, tempNode1.code.IndexOf('='));
                    //symbol.scope = scopeCounter;
                    Symbol.AddSymbol(symbol);
                    node.code = tempNode1.code;
                    createdVariables++;
                    undeclaredVariable = "";
                    node.variableList = new List<string>();
                    node.variableList.Add(symbol.identifier);
                    assignmentUsed = true;
                    break;
                case 42: //A->i=E
                case 43: //A->i=K
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "=" + tempNode2.code;
                    try
                    {
                        node.type = CheckType(GetTDSType(tempNode1.code), tempNode2.type, '=');
                    }
                    catch
                    {
                        node.type = CheckType(declareType, tempNode2.type, '=');
                        undeclaredVariable = tempNode1.code;
                    }
                    tempVarName1 = tempNode1.code;
                    tempForVariable = tempVarName1;
                    break;
                case 44: //C->ia
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "++";
                    try
                    {
                        node.type = CheckType(GetTDSType(tempNode1.code), 1, '+');
                    }
                    catch //Error 411
                    {
                        node.type = -11;
                    }
                    tempVarName1 = tempNode1.code;
                    break;
                case 45: //C->id
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "--";
                    try
                    {
                        node.type = CheckType(GetTDSType(tempNode1.code), 1, '-');
                    }
                    catch //Error 411
                    {
                        node.type = -11;
                    }
                    tempVarName1 = tempNode1.code;
                    break;
                case 46: //K->QOK
                    node.code = "";
                    for (int i = 0; i < 3; i++)
                    {
                        tempNode1 = stack.Pop();
                        node.code = tempNode1.code + node.code;
                    }
                    node.type = 5;
                    break;
                case 47: //K->(QOK)
                    node.code = "";
                    stack.Pop();
                    for (int i = 0; i < 3; i++)
                    {
                        tempNode1 = stack.Pop();
                        node.code = tempNode1.code + node.code;
                    }
                    stack.Pop();
                    node.type = 5;
                    break;
                case 48: //K->Q
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = 5;
                    break;
                case 50: //Q->%X
                    tempNode1 = stack.Pop();
                    stack.Pop();
                    node.code = "!(" + tempNode1.code + ")";
                    break;
                case 51: //X->ERE
                    tempNode3 = stack.Pop();
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code + tempNode3.code;
                    node.type = CheckType(tempNode1.type, tempNode3.type, '=');
                    break;
                case 54: //E->E+J
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempNode1.type == 9) tempNode1 = ConvertedNode(tempNode1, tempNode2);
                    else if (tempNode2.type == 9) tempNode2 = ConvertedNode(tempNode2, tempNode1);
                    node.code = tempNode1.code + "+" + tempNode2.code;
                    node.type = CheckType(tempNode1.type, tempNode2.type, '+');
                    break;
                case 55: //E->E-J
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempNode1.type == 9) tempNode1 = ConvertedNode(tempNode1, tempNode2);
                    else if (tempNode2.type == 9) tempNode2 = ConvertedNode(tempNode2, tempNode1);
                    node.code = tempNode1.code + "-" + tempNode2.code;
                    node.type = CheckType(tempNode1.type, tempNode2.type, '-');
                    break;
                case 57: //J->J*N
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempNode1.type == 9) tempNode1 = ConvertedNode(tempNode1, tempNode2);
                    else if (tempNode2.type == 9) tempNode2 = ConvertedNode(tempNode2, tempNode1);
                    node.code = tempNode1.code + "*" + tempNode2.code;
                    node.type = CheckType(tempNode1.type, tempNode2.type, '*');
                    break;
                case 58: //J->J/N
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempNode1.type == 9) tempNode1 = ConvertedNode(tempNode1, tempNode2);
                    else if (tempNode2.type == 9) tempNode2 = ConvertedNode(tempNode2, tempNode1);
                    node.code = tempNode1.code + "/" + tempNode2.code;
                    node.type = CheckType(tempNode1.type, tempNode2.type, '/');
                    possibleError = 2;
                    break;
                case 59: //J->J^N
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempNode1.type == 9) tempNode1 = ConvertedNode(tempNode1, tempNode2);
                    else if (tempNode2.type == 9) tempNode2 = ConvertedNode(tempNode2, tempNode1);
                    node.code = "Math.Pow(" + tempNode1.code + "," + tempNode2.code + ")";
                    node.type = CheckType(tempNode1.type, tempNode2.type, '^');
                    break;
                case 61: //N->i
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    try
                    {
                        node.type = GetTDSType(tempNode1.code);
                    }
                    catch
                    {
                        node.type = -11;
                    }
                    break;
                case 62: //N->i[Á]
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "[" + tempNode2.code + "]";
                    try
                    {
                        node.type = GetTDSType(tempNode1.code);
                        if (!CheckIntDataInput(dataTypes)) node.type = -10;
                        else 
                        {
                            Symbol st = Symbol.GetSymbol(tempNode1.code);
                            if (st.dimensions != GetArgumentDimensions(tempNode2.code))
                            {
                                if (!(st.dataType == 3 && GetArgumentDimensions(tempNode2.code) < 2)) node.type = -12;
                            }
                        }
                    }
                    catch
                    {
                        node.type = -11;
                    }
                    dataTypes.Clear();
                    possibleError = 3;
                    break;
                case 63: //N->(E)
                    stack.Pop();
                    tempNode1 = stack.Pop();
                    stack.Pop();
                    node.code = "(" + tempNode1.code + ")";
                    node.type = tempNode1.type;
                    break;
                case 66: //R->¿
                    stack.Pop();
                    node.code = "==";
                    break;
                case 67: //R->#
                    stack.Pop();
                    node.code = "!=";
                    break;
                case 68: //R->m
                    stack.Pop();
                    node.code = ">=";
                    break;
                case 69: //R->>
                    stack.Pop();
                    node.code = ">";
                    break;
                case 70: //R->¡
                    stack.Pop();
                    node.code = "<=";
                    break;
                case 71: //R-><
                    stack.Pop();
                    node.code = "<";
                    break;
                case 72: //O->&
                    stack.Pop();
                    node.code = "&&";
                    break;
                case 73: //O->o
                    stack.Pop();
                    node.code = "||";
                    break;
                case 74: //Í->u(K)~}SÑ
                    tempNode3 = stack.Pop();
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "if(" + tempNode1.code + ")\n{\n" + tempNode2.code + tempNode3.code;
                    node.scopeList = MergeScopeLists(tempNode2.scopeList, MergeScopeLists(tempNode3.scopeList, scopeCounter++));
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    if (tempNode3.variableList == null) tempNode3.variableList = new List<string>();
                    tempNode2.variableList.AddRange(tempNode3.variableList);
                    node.variableList = tempNode2.variableList;
                    break;
                case 75: //Í->u(K)~}S
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "if(" + tempNode1.code + ")\n{\n" + tempNode2.code;
                    node.scopeList = tempNode2.scopeList;
                    node.variableList = tempNode2.variableList;
                    break;
                case 76: //Ñ->w(K)~}SÑ
                    tempNode3 = stack.Pop();
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "else if(" + tempNode1.code + ")\n{\n" + tempNode2.code + tempNode3.code;
                    node.scopeList = MergeScopeLists(tempNode2.scopeList, MergeScopeLists(tempNode3.scopeList, scopeCounter++));
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    if (tempNode3.variableList == null) tempNode3.variableList = new List<string>();
                    tempNode2.variableList.AddRange(tempNode3.variableList);
                    node.variableList = tempNode2.variableList;
                    break;
                case 77: //Ñ->w(K)~}S
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "else if(" + tempNode1.code + ")\n{\n" + tempNode2.code;
                    node.scopeList = MergeScopeLists(tempNode2.scopeList, scopeCounter++);
                    node.variableList = tempNode2.variableList;
                    break;
                case 78: //Ñ->x~}S
                    tempNode1 = stack.Pop();
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.code = "else\n{\n" + tempNode1.code;
                    node.scopeList = MergeScopeLists(tempNode2.scopeList, scopeCounter++);
                    node.variableList = tempNode1.variableList;
                    break;
                case 79: //Ó->y(i)~}Úf
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    try
                    {
                        node.type = GetTDSType(tempNode1.code);
                        node.code = "switch(" + tempNode1.code + ")\n{\n" + tempNode2.code + "}";
                        node.type = CheckType(tempNode1.type, tempNode2.type, '=');
                    }
                    catch
                    {
                        node.type = -11;
                    }
                    node.scopeList = MergeScopeLists(tempNode2.scopeList, scopeCounter++);
                    node.variableList = tempNode2.variableList;
                    break;
                case 80: //Ú->VÚ
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code;
                    node.type = CheckType(tempNode1.type, tempNode2.type, '=');
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, tempNode2.scopeList);
                    if (tempNode1.variableList == null) tempNode1.variableList = new List<string>();
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 81: //Ú->VX
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code;
                    node.type = tempNode1.type;
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, tempNode2.scopeList);
                    if (tempNode1.variableList == null) tempNode1.variableList = new List<string>();
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 82: //Ú->V
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = tempNode1.type;
                    node.scopeList = tempNode1.scopeList;
                    node.variableList = tempNode1.variableList;
                    break;
                case 83: //V->z|~}S
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 3; i++) tempNode1 = stack.Pop();
                    stack.Pop();
                    if (tempNode2.code.Contains('}'))
                    {
                        tempNode2.code = tempNode2.code.Substring(0, tempNode2.code.LastIndexOf('}'))
                            + tempNode2.code.Substring(tempNode2.code.LastIndexOf('}') + 1);
                    }
                    node.code = "case " + tempNode1.code + ":\n" + tempNode2.code + "break;";
                    node.type = tempNode1.type;
                    node.scopeList = tempNode2.scopeList;
                    node.variableList = tempNode2.variableList;
                    break;
                case 84: //X->á~}S
                    tempNode1 = stack.Pop();
                    for (int i = 0; i < 3; i++) stack.Pop();
                    if (tempNode1.code.Contains('}'))
                    {
                        tempNode1.code = tempNode1.code.Substring(0, tempNode1.code.LastIndexOf('}'))
                            + tempNode1.code.Substring(tempNode1.code.LastIndexOf('}') + 1);
                    }
                    node.code = "default:\n" + tempNode1.code + "break;";
                    node.scopeList = tempNode2.scopeList;
                    node.variableList = tempNode1.variableList;
                    break;
                case 85: //W->é(K)~}S
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "while(" + tempNode1.code + ")\n{" + tempNode2.code;
                    node.scopeList = tempNode2.scopeList;
                    node.variableList = tempNode2.variableList;
                    break;
                case 86: //H->í~}Sé(K)
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 3; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.code = "do\n{\n" + tempNode1.code + " while(" + tempNode2.code + ")";
                    node.scopeList = tempNode1.scopeList;
                    node.variableList = tempNode1.variableList;
                    break;
                case 87: //Y->ñ~}S(E)
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.code = "for(int repetir000" + repeatVariable + " = 0; repetir000" + repeatVariable + "<" + tempNode2.code + "; repetir000"
                        + repeatVariable++ + "++)\n{\n" + tempNode1.code;
                    node.scopeList = tempNode2.scopeList;
                    node.variableList = tempNode1.variableList;
                    break;
                case 88: //Z->ó(ÄËÖ~}S
                    tempNode4 = stack.Pop();
                    for (int i = 0; i < 3; i++) tempNode3 = stack.Pop();
                    tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.scopeList = tempNode4.scopeList;
                    if (tempNode1.variableList == null) tempNode1.variableList = new List<string>();
                    if (tempNode4.variableList == null) tempNode4.variableList = new List<string>();
                    if (FindMultipleDeclarations(tempNode1.variableList, tempNode4.variableList) != "")
                    {
                        //error, variable declared multiple times
                    }
                    node.variableList = tempNode1.variableList;
                    if (debugging && tempNode2.code.Length > 1)
                    {
                        string name = tempNode1.code.Substring(tempNode1.code.IndexOf(" ") + 1, tempNode1.code.IndexOf('=') - (tempNode1.code.IndexOf(" ") + 1));
                        string extraCode;
                        try
                        {
                            Symbol.GetSymbol(name);
                            if (debugVariableList.Contains(name))
                            {
                                extraCode = "\nint _found" + foundCounter + " = -1;\r\n"
                                    + "for (int _i1 = 0; _i1 < treeView1.Nodes.Count; _i1++){"
                                    + "if (treeView1.Nodes[_i1].Text == \"" + name + "\") _found" + foundCounter + " = _i1; }"
                                    + "if (_found" + foundCounter + " < 0){ treeView1.Nodes.Add(\"" + name + "\"); _found" + foundCounter + " = treeView1.Nodes.Count - 1; }";
                                tempNode4.code = extraCode + "\ntreeView1.Nodes[_found" + foundCounter++ + "].Nodes.Add((" + name
                                    + ").ToString());" + tempNode4.code;
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                    node.code = "for(" + tempNode1.code + tempNode2.code + tempNode3.code + "\n{\n" + tempNode4.code;
                    break;
                case 89: //Ä->TAp
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code + tempNode2.code + ";";
                    undeclaredVariable = "";
                    symbol = new Symbol();
                    symbol.dimensions = 0;
                    symbol.identifier = tempNode2.code.Substring(0, tempNode2.code.IndexOf('='));
                    symbol.dataType = tempNode1.type;
                    Symbol.AddSymbol(symbol);
                    node.variableList = new List<string>();
                    node.variableList.Add(symbol.identifier);
                    break;
                case 90: //Ä->ip
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    tempForVariable = tempNode1.code;
                    node.code = "int " + tempNode1.code + " = 0;";
                    break;
                case 92: //Ë->Kp
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ";";
                    break;
                case 91: //Ä->p
                case 93: //Ë->p
                    stack.Pop();
                    node.code = ";";
                    break;
                case 94: //Ö->A)
                case 95: //Ö->C)
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + ")";
                    break;
                case 96: //Ö->E)
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    if (tempForVariable != "") tempNode1.code = tempForVariable + " = " + tempNode1.code;
                    else node.type = -17;
                    node.code = tempNode1.code + ")";
                    break;
                case 97: //Ö->)
                    stack.Pop();
                    node.code = ")";
                    break;
                case 98: //\->°(E)
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 2; i++) stack.Pop();
                    node.code = "return " + tempNode1.code;
                    node.type = tempNode1.type;
                    if (returnType != 0 && CheckType(returnType, node.type, '=') <= 0 && node.type != -11)
                    {
                        if (node.type == 1 && returnType == 2 || node.type == 2 && returnType == 1) node.type = returnType;
                        node.type = -16;
                    }
                    returnType = node.type;
                    if (debugging)
                    {
                        node.code = "Console.WriteLine(\"Retornando:\" + (" + tempNode1.code + "));" + node.code;
                    }
                    possibleError = 0;
                    break;
                case 99: //\->°()
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.code = "return";
                    returnType = 8;
                    node.type = 8;
                    if (debugging)
                    {
                        node.code = "Console.WriteLine(\"Retornando de la función.\");" + node.code;
                    }
                    break;
                case 100: //_->ú~}Sü~}S
                    tempNode2 = stack.Pop();
                    for (int i = 0; i < 4; i++) tempNode1 = stack.Pop();
                    for (int i = 0; i < 3; i++) stack.Pop();
                    node.code = "try\n{\n" + tempNode1.code + "catch\n{\n" + tempNode2.code;
                    node.scopeList = MergeScopeLists(tempNode1.scopeList, tempNode2.scopeList);
                    if (tempNode1.variableList == null) tempNode1.variableList = new List<string>();
                    if (tempNode2.variableList == null) tempNode2.variableList = new List<string>();
                    tempNode1.variableList.AddRange(tempNode2.variableList);
                    node.variableList = tempNode1.variableList;
                    break;
                case 101: //`->i(Á)
                    for (int i = 0; i < 2; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.type = tempNode1.type;
                    try
                    {
                        node.type = GetTDSType(tempNode1.code);
                        if (!CheckMethodTypes(tempNode1.code, dataTypes))
                        {
                            node.type = -14;
                        }
                    }
                    catch
                    {
                        if (tempNode1.code == recursiveMethodName) node.type = recursiveMethodType;
                        else
                        {
                            switch (tempNode1.code)
                            {
                                case "Math.Sin":
                                case "Math.Cos":
                                case "Math.Tan":
                                case "Math.Sqrt":
                                    if (dataTypes.Count == 1 && CheckType(2, dataTypes[0], '=') > 0) node.type = 2;
                                    else node.type = -14;
                                    possibleError = 4;
                                    break;
                                case "Math.Pow":
                                    if (dataTypes.Count == 2
                                        && CheckType(2, dataTypes[0], '=') > 0 && CheckType(2, dataTypes[1], '=') > 0) node.type = 2;
                                    else node.type = -14;
                                    possibleError = 4;
                                    break;
                                case "System.Threading.Thread.Sleep":
                                    if (dataTypes.Count == 1 && CheckType(1, dataTypes[0], '=') > 0) node.type = 8;
                                    else node.type = -14;
                                    possibleError = 4;
                                    break;
                                case "reproducir":
                                    if (dataTypes.Count == 1 && CheckType(3, dataTypes[0], '=') > 0) node.type = 8;
                                    else node.type = -14;
                                    possibleError = 5;
                                    break;
                                case "escribir":
                                    if (dataTypes.Count == 1 && CheckType(3, dataTypes[0], '=') > 0) node.type = 8;
                                    else node.type = -14;
                                    break;
                                case "Content.Load<Texture2D>":
                                    if (dataTypes.Count == 1 && CheckType(3, dataTypes[0], '=') > 0)
                                    {
                                        node.type = 6;
                                        try
                                        {
                                            string source = tempNode2.code;
                                            source = source.Replace("\"", "");
                                            string destination = ".\\Archivos\\WindowsGame1\\WindowsGame1\\WindowsGame1Content\\";
                                            if (source.Contains('\\')) destination += source.Substring(source.LastIndexOf('\\') + 1);
                                            else destination += source;
                                            File.Copy(source, destination, true);
                                        }
                                        catch
                                        {
                                            //MessageBox.Show("Archivo inexistente: " + tempNode2.code);
                                            return false;
                                        }
                                    }
                                    else node.type = -14;
                                    tempNode2.code = tempNode2.code.Substring(0, tempNode2.code.LastIndexOf('.')) + "\"";
                                    possibleError = 5;
                                    break;
                                case "convertir":
                                    if (dataTypes.Count == 1) node.type = 9;
                                    else node.type = -14;
                                    break;
                                case "Convert.ToInt32":
                                    node.type = 1;
                                    break;
                                case "Convert.ToDouble":
                                    node.type = 2;
                                    break;
                                case "Convert.ToString":
                                    node.type = 3;
                                    break;
                                case "Convert.ToChar":
                                    node.type = 4;
                                    break;
                                case "Convert.ToBoolean":
                                    node.type = 5;
                                    break;
                                case "Console.Beep":
                                    if ((dataTypes.Count == 2 && CheckType(1, dataTypes[0], '=') > 0
                                        && CheckType(1, dataTypes[1], '=') > 0) || (dataTypes.Count == 0)) node.type = 8;
                                    else node.type = -14;
                                    possibleError = 4;
                                    break;
                                case "escribirtexto":
                                    if (dataTypes.Count == 2 && CheckType(3, dataTypes[0], '=') > 0
                                        && CheckType(3, dataTypes[1], '=') > 0) node.type = 8;
                                    else node.type = -14;
                                    possibleError = 5;
                                    break;
                                case "spriteBatch.Draw":
                                    if (dataTypes.Count == 4)
                                    {
                                        node.type = 8;
                                        string firstArgument = tempNode2.code.Substring(0, tempNode2.code.IndexOf(',') + 1);
                                        int secondCommaIndex = tempNode2.code.IndexOf(',', firstArgument.Length + 1);
                                        int thirdCommaIndex = tempNode2.code.IndexOf(',', secondCommaIndex + 1);
                                        string middleArgument = tempNode2.code.Substring(firstArgument.Length,  thirdCommaIndex - firstArgument.Length);
                                        middleArgument = "Convert.ToSingle(" + middleArgument.Substring(0, middleArgument.IndexOf(','))
                                            + ")," + "Convert.ToSingle(" + middleArgument.Substring(middleArgument.IndexOf(',') + 1) + ")";
                                        middleArgument = "new Vector2(" + middleArgument + ")";
                                        string lastARgument = tempNode2.code.Substring(thirdCommaIndex);
                                        tempNode2.code = firstArgument + middleArgument + lastARgument;
                                    }
                                    else node.type = -14;
                                    break;
                                case "longitud":
                                    if (dataTypes.Count == 1) node.type = 1;
                                    else node.type = -14;
                                    break;
                                case "rand.Next":
                                    if ((dataTypes.Count == 1 && CheckType(1, dataTypes[0], '=') > 0)
                                        || (dataTypes.Count == 2 && CheckType(1, dataTypes[0], '=') > 0
                                        && CheckType(1, dataTypes[1], '=') > 0)) node.type = 1;
                                    else node.type = -14;
                                    possibleError = 4;
                                    break;
                                default:
                                    if (tempNode1.code != null && tempNode1.code.Contains("ReadToEnd"))
                                    {
                                        if (dataTypes.Count == 1 && CheckType(3, dataTypes[0], '=') > 0) node.type = 3;
                                        else node.type = -14;
                                        possibleError = 5;
                                    }
                                    else undeclaredFunctions.Add(tempNode1.code);
                                    break;
                            }
                        }
                    }
                    node.code = tempNode1.code + "(" + tempNode2.code + ")";
                    dataTypes = new List<sbyte>(5);
                    break;
                case 103: ////Á->E
                    tempNode1 = stack.Pop();
                    node.code = tempNode1.code;
                    node.type = tempNode1.type;
                    dataTypes.Add(node.type);
                    break;
                case 104://M->{ T i ( ) ~ } S
                    tempNode3 = stack.Pop();
                    for (int i = 0; i < 5; i++) tempNode2 = stack.Pop();
                    tempNode1 = stack.Pop();
                    tempNode3.code = BackPatcher(tempNode3.code);
                    node.code = "static " + tempNode1.code + tempNode2.code + "()\n{\n" + tempNode3.code;
                    AddTDSMethod(tempNode1.type, tempNode2.token.text, new List<sbyte>());
                    dataTypes = new List<sbyte>(5);
                    stack.Pop();
                    node.variableList = tempNode3.variableList;
                    break;
                case 105://`->i ( )
                    for (int i = 0; i < 3; i++) tempNode1 = stack.Pop();
                    node.type = tempNode1.type;
                    node.code = tempNode1.code + "()";
                    try
                    {
                        node.type = GetTDSType(tempNode1.code);
                        if (!CheckMethodTypes(tempNode1.code, new List<sbyte>())) node.type = -14;
                    }
                    catch
                    {
                        if (tempNode1.code == "Console.ReadLine") node.type = 3;
                        else if (tempNode1.code == "rand.Next") node.type = 1;
                        else node.type = -15;
                    }
                    dataTypes = new List<sbyte>(5);
                    break;
                case 107://Ï->i[Á]=K
                case 108://Ï->i[Á]=E
                    tempNode3 = stack.Pop();
                    for (int i = 0; i < 3; i++) tempNode2 = stack.Pop();
                    for (int i = 0; i < 2; i++) tempNode1 = stack.Pop();
                    node.code = tempNode1.code + "[" + tempNode2.code + "] = " + tempNode3.code;
                    node.type = CheckType(1, tempNode2.type, '=');
                    try
                    {
                        if (node.type >= 0) node.type = CheckType(GetTDSType(tempNode1.code), tempNode3.type, '=');
                    }
                    catch
                    {

                    }
                    possibleError = 3;
                    tempVarName1 = tempNode1.code + "[" + tempNode2.code + "]";
                    tempVarName2 = tempNode2.code;
                    break;
            }
            if (node.type < 0) //Throw error
            {
                if (Error.errorsFound.Count == 0 || Error.errorsFound[Error.errorsFound.Count - 1].id != Error.GetSemanticError(node.type).id)
                {
                    Error error = Error.GetSemanticError(node.type), newError = new Error(error.id, error.description);
                    newError.lineNum = node.token.line;
                    newError.incorrectText = node.code;
                    newError.type = 3;
                    Error.errorsFound.Add(newError);
                    errorCount++;
                }
            }
            stack.Push(node);
            return true;
        }

        public static void AddTerminal(Semantic node)
        {
            switch (node.token.symbol)
            {
                case 'i':
                    if (node.token.text[0] >= '0' && node.token.text[0] <= '9')
                    {
                        int methodNum = int.Parse(node.token.text);
                        switch (methodNum)
                        {
                            case 10://sen
                                node.code = "Math.Sin";
                                break;
                            case 11://cos
                                node.code = "Math.Cos";
                                break;
                            case 12://tan
                                node.code = "Math.Tan";
                                break;
                            case 13://potencia
                                node.code = "Math.Pow";
                                break;
                            case 14://esperar
                                node.code = "System.Threading.Thread.Sleep";
                                break;
                            case 15://raizcuadrada
                                node.code = "Math.Sqrt";
                                break;
                            case 16://leer
                                node.code = "Console.ReadLine";
                                node.type = 3;
                                break;
                            case 17://escribir
                                node.code = "Console.WriteLine";
                                break;
                            case 18://convertir
                                node.type = 9;
                                break;
                            case 19://reproducir
                                backpatcher.Push(19);
                                node.code = "t" + temporaryCounter++ + ".Play()";
                                break;
                            case 20://sonido
                                node.code = "Console.Beep";
                                break;
                            case 21://leerras
                                node.code = "Content.Load<Texture2D>";
                                needGraphics = true;
                                break;
                            case 22://leertexto
                                backpatcher.Push(22);
                                node.code = "t" + temporaryCounter++ + ".ReadToEnd()";
                                break;
                            case 25://escribirtexto
                                backpatcher.Push(25);
                                node.code = "t" + temporaryCounter++ + ".Write";
                                break;
                            case 26://dibujarras
                                node.code = "spriteBatch.Draw";
                                break;
                            case 27://longitud
                                backpatcher.Push(27);
                                node.code = "l" + temporaryCounter++;
                                break;
                            case 28://azar
                                backpatcher.Push(28);
                                node.code = "rand.Next";
                                break;
                            case 29://convertirEnt
                                node.code = "Convert.ToInt32";
                                break;
                            case 30://convertirDec
                                node.code = "Convert.ToDouble";
                                break;
                            case 31://convertirTex
                                node.code = "Convert.ToString";
                                break;
                            case 32://convertirCar
                                node.code = "Convert.ToChar";
                                break;
                            case 33://convertirLog
                                node.code = "Convert.ToBoolean";
                                break;
                            default://error, method does not exist!

                                break;
                        }
                    }
                    else
                    {
                        node.code = node.token.text;
                        if (tempMethodType != 0 && tempMethodName == "") tempMethodName = node.code;
                        try
                        {
                            node.type = GetTDSType(node.code);
                        }
                        catch
                        {

                        }
                    }
                    break;
                case 's':
                    node.code = "break";
                    break;
                case 'e':
                    node.code = "int ";
                    node.type = 1;
                    declareType = 1;
                    break;
                case 'b':
                    node.code = "double ";
                    node.type = 2;
                    declareType = 2;
                    break;
                case 't':
                    node.code = "string ";
                    node.type = 3;
                    declareType = 3;
                    break;
                case 'g':
                    node.code = "char ";
                    node.type = 4;
                    declareType = 4;
                    break;
                case 'h':
                    node.code = "bool ";
                    node.type = 5;
                    declareType = 5;
                    break;
                case 'r':
                    node.code = "Texture2D ";
                    node.type = 6;
                    declareType = 6;
                    break;
                case 'k':
                    node.code = "Color ";
                    node.type = 7;
                    declareType = 7;
                    break;
                case 'ë':
                    node.code = "void ";
                    node.type = 0;
                    break;
                case 'v':
                    node.code = "true ";
                    node.type = 5;
                    break;
                case 'q':
                    node.code = "false ";
                    node.type = 5;
                    break;
                case 'l':
                    switch (node.token.text)
                    {
                        case "pi":
                            node.code = Math.PI.ToString();
                            node.type = 2;
                            break;
                        case "negro":
                            node.code = "Color.Black";
                            node.type = 7;
                            break;
                        case "azul":
                            node.code = "Color.Blue";
                            node.type = 7;
                            break;
                        case "verde":
                            node.code = "Color.Green";
                            node.type = 7;
                            break;
                        case "celeste":
                            node.code = "Color.LightBlue";
                            node.type = 7;
                            break;
                        case "rojo":
                            node.code = "Color.Red";
                            node.type = 7;
                            break;
                        case "rosado":
                            node.code = "Color.Pink";
                            node.type = 7;
                            break;
                        case "amarillo":
                            node.code = "Color.Yellow";
                            node.type = 7;
                            break;
                        case "blanco":
                            node.code = "Color.White";
                            node.type = 7;
                            break;
                        default:
                            node.code = node.token.text;
                            switch (node.token.lexeme.id)
                            {
                                case 26:
                                    node.type = 2;
                                    break;
                                case 27:
                                    node.type = 1;
                                    break;
                                case 28:
                                    node.type = 3;
                                    if (needGraphics)
                                    {
                                        graphics.Add(node.code);
                                        needGraphics = false;
                                    }
                                    break;
                                case 29:
                                    node.type = 4;
                                    break;
                            }
                            break;
                    }
                    break;
                case '=':
                    dataTypes.Clear();
                    break;
            }
            stack.Push(node);
        }

        private static sbyte CheckType(sbyte type1, sbyte type2, char op)
        {
            if (type1 < 0 || type2 < 0) return Math.Min(type1, type2);
            if (type1 == 0 || type2 == 0) return Math.Max(type1, type2);
            if (type1 == 9) return type2;
            if (type2 == 9) return type1;
            if (type1 > 10) type1 -= 10;
            if (type2 > 10) type2 -= 10;
            switch (type1)
            {
                case 1://int
                    switch (type2)
                    {
                        case 1://int
                        case 4://char
                            return 1;
                        case 2://decimal
                            if (op == '=') return -1;
                            return 2;
                        case 3://string
                            if (op == '+') return 3;
                            return -1;
                        default://bool, texture, color, or void
                            return -1;
                    }
                case 2://decimal
                    switch (type2)
                    {
                        case 1://int
                        case 2://decimal
                        case 4://char
                            return 2;
                        case 3://string
                            if (op == '+') return 3;
                            return -2;
                        default:
                            return -2;
                    }
                case 3://string
                    switch(type2)
                    {
                        case 1://int
                        case 2://decimal
                        case 4://char
                            if (op == '+') return 3;
                            return -3;
                        case 3://string
                            if (op == '=' || op == '+') return 3;
                            return -3;
                        default:
                            return -3;
                    }
                case 4://char
                    switch(type2)
                    {
                        case 1://int
                        case 2://decimal
                            if (op == '=') return -4;
                            return type2;
                        case 3://string
                            if (op == '+') return 3;
                            return -4;
                        case 4://char
                            if (op == '=') return 4;
                            return 1;
                        default:
                            return -4;
                    }
                case 5://bool
                    if (type2 == 5 && op == '=') return 5;
                    return -5;
                case 6://Texture2D
                    if (type2 == 6 && op == '=') return 6;
                    return -6;
                case 7://Color
                    if (type2 == 7 && op == '=') return 7;
                    return -7;
                case 8://void
                    if (type2 == 8 && op == '=') return 8;
                    return -8;
                default:
                    return -9;
            }
        }

        private static string PrintMatrixDimensions(string code)
        {
            string text = "";
            for (int i = 1; i < code.Split(',').Length; i++) text += ',';
            return text;
        }

        private static void AddTDSMethod(sbyte type, string name, List<sbyte> dimensions)
        {
            Symbol symbol = new Symbol();
            symbol.dataType = (sbyte)(type + 10);
            symbol.identifier = name;
            symbol.methodDataTypes = new List<sbyte>(dimensions);
            Symbol.AddSymbol(symbol);
        }

        private static int InsertTDSArray(sbyte type, string name, string code)
        {
            Symbol symbol = new Symbol();
            symbol.dataType = type;
            symbol.identifier = name;
            symbol.dimensions = (sbyte)code.Split(',').Length;
            Symbol.AddSymbol(symbol);
            return symbol.id;
        }

        private static string BackPatcher(string nodeCode)
        {
            while (backpatcher.Count > 0)
            {
                int code = backpatcher.Pop();
                string argument, tempCode;
                int startIndex;
                switch (code)
                {
                    case 19:
                        startIndex = nodeCode.IndexOf("t" + --temporaryCounter + ".Play()(") + 12;
                        argument = nodeCode.Substring(startIndex, nodeCode.IndexOf(')', startIndex) - startIndex);
                        tempCode = "δSystem.Media.SoundPlayer t" + temporaryCounter + " = new System.Media.SoundPlayer();δ\n";
                        tempCode += "t" + temporaryCounter + ".SoundLocation = " + argument + ";\n";
                        nodeCode = nodeCode.Substring(0, startIndex - 1) + nodeCode.Substring(startIndex + argument.Length + 1);
                        nodeCode = tempCode + nodeCode;
                        break;
                    case 22:
                        startIndex = nodeCode.IndexOf("t" + --temporaryCounter + ".ReadToEnd()(") + 17;
                        argument = nodeCode.Substring(startIndex, nodeCode.IndexOf(')', startIndex) - startIndex);
                        tempCode = "δSystem.IO.StreamReader t" + temporaryCounter + " = new System.IO.StreamReader(" + argument + ");δ\n";
                        nodeCode = nodeCode.Substring(0, startIndex - 1) + ";\nt" + temporaryCounter + ".Close()" + nodeCode.Substring(startIndex +
                            argument.Length + 1);
                        nodeCode = tempCode + nodeCode;
                        break;
                    case 25:
                        startIndex = nodeCode.IndexOf(',', nodeCode.IndexOf("t" + --temporaryCounter + ".Write(")) + 1;
                        argument = nodeCode.Substring(startIndex, nodeCode.IndexOf(')', startIndex) - startIndex);
                        tempCode = "δSystem.IO.StreamWriter t" + temporaryCounter + " = new System.IO.StreamWriter(" + argument + ");δ\n";
                        nodeCode = nodeCode.Substring(0, startIndex - 1) + ");\nt" + temporaryCounter + ".Close()" + nodeCode.Substring(startIndex +
                            argument.Length + 1);
                        nodeCode = tempCode + nodeCode;
                        break;
                    case 27:
                        startIndex = nodeCode.IndexOf("l" + --temporaryCounter + "(") + 5;
                        argument = nodeCode.Substring(startIndex, nodeCode.IndexOf(')', startIndex) - startIndex);
                        tempCode = argument + ".Length";
                        nodeCode = nodeCode.Substring(0, startIndex - 5) + tempCode + nodeCode.Substring(startIndex + argument.Length + 1);
                        break;
                    case 28:
                        if (!randUsed)
                        {
                            nodeCode = "δRandom rand = new Random(DateTime.Now.Year + DateTime.Now.Month * DateTime.Now.Day + DateTime.Now.Hour "
                                + "* (DateTime.Now.Minute - DateTime.Now.Second) + DateTime.Now.Millisecond);δ\n" + nodeCode;
                            randUsed = true;
                            classVariablesCode.Add("Random rand = new Random(DateTime.Now.Year + DateTime.Now.Month * DateTime.Now.Day + DateTime.Now.Hour "
                                + "* (DateTime.Now.Minute - DateTime.Now.Second) + DateTime.Now.Millisecond);");
                        }
                        break;
                }
            }
            return nodeCode;
        }

        private static sbyte GetTDSType(string identifier)
        {
            return Symbol.GetSymbol(identifier).dataType;
        }

        private static Semantic ConvertedNode(Semantic tempNode1, Semantic tempNode2)
        {
            switch (tempNode2.type)
            {
                case 1:
                    tempNode1.code = "Convert.ToInt32" + tempNode1.code;
                    break;
                case 2:
                    tempNode1.code = "Convert.ToDouble" + tempNode1.code;
                    break;
                case 3:
                    tempNode1.code = "Convert.ToString" + tempNode1.code;
                    break;
                case 4:
                    tempNode1.code = "Convert.ToChar" + tempNode1.code;
                    break;
                case 5:
                    tempNode1.code = "Convert.ToBoolean" + tempNode1.code;
                    break;
            }
            tempNode1.type = tempNode2.type;
            return tempNode1;
        }

        private static int GetArgumentDimensions(string code)
        {
            int dimensions = 1;
            int indexCounter = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '[') indexCounter++;
                else if (code[i] == ']') indexCounter--;
                else if (code[i] == ',' && indexCounter == 0) dimensions++;
            }
            return dimensions;
        }

        private static bool CheckMethodTypes(string identifier, List<sbyte> types)
        {
            Symbol symbol;
            try
            {
                symbol = Symbol.GetSymbol(identifier);
            }
            catch
            {
                return true;
            }
            if (symbol.methodDataTypes.Count != types.Count) return false;
            for (int i = 0; i < types.Count; i++)
            {
                if (CheckType(symbol.methodDataTypes[i], types[types.Count - i - 1], '=') <= 0) return false;
            }
            return true;
        }

        private static bool CheckIntDataInput(List<sbyte> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                if (types[i] != 1) return false;
            }
            return true;
        }

        private static List<int> MergeScopeLists(List<int> list1, List<int> list2)
        {
            List<int> finalList = new List<int>();
            if (list1 == null) list1 = new List<int>();
            if (list2 == null) list2 = new List<int>();
            for (int i = 0; i < list1.Count; i++) finalList.Add(list1[i]);
            for (int i = 0; i < list2.Count; i++) if (!finalList.Contains(list2[i])) finalList.Add(list2[i]);
            return finalList;
        }

        private static List<int> MergeScopeLists(List<int> list1, int num2)
        {
            List<int> finalList = new List<int>();
            if (list1 == null) list1 = new List<int>();
            for (int i = 0; i < list1.Count; i++) finalList.Add(list1[i]);
            finalList.Add(num2);
            return finalList;
        }

        private static string FindMultipleDeclarations(List<string> list1, List<string> list2)
        {
            if (list1 == null || list2 == null) return "";
            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i] == list2[j]) return list1[i];
                }
            }
            return "";
        }

        private static string SurroundTryCatch(string code, byte errorCode)
        {
            if (errorRecovery)
            {
                Error e = Error.GetRuntimeError(errorCode);
                if (code.Contains('\n')) code = "try\n{\n" + code + "\n}\ncatch\n{\nConsole.WriteLine(\"" + e.description + "\",\"" + e.id + "\");\nConsole.WriteLine(\""
                    + code.Substring(0, code.IndexOf('\n') - 1).Replace("\"", "") + "\");\n}\n";
                else code = "try\n{\n" + code + "\n}\ncatch\n{\nConsole.WriteLine(\"" + e.description + "\",\"" + e.id + "\");\nConsole.WriteLine(\""
                    + code.Replace("\"", "") + "\");\n}\n";
                possibleError = 0;
            }
            return code;
        }

        private static string CreateGraphicsCode(List<string> code1, string code2, string code3, string funciones)
        {
            StreamReader read = new StreamReader(".\\Archivos\\Game1.cs");
            string allCode = "";
            for (int i = 0; i < 16; i++)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            for (int i = 0; i < code1.Count; i++)
            {
                allCode += "public static " + code1[i];
                if (!code1[i].Contains(';')) allCode += ';';
                allCode += "\r\n";
            }
            allCode += funciones;
            for (int i = 0; i < 18; i++)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            allCode += code2.Substring(0, code2.LastIndexOf('}') - 1) + "\r\n\r\n";
            for (int i = 0; i < 54; i++)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            allCode += code3.Substring(0, code3.LastIndexOf('}') - 1) + "\r\n\r\n";
            while (!read.EndOfStream)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            read.Close();
            return allCode;
        }

        private static string CreateDebugCode(List<string> code1, string code2, string funciones)
        {
            StreamReader read = new StreamReader(".\\Archivos\\TreeApp\\TreeApp\\Form1 - Copy.cs");
            string allCode = "";
            for (int i = 0; i < 19; i++)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            for (int i = 0; i < code1.Count; i++)
            {
                if (code1[i].Contains("treeView1.Nodes")) code1[i] = code1[i].Substring(0, code1[i].IndexOf("treeView1.Nodes"));
                allCode += "public static " + code1[i];
                if (!code1[i].Contains(';')) allCode += ';';
                allCode += "\r\n";
            }
            funciones = funciones.Replace("static", "");
            allCode += funciones;
            for (int i = 0; i < 9; i++)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            allCode += code2.Substring(0, code2.LastIndexOf('}') - 1) + "\r\n\r\n";
            while (!read.EndOfStream)
            {
                allCode += read.ReadLine() + "\r\n";
            }
            read.Close();
            return allCode;
        }

        private static void GraphicsContentWriter()
        {
            if (graphics.Count > 0)
            {
                StreamReader read = new StreamReader(".\\Archivos\\WindowsGame1\\WindowsGame1\\WindowsGame1Content\\WindowsGame1Content (2).contentproj");
                string line, allText = "";
                do
                {
                    line = read.ReadLine();
                    allText += line + "\r\n";
                } while (!line.Contains("</ItemGroup>"));
                allText += "<ItemGroup>\r\n";
                for (int i = 0; i < graphics.Count; i++)
                {
                    if (graphics[i].Contains('\\')) graphics[i] = "\"" + graphics[i].Substring(graphics[i].LastIndexOf('\\') + 1, graphics[i].LastIndexOf('.')) + "\"";
                    string tempText = "<Compile Include=" + graphics[i] + ">\r\n";
                    if (graphics[i].Contains('\\')) tempText += "<Name>" + graphics[i].Substring(graphics[i].LastIndexOf('\\') + 1,
                        graphics[i].LastIndexOf('.')) + "</Name>\r\n";
                    else tempText += "<Name>" + graphics[i].Substring(0, graphics[i].LastIndexOf('.')).Replace("\"", "") + "</Name>\r\n";
                    tempText += "<Importer>TextureImporter</Importer>\r\n";
                    tempText += "<Processor>TextureProcessor</Processor>\r\n";
                    tempText += "</Compile>\r\n";
                    allText += tempText;
                }
                read.ReadLine();
                while (!read.EndOfStream)
                {
                    line = read.ReadLine();
                    allText += line + "\r\n";
                }
                read.Close();
                StreamWriter write = new StreamWriter(".\\Archivos\\WindowsGame1\\WindowsGame1\\WindowsGame1Content\\WindowsGame1Content.contentproj");
                write.Write(allText);
                write.Close();
            }
        }

        public static void EliminateGlobalVariableErrors()
        {
            for (int i = 0; i < Error.errorsFound.Count; i++)
            {
                int errorId = Error.errorsFound[i].id;
                if (!(errorId == 409 || errorId == 410 || errorId == 411 || errorId == 416)) break;
                bool removed = false;
                for (int j = 0; j < classVariableNames.Count; j++)
                {
                    if (Error.errorsFound[i].incorrectText.Contains(classVariableNames[j]))
                    {
                        Error.errorsFound.RemoveAt(i);
                        removed = true;
                        errorCount--;
                        break;
                    }
                }
                if (removed) i--;
            }
        }
    }
}
