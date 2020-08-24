using System;
using System.Linq;
using System.Collections.Generic;

public static class RNG {
    // public static int seed {get; private set;} = Environment.TickCount;
    // Errors:
    // 109695750

    public static int seed {get; private set;} = 109695750;

    public static System.Random rand {get; private set;} = new System.Random(RNG.seed);

    public static String stringSeed {get; private set;}

    public static int Next(){
        return rand.Next();
    }

    public static int Next(int max){
        return rand.Next(max);
    }

    public static int Next(int min, int max){
        return rand.Next(min, max);
    }

    public static T RandomElementFromList<T>(IList<T> list){
        if (list.Count == 0){
            throw new ArgumentException("List has no elements");
        }
        int index = rand.Next(list.Count);
        return list[index];
    }

    public static T RandomElementFromListExcluding<T>(IList<T> list, IList<T> excluding){
        List<T> selectable = list.Where(aElement => !excluding.Contains(aElement)).ToList();
        return RandomElementFromList(selectable);
    }

    public static T RandomElementFromDictionary<Key, T>(Dictionary<Key,T> dictionary){
        List<T> newList = dictionary.Values.ToList();
        return RandomElementFromList(newList);
    }

    public static T RandomElementFromDictionaryExcluding<Key, T>(Dictionary<Key,T> dictionary, IList<T> excluding){
        List<T> newList = dictionary.Values.ToList();
        return RandomElementFromListExcluding(newList, excluding);       
    }

    public static T RandomElementFromDictionaryExcludingKey<Key,T>(Dictionary<Key,T> dictionary, IList<Key> excluding){
        List<T> excludedElements = excluding.Select(aExludedKey => dictionary[aExludedKey]).ToList();
        return RandomElementFromDictionaryExcluding(dictionary, excludedElements);
    }

    public static String RandomString(int length){
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    public static void ResetSeed(String seed){
        int newSeed = seed.GetHashCode();
        RNG.seed = newSeed;
        stringSeed = seed;
        rand = new System.Random(newSeed);
    }
}