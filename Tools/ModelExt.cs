using System.Reflection;
using Godot.Collections;
using System.Collections;
using System;
using Godot;
public interface ConvertGodoData{}

public static class ModelExt{

    public static Godot.Collections.Dictionary GetGodotData(this ConvertGodoData model ){
        Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
        PropertyInfo[] myPropertyInfo = model.GetType().GetProperties();

        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            data[myPropertyInfo[i].Name] =  myPropertyInfo[i].GetValue(model);
            //GD.Print(data[myPropertyInfo[i].Name]);
        }

        return data;
    }

    public static T GetModelData<T>(this Godot.Collections.Dictionary data ) where T : new() {

        T ret = new T();

        PropertyInfo[] myPropertyInfo = ret.GetType().GetProperties();

        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            myPropertyInfo[i].SetValue(ret , data[myPropertyInfo[i].Name]);
        }

        return ret;
    }

}