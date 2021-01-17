using System;
using System.Reflection;
//using do;

namespace DL
{
    static class Cloning
    {
        //first way - No Bonus
        //do it for each do entity
        //internal static do.Person Clone(this do.Person original)
        //{
        //    do.Person target = new do.Person();
        //    target.ID = original.ID;
        //    target.Name = target.Name;
        //    //...
        //    return target;
        //}

        //second way - With Bonus
        //cretae empty interface named IClonable in the DLAPI project
        //namespace do
        //{
        //        public interface IClonable { }
        //}
        //add this method here in DLObject project
        //internal static IClonable Clone(this IClonable original)
        //{
        //    IClonable target = (IClonable)Activator.CreateInstance(original.GetType());
        //    //...
        //    return target;
        //}

        //third way - With Bonus // generic shallowed copy, properties only
        internal static T Clone<T>(this T original) where T : new()
        {
            T copyToObject = new T();
            //T copyToObject = (T)Activator.CreateInstance(typeof(T));
 
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);

            return copyToObject;
        }


    }
}
