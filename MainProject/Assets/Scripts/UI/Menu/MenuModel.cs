using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace UI.Menu
{
    public class MenuModel : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<int> MenuWindowId => _menuWindowId;
        private readonly IntReactiveProperty _menuWindowId = new IntReactiveProperty(0);

        //画面遷移
        public void ChangeMenuId(int id)
        {
            _menuWindowId.Value = id;
        }
    }
}