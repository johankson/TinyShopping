﻿using System;
using System.Linq;

namespace TinyShopping.Api.Extensions
{
    public class CopyAttribute : Attribute
    {
        public bool Exclude
        {
            get;
            set;
        }
        public string FromProperty { get; set; }
    }

    public interface IRunOnCopy
    {
        void DataCopied(object fromObject);
    }

    public static class GenericExtensions
    {

        public static void MemberviseCopyTo(this object a, object b, bool overwrite = true)
        {
            var aType = a.GetType();
            var bType = b.GetType();
            var copyAttr = typeof(CopyAttribute);
            foreach (var prpInfo in bType.GetProperties())
            {
                if (prpInfo.CanWrite)
                {
                    var customAttr = prpInfo.GetCustomAttributes(copyAttr, true).OfType<CopyAttribute>().FirstOrDefault();
                    var copyFromName = prpInfo.Name;
                    if (customAttr == null || !customAttr.Exclude)
                    {
                        if (customAttr != null && !string.IsNullOrEmpty(customAttr.FromProperty))
                        {
                            copyFromName = customAttr.FromProperty;
                        }
                        var fromProperty = bType.GetProperty(copyFromName);
                        if (fromProperty != null && fromProperty.CanRead)
                        {
                            var valueToAdd = fromProperty.GetValue(a, null);
                            if (valueToAdd != null)
                            {
                                if (overwrite || !IsEmpty(valueToAdd))
                                    prpInfo.SetValue(b, valueToAdd);
                            }
                        }
                    }

                }
            }
            var runOnCopy = b as IRunOnCopy;
            if (runOnCopy != null)
            {
                runOnCopy.DataCopied(a);
            }
        }

        private static bool IsEmpty(object valueToAdd)
        {
            if (valueToAdd == null)
                return true;
            if (valueToAdd is string s)
            {
                return string.IsNullOrEmpty(s);
            }
            if (valueToAdd is int i)
            {
                return i != 0;
            }
            if (valueToAdd is double d)
            {
                return d != 0;
            }
            if (valueToAdd is float f)
            {
                return f != 0;
            }
            if (valueToAdd is DateTime date)
            {
                return date > DateTime.MinValue;
            }
            return false;
        }
    }
}
