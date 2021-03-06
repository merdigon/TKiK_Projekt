﻿using MapFileReader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapFileReader.Tokens
{
    public class TokenDictListBuilder
    {
        List<TokenDictionary> tokenList;

        public TokenDictListBuilder()
        {
            tokenList = new List<TokenDictionary>();
        }

        public TokenDictListBuilder CreateTokenList()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Type[] kmlTypes = myAssembly.ManifestModule.GetTypes().Where(p => p.Namespace.EndsWith("KMLObjects")).ToArray();

            foreach (Type type in kmlTypes)
            {
                //jeżeli klasa nie ma atrybutu oznaczającego klasy pliku KML
                if (type.GetCustomAttributes(true).Where(p => p is KMLMarkClassAttribute).FirstOrDefault() == null)
                    continue;

                foreach(PropertyInfo pinfo in type.GetProperties())
                {
                    KMLMarkNameAttribute kmlAtribute = (KMLMarkNameAttribute)((object[])pinfo.GetCustomAttributes(true)).Where(p => p is KMLMarkNameAttribute).FirstOrDefault();
                    if (kmlAtribute != null)
                    {
                        if(!tokenList.Select(p => p.Value).Contains(kmlAtribute.Name))
                        {
                            tokenList.Add(new TokenDictionary(kmlAtribute.Name));
                        }
                    }
                }
            }
            return this;
        }

        public List<TokenDictionary> GetTokenList()
        {
            return this.tokenList ?? new List<TokenDictionary>();
        }
    }
}
