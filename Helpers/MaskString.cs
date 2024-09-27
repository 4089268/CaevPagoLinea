using System;
using System.Collections.Generic;

namespace CAEV.PagoLinea {

    public class MaskString {

        public static string Mask(string input) {
            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++){
                if (words[i].Length > 2) {
                    string firstTwoChars = words[i].Substring(0, 2);
                    string maskedPart = new string('*', words[i].Length - 2);
                    words[i] = firstTwoChars + maskedPart;
                }
            }
            return string.Join(" ", words);
        }

    }
}