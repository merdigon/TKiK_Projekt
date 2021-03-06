﻿using Compiler.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Scanner.CharAnalizeLinks
{
    public class StringLink : LinkBase
    {
        public override Token GetRequest(Token tempToken, char charac)
        {
            if (tempToken != null)
            {
                if (tempToken.Type == TokenType.STRING)
                {
                    if (charac == '"' && tempToken.Value.Last()!='\\')
                    {
                        tempToken.Value += charac;
                        AddToken(tempToken);
                        return null;
                    }
                    else if(charac == '\n')
                    {
                        AddToken(tempToken);
                        AddToken(new Token() { Type = TokenType.END_OF_LINE, Value = charac.ToString() });
                        return null;
                    }
                    else
                    {
                        tempToken.Value += charac;
                        return tempToken;
                    }
                }
            }

            return base.GetRequest(tempToken, charac);
        }
    }
}
