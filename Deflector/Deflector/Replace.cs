﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Deflector
{
    public static class Replace
    {
        public static Func<MethodBase> Method<TObject>(Expression<Action<TObject>> expression)
        {
            return () => GetMethodByCall(expression);
        }
        public static Func<MethodBase> Method(Expression<Action> expression)
        {
            return () => GetMethodByCall(expression);
        }

        public static Func<MethodBase> Method<TObject, T>(Expression<Func<TObject, Action<T>>> expression)
        {
            return () => GetMethodImpl(expression);
        }

        public static Func<MethodBase> Method<T>(Expression<Func<Action<T>>> expression)
        {
            return () => GetMethodImpl(expression);
        }

        public static void With<T1, T2, T3, T4, T5, T6, T7>(this Func<MethodBase> methodSelector,
            Action<T1, T2, T3, T4, T5, T6, T7> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4, T5, T6>(this Func<MethodBase> methodSelector,
            Action<T1, T2, T3, T4, T5, T6> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4, T5>(this Func<MethodBase> methodSelector,
            Action<T1, T2, T3, T4, T5> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4>(this Func<MethodBase> methodSelector,
            Action<T1, T2, T3, T4> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3>(this Func<MethodBase> methodSelector,
            Action<T1, T2, T3> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }
        public static void With<T1, T2>(this Func<MethodBase> methodSelector, Action<T1, T2> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1>(this Func<MethodBase> methodSelector, Action<T1> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }
        public static void With(this Func<MethodBase> methodSelector, Action implementation)
        {
            methodSelector.WithDelegate(implementation);
        }
        public static void With<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<MethodBase> methodSelector,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4, T5, T6, TResult>(this Func<MethodBase> methodSelector,
            Func<T1, T2, T3, T4, T5, T6, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4, T5, TResult>(this Func<MethodBase> methodSelector,
            Func<T1, T2, T3, T4, T5, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, T4, TResult>(this Func<MethodBase> methodSelector,
            Func<T1, T2, T3, T4, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, T2, T3, TResult>(this Func<MethodBase> methodSelector,
            Func<T1, T2, T3, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }
        public static void With<T1, T2, TResult>(this Func<MethodBase> methodSelector, Func<T1, T2, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void With<T1, TResult>(this Func<MethodBase> methodSelector, Func<T1, TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }
        public static void With<TResult>(this Func<MethodBase> methodSelector, Func<TResult> implementation)
        {
            methodSelector.WithDelegate(implementation);
        }

        public static void WithDelegate(this Func<MethodBase> methodSelector, MulticastDelegate implementation)
        {
            var targetMethod = methodSelector();
            // Verify that the method and the implementation have compatible signatures
            var method = implementation.Method;
            var hasCompatibleMethodSignature = method.HasCompatibleMethodSignatureWith(targetMethod);
            if (!hasCompatibleMethodSignature)
                throw new InvalidOperationException(
                    string.Format(
                        "The delegate you provided does not have a compatible signature with the '{0}' method.",
                        targetMethod.Name));

            MethodCallProviderRegistry.AddProvider(new SingleMethodCallProvider(targetMethod, implementation));
        }        

        private static MethodInfo GetMethodImpl(LambdaExpression expression)
        {
            var body = expression.Body;
            var unaryExpression = (UnaryExpression)body;
            var operand = (MethodCallExpression)unaryExpression.Operand;
            var methodCallExpression = operand;

            var arguments = methodCallExpression.Arguments;
            var constantExpression = (ConstantExpression)arguments.Last();

            return (MethodInfo)constantExpression.Value;
        }

        private static MethodInfo GetMethodByCall(LambdaExpression expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}