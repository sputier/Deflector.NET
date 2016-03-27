﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Deflector
{
    public class SingleMethodCallBinder : IMethodCallBinder
    {
        private readonly MethodBase _targetMethod;
        private readonly MulticastDelegate _implementation;

        public SingleMethodCallBinder(MethodBase targetMethod, MulticastDelegate implementation)
        {
            _targetMethod = targetMethod;
            _implementation = implementation;
        }

        public void AddMethodCalls(object target, MethodBase hostMethod, IEnumerable<MethodBase> interceptedMethods, IMethodCallMap methodCallMap,
            StackTrace stackTrace)
        {
            // Map the implementation to the most compatible method signature
            var bestMatch = interceptedMethods.GetBestMatch(_targetMethod);
            if (bestMatch == null)
                return;

            // Verify the delegate signature
            if (!bestMatch.HasCompatibleMethodSignatureWith(_implementation.Method))
                return;

            methodCallMap.Add(method => method == bestMatch, new DelegateMethodCall(_implementation));
        }
    }
}