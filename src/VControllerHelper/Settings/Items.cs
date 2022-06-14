using System.Collections.Generic;
using System.Linq;

namespace VControllerHelper.Settings
{
    internal class Items
    {
        #region Private Fields

        private static List<CSettingsFlags> _items = new List<CSettingsFlags>();

        #endregion Private Fields

        #region Public Methods

        public static T GetItem<T>() where T : CSettingsFlags, new()
        {
            var o = _items.FirstOrDefault(i => i.GetType() == typeof(T));
            if (o == null)
            {
                o = new T();
                _items.Add(o);
            }
            return (T)o;
        }

        #endregion Public Methods
    }
}