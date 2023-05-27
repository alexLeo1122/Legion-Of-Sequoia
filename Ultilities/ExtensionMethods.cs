
//using UnityEngine;
using System;
using System.Collections.Generic;

namespace RPG.Ultilities
{
    public static class ExtensionMethods
    {
        public static T[] Shuffle<T>(this T[] array)
        {

            var random = new Random();
            for (int i = array.Length -1; i > 1; i--)
            {
                int tempIndex = random.Next(i+1);
                var tempNumber = array[tempIndex];
                array[tempIndex] = array[i];
                array[i] = tempNumber;
            }

            return array;
        }

        public static IList<T> Shuffle<T>(this IList<T> iList)
        {
            var random = new Random();
            for (int i = iList.Count - 1; i > 0; i--)
            {
                int tempIndex = random.Next(i + 1);
                T tempItem = iList[tempIndex];
                iList[tempIndex] = iList[i];
                iList[i] = tempItem;
            }
            return iList;
        }

    }

}




