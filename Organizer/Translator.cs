using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{
    internal static class Translator
    {
        public static string Translate(string word) 
        {
            switch (word) 
            {
                case "Description":
                    {
                        return "Описание";
                    }
                case "Day":
                    {
                        return "Дни";
                    }
                case "Type":
                    {
                        return "Тип";
                    }
                case "Id":
                    {
                        return "ID";
                    }
            }
            return $"{word}";
        }
    }
}
