using System.Collections;
using System.Linq;
using System.Reflection;
using Godot.Collections;
using System.Collections.Generic;
using System;
using Godot;
public interface ConvertGodoData{}

public static class ModelExt{

    public static List<object> GetGodotData(this ConvertGodoData model ){
        List<object> data = new List<object>();
        PropertyInfo[] myPropertyInfo = model.GetType().GetProperties();

        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            GD.Print(myPropertyInfo[i].PropertyType);
            if( myPropertyInfo[i].GetValue(model) is ConvertGodoData cgd ){
                GD.Print("type of ConvertGodoData");
                data.AddRange( cgd.GetGodotData() );
            }else if( typeof(IEnumerable<ConvertGodoData>).IsAssignableFrom(myPropertyInfo[i].PropertyType) ){
                GD.Print("type list of ConvertGodoData");
                IEnumerable<ConvertGodoData> cgdL = (IEnumerable<ConvertGodoData>)myPropertyInfo[i].GetValue(model);
                if(cgdL != null){
                    int cpt = 0;
                    List<object> dataTP = new List<object>();
                    
                        foreach(var c in cgdL){
                            cpt++;
                            dataTP.AddRange(c.GetGodotData());
                        }
                    data.Add(cpt);
                    data.AddRange(dataTP);
                }else{
                    data.Add(0);
                }
                

            }else{ 
                GD.Print("type primitif");
                data.Add( myPropertyInfo[i].GetValue(model) ) ;
            }
        }
        foreach(object o in data)
            GD.Print(o);
        
        return data;
    }

    public static T GetModelData<T>(this object[] data ) where T : new() {

        foreach(object o in data)
            GD.Print(o);

        T ret = new T();

        PropertyInfo[] myPropertyInfo = ret.GetType().GetProperties();

        int di = 0;
        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            GD.Print(myPropertyInfo[i].PropertyType);
            
            if( typeof(ConvertGodoData).IsAssignableFrom(myPropertyInfo[i].PropertyType) ){
                var cst = myPropertyInfo[i].PropertyType.GetConstructor(Type.EmptyTypes);
                
                ConvertGodoData tt = (ConvertGodoData)cst.Invoke(Type.EmptyTypes);
                PropertyInfo[] myPropertyInfo2 = tt.GetType().GetProperties();
                foreach(var prop in myPropertyInfo2){
                    prop.SetValue(tt,data[di]);
                    di++;
                }
                myPropertyInfo[i].SetValue(ret , tt);

            }if( typeof(IEnumerable<ConvertGodoData>).IsAssignableFrom(myPropertyInfo[i].PropertyType ) ){
                GD.Print("is List");
                
                
                var genericType = myPropertyInfo[i].PropertyType.GetGenericArguments().Single();
                GD.Print(genericType);
                var creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                GD.Print(creatingCollectionType);
                var ttL2 = Activator.CreateInstance(creatingCollectionType);
                GD.Print(ttL2);
                
                //List<ConvertGodoData> ttL = new List<ConvertGodoData>();
                
                var cst = myPropertyInfo[i].PropertyType.GetConstructor(Type.EmptyTypes);
                GD.Print(cst);
                var ttL = (IList)Activator.CreateInstance(myPropertyInfo[i].PropertyType);
                GD.Print(myPropertyInfo[i].PropertyType.GenericTypeArguments);
                


                Type t = myPropertyInfo[i].PropertyType.GenericTypeArguments.Single();
                var cst22 = t.GetConstructor(Type.EmptyTypes);


                
                int length = (int) data[di];
                di++;
                for(int j = 0; j<length ;j++){
                    GD.Print("in loop j = "+j);
                    //IEnumerable<ConvertGodoData> cst2 = ( IEnumerable<ConvertGodoData> ) myPropertyInfo[i].PropertyType.GetConstructor(Type.EmptyTypes);
                    
                    ConvertGodoData tt = (ConvertGodoData)cst22.Invoke(Type.EmptyTypes);
                    PropertyInfo[] myPropertyInfo2 = tt.GetType().GetProperties();
                    foreach(var prop in myPropertyInfo2){
                        GD.Print("in loop prop = "+prop.Name);
                        prop.SetValue(tt, data[di]);
                        di++;
                    }
                    ttL.Add(tt);
                } 
                myPropertyInfo[i].SetValue(ret , ttL);
                
            }else{
                myPropertyInfo[i].SetValue(ret , data[di]);
                di++;
            }
            
        }
        

        return ret;
    }

}