using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CloneHelper 
{
    public class BaseCloneObject : ICloneable
    {
        public object Clone()
        {
            object obj = new BaseCloneObject();
            //字段
            FieldInfo[] fields = typeof(BaseCloneObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                field.SetValue(obj, field.GetValue(this));
            }
            //属性
            PropertyInfo[] properties = typeof(BaseCloneObject).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(obj, property.GetValue(this));
            }
            return obj;
        }
    }

    public static object DeepClone(object obj)
    {
        Type type = obj.GetType();
        //对于没有公共无参构造函数的类型此处会报错
        object returnObj = Activator.CreateInstance(type);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];
            var fieldValue = field.GetValue(obj);
            ///值类型，字符串，枚举类型直接把值复制，不存在浅拷贝
            if (fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
            {
                field.SetValue(returnObj, fieldValue);
            }
            else
            {
                field.SetValue(returnObj, DeepClone(fieldValue));
            }
        }
        //属性
        PropertyInfo[] properties = typeof(BaseCloneObject).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];
            var propertyValue = property.GetValue(obj);
            if (propertyValue.GetType().IsValueType || propertyValue.GetType().Equals(typeof(System.String)) || propertyValue.GetType().IsEnum)
            {
                property.SetValue(returnObj, propertyValue);
            }
            else
            {
                property.SetValue(returnObj, DeepClone(propertyValue));
            }
        }

        return returnObj;
    }
}
