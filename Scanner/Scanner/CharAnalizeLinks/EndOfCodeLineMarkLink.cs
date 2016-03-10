﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scanner.Tokens;

namespace Scanner.CharAnalizeLinks
{
    public class EndOfCodeLineMarkLink : LinkBase
    {
        public override Token GetRequest(Token tempToken, char charac)
        {
            if (charac == ';')
            {
                if (tempToken != null)
                {
                    if (tempToken.Type == TokenType.NIEZNANE)
                    {
                        tempToken.Type = TokenManager.CheckUnknownElem(tempToken, false);
                    }
                    AddToken(tempToken);
                }
                AddToken(new Token() { Type = TokenType.END_OF_CODE_LINE, Value = charac.ToString() });
                return null;
            }

            return base.GetRequest(tempToken, charac);
        }
    }
}
