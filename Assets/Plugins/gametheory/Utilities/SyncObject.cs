//using Parse;

using System.Collections.Generic;

public interface SyncObject 
{
    bool Update(object obj);
    bool IsInvalid();

    Dictionary<string,object> ToDictionary();
    void FromDictionary(Dictionary<string,object> json);
}
