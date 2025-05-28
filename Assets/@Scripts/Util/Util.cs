using UnityEngine;

public class Util
{
    static public bool NullCheck<T>(T t) where T : Object
    {
        if(t == null)
        {
            Debug.Log(t.ToString() + " : Null Reference Exception");
            return true;
        }
        return false;
    }
}
