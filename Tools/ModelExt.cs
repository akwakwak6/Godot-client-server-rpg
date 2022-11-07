using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
using System;
using Godot;

public interface ConvertGodoData{

}

public static class ModelExt{

    public static List<object> GetGodotData(this ConvertGodoData model ){
        List<object> data = new List<object>();
        foreach(PropertyInfo prop in model.GetType().GetProperties()){
            if( prop.GetValue(model) is ConvertGodoData cgd ){//TODO is ? ok ? check ?
                data.AddRange( cgd.GetGodotData() );
            }else if( typeof(IEnumerable<ConvertGodoData>).IsAssignableFrom(prop.PropertyType) ){
                IEnumerable<ConvertGodoData> cgdL = (IEnumerable<ConvertGodoData>)prop.GetValue(model);
                if(cgdL == null){
                    data.Add(0);
                }else{
                    int cpt = 0;
                    List<object> dataTP = new List<object>();
                    foreach(var c in cgdL){
                        cpt++;
                        dataTP.AddRange(c.GetGodotData());
                    }
                    data.Add(cpt);
                    data.AddRange(dataTP);
                }
            }else{
                
                //TODO this is a rustin to fix issue in godot objet ( long become int ) => check issue fix ?
                if( prop.GetValue(model) is ulong lv ){
                    data.Add(Convert.ToString(lv));
                }else
                    data.Add( prop.GetValue(model) ) ;
            }
        }
        
        return data;
    }

    public static T GetModelData<T>(this object[] data ) where T : ConvertGodoData,new() {

        T ret = new T();
        int di = 0;

        foreach(PropertyInfo pi in ret.GetType().GetProperties() ){
            if( typeof(ConvertGodoData).IsAssignableFrom(pi.PropertyType) ){                
                ConvertGodoData tt = (ConvertGodoData)pi.PropertyType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
                foreach(var prop in tt.GetType().GetProperties()){
                    prop.SetValue(tt,data[di]);
                    di++;
                }
                pi.SetValue(ret , tt);

            }if( typeof(IEnumerable<ConvertGodoData>).IsAssignableFrom(pi.PropertyType ) ){
                
                IList cgdL = (IList)Activator.CreateInstance(pi.PropertyType);
                ConstructorInfo cnstrct = pi.PropertyType.GenericTypeArguments.Single().GetConstructor(Type.EmptyTypes);
                int length = (int) data[di];
                di++;
                for(int j=0;j<length;j++){
                    ConvertGodoData tt = (ConvertGodoData)cnstrct.Invoke(Type.EmptyTypes);
                    foreach(var prop in tt.GetType().GetProperties()){
                        prop.SetValue(tt, data[di]);
                        di++;
                    }
                    cgdL.Add(tt);
                } 
                pi.SetValue(ret , cgdL);
                
            }else{
                
                //TODO this is a rustin to fix issue in godot objet ( long become int ) => check issue fix ?
                if( TypeCode.UInt64 == Type.GetTypeCode(pi.PropertyType)){
                    data[di] = Convert.ToUInt64(data[di]);
                }
                pi.SetValue(ret , data[di]);
                di++;                
            }
        }
        

        return ret;
    }

}