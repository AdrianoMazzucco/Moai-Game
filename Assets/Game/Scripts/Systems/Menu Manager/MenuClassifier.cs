using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu Classifier", menuName = "UI/Menu Classifier")]
public class MenuClassifier : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnly] public string menuName;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        menuName = name;
    }
}
