﻿using Compiler.Tokens;
using Compiler.Scanner.CharAnalizeLinks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compiler.Scanner
{
    public class Scanner
    {
        public string Input { get; set; }
        public List<Token> tokens { get; set; }
        public HeadLink hLink { get; set; }

        public Scanner()
        {
            tokens = new List<Token>();
            using(StreamReader stR = new StreamReader("C:\\Users\\Szymon\\Desktop\\code.txt"))
            {
                Input = stR.ReadToEnd();
            }

            // = "(34*+\"2.4  .\"class&!=>=(-4)";
            hLink = new HeadLink();
            InitAnalizeChain();
        }

        void InitAnalizeChain()
        {
            hLink = new HeadLink();
            hLink.SetScanner(this);
            hLink.RegisterNext(new StringLink());
            hLink.RegisterNext(new MultipleLineCommentLink());
            hLink.RegisterNext(new OneLineCommentLink());
            hLink.RegisterNext(new CharLink());

            hLink.RegisterNext(new BracketsLink());
            hLink.RegisterNext(new BasicMathLink());
            hLink.RegisterNext(new DigitLink());
            hLink.RegisterNext(new DotLink());
            hLink.RegisterNext(new QuotationMarkLink());
            hLink.RegisterNext(new SpaceLink());
            hLink.RegisterNext(new EqualMarkLink());
            hLink.RegisterNext(new MoreLessMarkLink());
            hLink.RegisterNext(new NegationMarkLink());
            hLink.RegisterNext(new OrAndMarkLink());
            hLink.RegisterNext(new EndOfCodeLineMarkLink());
            hLink.RegisterNext(new SlashMarkLink());
            hLink.RegisterNext(new EndOfLineLink());
            hLink.RegisterNext(new IgnoreMarkLink());
            hLink.RegisterNext(new ApostropheMarkLink());
            hLink.RegisterNext(new CommaMarkLink());
            hLink.RegisterNext(new PunctuationMarkLink());

            hLink.RegisterNext(new OtherLink());
        }

        public void AnalizeInput()
        {
            Token currentToken = null;
            foreach (char charac in Input)
            {
                currentToken = AnalizeCharac(charac, currentToken);
            }
            if (currentToken != null)
            {
                AddToken(currentToken);
            }
            CheckErrorBrackets();
        }

        public Token AnalizeCharac(char charac, Token tempToken)
        {

            return hLink.GetRequest(tempToken, charac);
        }

        public void AddToken(Token tokToAdd)
        {
            if (tokToAdd.Type == TokenType.LICZBA_WYMIERNA && tokToAdd.Value.Last() == '.')
            {
                tokToAdd.Type = TokenType.ERROR;
            }
            if (tokToAdd.Type == TokenType.STRING && tokToAdd.Value.Last() != '"')
            {
                tokToAdd.Type = TokenType.ERROR;
            }
            if (tokToAdd.Type == TokenType.OP_LOG && (tokToAdd.Value.Equals("|") || tokToAdd.Value.Equals("&")))
            {
                tokToAdd.Type = TokenType.ERROR;
            }
            tokens.Add(tokToAdd);
        }
        
        public void ShowTokens()
        {
            foreach (Token tok in tokens)
            {
                Console.WriteLine(tok.Type.ToString() + " - " + tok.Value);
            }
        }

        public void CheckErrorBrackets()
        {
            var bracketsTokens = tokens.Where(p => p.Type == TokenType.NAWIAS);
            Stack<Token> tokenStack = new Stack<Token>();

            foreach (Token tok in bracketsTokens)
            {
                if (tok.Value.Equals("(") || tok.Value.Equals("[") || tok.Value.Equals("{"))
                    tokenStack.Push(tok);
                else if(tok.Value.Equals(")"))
                {
                    if (!tokenStack.Peek().Value.Equals("("))
                        tok.Type = TokenType.ERROR;
                    else
                        tokenStack.Pop();
                }
                else if (tok.Value.Equals("]"))
                {
                    if (!tokenStack.Peek().Value.Equals("["))
                        tok.Type = TokenType.ERROR;
                    else
                        tokenStack.Pop();
                }
                else if (tok.Value.Equals("}"))
                {
                    if (!tokenStack.Peek().Value.Equals("{"))
                        tok.Type = TokenType.ERROR;
                    else
                        tokenStack.Pop();
                }
            }

            if (tokenStack.Count > 0)
            {
                foreach (Token tok in tokenStack)
                    tok.Type = TokenType.ERROR;
            }
        }

    }
}
