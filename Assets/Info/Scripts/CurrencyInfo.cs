using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Currency", menuName = "Currency/Create Currency")]
public class CurrencyInfo : ScriptableObject {
    public string Title;
    public List<CurrencyInteger> Integers = new();

    public string ConvertToString(BigFloat amount) {
        if (amount < 1) return "0";

        BigFloat power = 1;
        string name = "";

        for (int i = 0; i < Integers.Count; i++) {
            if (amount >= power) {
                name = Integers[i].ShortName;
                power *= 1000;
            } else {
                power /= 1000;
                break;
            }
        }

        return $"{BigFloat.Round(amount / power * 1000) / 1000} {name}";

    }

    [Serializable]
    public struct CurrencyInteger {
        public string Name;
        public string ShortName;
        // public string Value;
    }
}
