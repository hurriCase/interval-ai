using System;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.CustomTypes;
using UnityEngine;

namespace Client.Scripts.DB.Data
{
    [Resource("Assets/Resource/Data", "UserData", "Data")]
    internal sealed class UserData : ObservableScriptableObject<UserData>
    {
        internal static UserData Instance => _instance ? _instance : _instance = ResourceLoader<UserData>.Load();
        private static UserData _instance;

        [SerializeField, InspectorReadOnly] private string _userId;
        [SerializeField, InspectorReadOnly] private string _temporaryGuestId;
        [SerializeField, InspectorReadOnly] private LogInType _logInType;

        public string UserID
        {
            get => _userId;
            set
            {
                if (_userId == value)
                    return;

                _userId = value;
                NotifyValueChanged(this);
            }
        }

        public string TemporaryGuestId
        {
            get => _temporaryGuestId;
            set
            {
                if (_temporaryGuestId == value)
                    return;

                _temporaryGuestId = value;
                NotifyValueChanged(this);
            }
        }

        public LogInType LogInType
        {
            get => _logInType;
            set
            {
                if (_logInType == value)
                    return;

                _logInType = value;
                NotifyValueChanged(this);
            }
        }

        internal void LoadFromDataTransferObject(UserDataTransferObject transferObject)
        {
            if (transferObject == null)
                return;

            UserID = transferObject.UserID;
            TemporaryGuestId = transferObject.TemporaryGuestId;
            LogInType = transferObject.LogInType;
        }

        internal UserDataTransferObject ToDataTransferObject() =>
            new()
            {
                UserID = UserID,
                TemporaryGuestId = TemporaryGuestId,
                LogInType = LogInType
            };
    }

    [Serializable]
    internal class UserDataTransferObject
    {
        public string UserID { get; set; }
        public string TemporaryGuestId { get; set; }
        public LogInType LogInType { get; set; }
    }

    internal enum LogInType
    {
        Guest,
        GoogleSignIn
    }
}