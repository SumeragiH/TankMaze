using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 使用方法 SaveDate(要存储的对象，"自己设置的键名") LoadDate（要读取的对象的type，"设置的键名"）并且用对象装载
/// </summary>
public class GameDate
{
    private GameDate() { }
    private static GameDate _instance = new GameDate();

    public static GameDate Instance => _instance;

    public void SaveDate(object obj, string keyName)
    {
        if (obj == null)
            return;

        Type type = obj.GetType();
        Type[] types = type.GetGenericArguments();

        string baseKey = type.Name + "_" + keyName;
        // 根据不同类型采用不同的存储策略
        if (types.Length == 0) // 非泛型类型
        {
            SaveNonGenericType(obj, baseKey);
        }
        else if (types.Length == 1 && typeof(IList).IsAssignableFrom(type)) // 泛型List
        {
            SaveGenericList(obj, baseKey, types[0]);
        }
        else if (types.Length == 2 && typeof(IDictionary).IsAssignableFrom(type)) // 泛型Dictionary
        {
            SaveGenericDictionary(obj, baseKey, types[0], types[1]);
        }

        PlayerPrefs.Save();
    }

    // 处理非泛型类型 - 移除索引
    private void SaveNonGenericType(object obj, string baseKey)
    {
        Type type = obj.GetType();
        FieldInfo[] fieldInfos = type.GetFields();
        foreach (FieldInfo field in fieldInfos)
        {
            // 修正：移除索引，使用稳定的键名
            string fieldKey = baseKey + "_" + field.FieldType.Name + "_" + field.Name;
            object value = field.GetValue(obj);
            SaveFieldValue(value, fieldKey, field.FieldType);
        }
    }

    // 处理泛型List
    private void SaveGenericList(object obj, string baseKey, Type elementType)
    {
        IList list = obj as IList;
        if (list == null)
        {
            return;
        }

        // 存储List长度
        string lengthKey = baseKey + "_List_Length";
        PlayerPrefs.SetInt(lengthKey, list.Count);

        // 存储每个元素，使用了索引值
        for (int i = 0; i < list.Count; i++)
        {
            string listKey = baseKey + "_List_Element_" + i;
            SaveFieldValue(list[i], listKey, elementType);
        }
    }

    // 处理泛型Dictionary
    private void SaveGenericDictionary(object obj, string baseKey, Type keyType, Type valueType)
    {
        IDictionary dict = obj as IDictionary;
        if (dict == null) return;

        // 存储Dictionary长度
        string lengthKey = baseKey + "_Dict_Length";
        PlayerPrefs.SetInt(lengthKey, dict.Count);

        // 存储键值对，使用了索引值
        int index = 0;
        foreach (object key in dict.Keys)
        {
            // 存储Key
            string keyKey = baseKey + "_Key_" + index;
            SaveFieldValue(key, keyKey, keyType);

            // 存储Value
            string valueKey = baseKey + "_Value_" + index;
            SaveFieldValue(dict[key], valueKey, valueType);

            index++;
        }
    }

    // 统一的字段值存储方法
    private void SaveFieldValue(object value, string fullKey, Type fieldType)
    {
        if (value == null)
        {
            return;
        }

        Type valueType = value.GetType();

        // 基本类型存储
        if (valueType == typeof(int))
        {
            PlayerPrefs.SetInt(fullKey, (int)value);
        }
        else if (valueType == typeof(string))
        {
            PlayerPrefs.SetString(fullKey, (string)value);
        }
        else if (valueType == typeof(float))
        {
            PlayerPrefs.SetFloat(fullKey, (float)value);
        }
        else if (valueType == typeof(bool))
        {
            PlayerPrefs.SetInt(fullKey, (bool)value ? 1 : 0);
        }
        // 递归处理集合类型
        else if (typeof(IList).IsAssignableFrom(valueType))
        {
            SaveDate(value, fullKey);
        }
        else if (typeof(IDictionary).IsAssignableFrom(valueType))
        {
            SaveDate(value, fullKey);
        }
        // 递归处理自定义类型
        else if (!valueType.IsPrimitive && valueType != typeof(string))
        {
            SaveDate(value, fullKey);
        }
    }

