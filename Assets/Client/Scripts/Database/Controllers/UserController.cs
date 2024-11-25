namespace Client.Scripts.Database.User
{
    internal sealed class UserController
    {
        private string _userId;
        
        internal void Init(string userId)
        {
            _userId = userId;    
        }
    }
}