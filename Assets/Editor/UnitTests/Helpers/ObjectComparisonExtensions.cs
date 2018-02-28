// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Assets.Editor.UnitTests.Helpers
{
    public static class ObjectComparisonExtensions
    {
        // With help from https://stackoverflow.com/questions/506096/comparing-object-properties-in-c-sharp
        public static bool EqualByPublicProperties<TToCompare>(TToCompare actual, TToCompare expected, params string[] ignore)
           where TToCompare : class
        {
            if (actual != null && expected != null)
            {
                var type = typeof(TToCompare);
                var ignoreList = new List<string>(ignore);

                var unequalProperties =
                    from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name) && pi.GetUnderlyingType().IsSimpleType() && 
                        pi.GetIndexParameters().Length == 0
                    let actualValue = type.GetProperty(pi.Name).GetValue(actual, null)
                    let expectedValue = type.GetProperty(pi.Name).GetValue(expected, null)
                    where actualValue != expectedValue && (actualValue == null || !actualValue.Equals(expectedValue))
                    select actualValue;

                var unequalObjects =
                    from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name) && !pi.GetUnderlyingType().IsSimpleType() &&
                          pi.GetIndexParameters().Length == 0
                    let actualValue = type.GetProperty(pi.Name).GetValue(actual, null)
                    let expectedValue = type.GetProperty(pi.Name).GetValue(expected, null)
                    where actualValue != expectedValue // Only reference compares currently supported (no dynamic keyword in unity :()
                    select actualValue;

               return !unequalProperties.Any() &&  !unequalObjects.Any();
            }
            return actual == expected;
        }
    }

    public static class TypeExtensions
    {
        // With help from http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
        public static bool IsSimpleType(
            this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new[]
                {
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}
