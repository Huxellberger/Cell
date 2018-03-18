// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Scripts.Core
{
    public static class ReflectionFunctions 
    {
        public static IEnumerable<Type> GetTypesWithAttribute<TAttributeType>(Assembly assembly)
            where TAttributeType : Attribute
        {
            foreach (Type type in assembly.GetTypes())
            {
                var occurrencesOfAttribute = (TAttributeType[])type.GetCustomAttributes(typeof(TAttributeType), true);
                if (occurrencesOfAttribute.Length > 0)
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<TReturnType> GetAttributeValuesOnTarget<TAttributeType, TReturnType>(object inObject)
            where TAttributeType : Attribute
            where TReturnType : class
        {
            var type = inObject.GetType();
            foreach (var propertyInfo in
                type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(propertyInfo, typeof(TAttributeType)))
                {
                    var value = (TReturnType)propertyInfo.GetValue(inObject);
                    yield return value;
                }
            }
        }
    }
}
