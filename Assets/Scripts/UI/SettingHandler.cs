using System;
using UnityEngine;

namespace UI
{
    public class SettingHandler : MonoBehaviour
    {
        public string PlayerName;
        public int AvatarId;
        
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void StorePlayerName(string name) => PlayerName = name;
        public void StoreAvatarId(int id) => AvatarId = id;
    }
}