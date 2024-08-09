namespace QuantumUser
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class LocalData
    {
        private static string _nickname;
        public static string Nickname 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(_nickname)) return "Unknown"; return _nickname; 
            }  
            set 
            { 
                _nickname = value; 
            } 
        }
    }

}