    public object LoadDate(Type t, string keyName)
    {
        string baseKey = t.Name + "_" + keyName;
        Type[] types = t.GetGenericArguments();
        object obj = null;

        if (types.Length == 0) // 非泛型类
        {
            obj = LoadNonGenericType(t, baseKey);
        }
        else if (types.Length == 1 && typeof(IList).IsAssignableFrom(t))
        {
            obj = LoadGenericList(t, baseKey, types[0]);
        }
        else if (types.Length == 2 && typeof(IDictionary).IsAssignableFrom(t))
        {
            obj = LoadGenericDictionary(t, baseKey, types[0], types[1]);
        }

        return obj;
    }

    private object LoadNonGenericType(Type t, string baseKey)
    {
        object obj = Activator.CreateInstance(t);
        FieldInfo[] fieldInfos = t.GetFields();

        // 修正：移除索引，与存储时的键名保持一致
        foreach (FieldInfo field in fieldInfos)
        {
            string fieldKey = baseKey + "_" + field.FieldType.Name + "_" + field.Name;
            object value = LoadFieldValue(field.FieldType, fieldKey);
            if (value != null)
            {
                field.SetValue(obj, value);
            }
        }

        return obj;
    }

    private object LoadGenericList(Type t, string baseKey, Type elementType)
    {
        object obj = Activator.CreateInstance(t);
        IList list = obj as IList;
        if (list == null)
        {
            return null;
        }

        string lengthKey = baseKey + "_List_Length";
        int count = PlayerPrefs.GetInt(lengthKey);
        //使用了索引值
        for (int i = 0; i < count; i++)
        {
            string listKey = baseKey + "_List_Element_" + i;

            object element = LoadFieldValue(elementType, listKey);
            if (element != null)
            {
                list.Add(element);
            }
        }
        return list;
    }

    private object LoadGenericDictionary(Type t, string baseKey, Type typeKey, Type typeValue)
    {
        object obj = Activator.CreateInstance(t);
        IDictionary dict = obj as IDictionary;
        if (dict == null) return null;

        string lengthKey = baseKey + "_Dict_Length";

        if (!PlayerPrefs.HasKey(lengthKey))
        {
            return obj;
        }

        int count = PlayerPrefs.GetInt(lengthKey);
        //使用了索引值
        for (int i = 0; i < count; i++)
        {
            string keyKey = baseKey + "_Key_" + i;
            string valueKey = baseKey + "_Value_" + i;

            object key = LoadFieldValue(typeKey, keyKey);
            object value = LoadFieldValue(typeValue, valueKey);

            if (key != null && value != null)
            {
                dict.Add(key, value);
            }
        }

        return dict;
    }

    private object LoadFieldValue(Type t, string fullKey)
    {
        if (t == null)
        {
            return null;
        }
        if (t == typeof(int))
        {
            int value = PlayerPrefs.GetInt(fullKey, 0);
            return value;
        }
        else if (t == typeof(float))
        {
            float value = PlayerPrefs.GetFloat(fullKey, 0f);
            return value;
        }
        else if (t == typeof(string))
        {
            string value = PlayerPrefs.GetString(fullKey, "");
            return value;
        }
        else if (t == typeof(bool))
        {
            bool value = PlayerPrefs.GetInt(fullKey, 0) == 1;
            return value;
        }
        else if (typeof(IList).IsAssignableFrom(t))
        {
            return LoadDate(t, fullKey);
        }
        else if (typeof(IDictionary).IsAssignableFrom(t))
        {
            return LoadDate(t, fullKey);
        }
        else
        {
            return LoadDate(t, fullKey);
        }
    }
}
//代码反思，这个数据存储的框架很不错，但是一开始广泛使用索引值，导致读取和存储十分困难，所以尽量减少使用索引值
//这个代码最终用到的索引值只有List和Dictionary使用了，并且先存储和读取长度数据之后才使用的索引值，这样索引值出错的概率大大降低，并且List和Dictionary中数据特殊，也需要使用索引值
//并且最开始代码设计的时候使用了静态的索引值，本来是想着索引值一直递增，可以避免索引值的重复，但是没考虑读取的时候没有办法从0开始，所以最后改成变量最后直接废弃
//关于索引值的总结 只有在知道长度的时候使用索引值