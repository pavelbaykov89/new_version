namespace Slk.Domain.Core
{
    public class Role
    {
        #region Constructors

        protected Role() { }

        public Role(string title, bool isSystem, string menuPermissions = "")
        {
            Title = title;

            IsSystem = isSystem;

            MenuPermisions = menuPermissions;
        }

        #endregion
        
        #region Properties

        public long ID { get; protected set; }

        public string Title { get; protected set; }

        public bool IsSystem { get; protected set; }

        public string MenuPermisions { get; protected set; }

        #endregion

        #region Member functions

        public void AddPermission(string permission)
        {
            if (!MenuPermisions.Contains(permission))
            {
                if (MenuPermisions.Length == 0)
                {
                    MenuPermisions += @"{permission}";
                }
                else
                {
                    MenuPermisions += @"|{permission}";
                }
            }
        }

        public void RemovePermission(string permission)
        {
            if (MenuPermisions.Contains(permission))
            {
                MenuPermisions.Replace(permission, "");
                MenuPermisions.Replace("||", "|");
            }
        }

        public void RemoveAllPermissions()
        {            
            MenuPermisions = "";            
        }

        #endregion
    }
}
