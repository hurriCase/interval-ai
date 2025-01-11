using System;
using Client.Scripts.Patterns;
using UnityEngine;

namespace Client.Scripts.Core
{
    internal sealed class UserData : ObservableScriptableObject<UserData>
    {
        internal static UserData Instance => _instance ?? (_instance = DataLoader.LoadUserData<UserData>());
        private static UserData _instance;

        [SerializeField, InspectorReadOnly] private string _userId;
        [SerializeField, InspectorReadOnly] private string _temporaryGuestId;
        [SerializeField, InspectorReadOnly] private LogInType _logInType;

        public string UserID
        {
            get => _userId;
            set
            {
                if (_userId == value) return;
                _userId = value;
                NotifyValueChanged(this);
            }
        }

        public string TemporaryGuestId
        {
            get => _temporaryGuestId;
            set
            {
                if (_temporaryGuestId == value) return;
                _temporaryGuestId = value;
                NotifyValueChanged(this);
            }
        }

        public LogInType LogInType
        {
            get => _logInType;
            set
            {
                if (_logInType == value) return;
                _logInType = value;
                NotifyValueChanged(this);
            }
        }

        internal void LoadFromDTO(UserDataDTO dto)
        {
            if (dto == null)
                return;

            UserID = dto.UserID;
            TemporaryGuestId = dto.TemporaryGuestId;
            LogInType = dto.LogInType;
        }

        internal UserDataDTO ToDTO() =>
            new()
            {
                UserID = UserID,
                TemporaryGuestId = TemporaryGuestId,
                LogInType = LogInType
            };
    }

    [Serializable]
    internal class UserDataDTO
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