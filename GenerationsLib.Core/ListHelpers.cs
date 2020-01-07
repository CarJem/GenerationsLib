﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationsLib.Core
{
    public static class ListHelpers
    {
        public static int MoveSelectedIndex(int listCount, int oldIndex, int newIndex)
        {
            if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= listCount) || (0 > newIndex) || (newIndex >= listCount)) return oldIndex;
            else return newIndex;
        }
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {

            // exit if possitions are equal or outside array
            if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
                (newIndex >= list.Count)) return;
            // local variables
            var i = 0;
            T tmp = list[oldIndex];
            // move element down and shift other elements up
            if (oldIndex < newIndex)
            {
                for (i = oldIndex; i < newIndex; i++)
                {
                    list[i] = list[i + 1];
                }
            }
            // move element up and shift other elements down
            else
            {
                for (i = oldIndex; i > newIndex; i--)
                {
                    list[i] = list[i - 1];
                }
            }
            // put element from position 1 to destination
            list[newIndex] = tmp;
        }
    }
}
