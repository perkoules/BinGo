namespace Mapbox.Unity.MeshGeneration.Modifiers
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using UnityEditor;

#endif

    [Serializable]
    public class AddMonoBehavioursModifierType
    {
        [SerializeField]
        private string _typeString;

        private Type _type;

#if UNITY_EDITOR

        [SerializeField]
        private MonoScript _script;

#endif

        public Type Type
        {
            get
            {
                if (_type == null)
                {
                    _type = Type.GetType(_typeString);
                }
                return _type;
            }
        }
    }
}