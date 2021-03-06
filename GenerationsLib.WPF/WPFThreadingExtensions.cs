﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;

namespace GenerationsLib.WPF
{
    public static class WPFThreadingExtensions
    {
        /// <summary>
        /// Simple helper extension method to marshall to correct
        /// thread if its required
        /// </summary>
        /// <param name="""control""">The source control</param>
        /// <param name="""methodcall""">The method to call</param>
        /// <param name="""priorityForCall""">The thread priority</param>
        public static void InvokeIfRequired(
            this DispatcherObject control,
           Action methodcall,
           DispatcherPriority priorityForCall)
        {
            //see if we need to Invoke call to Dispatcher thread
            if (control.Dispatcher.Thread != Thread.CurrentThread)
                control.Dispatcher.Invoke(priorityForCall, methodcall);
            else
                methodcall();
        }
    }
}
