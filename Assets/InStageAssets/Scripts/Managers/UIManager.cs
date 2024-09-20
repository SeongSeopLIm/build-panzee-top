using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;
using WAK.UI;

namespace WAK.Managers
{  
    public enum ViewState
    {
        Hidden,
        Show,
        StartTranslationToShow,
        StartTranslationToHide,
    }

    public class UIManager : PersistentMonoSingleton<UIManager>
    { // 고도화 없이 간단하게 페이지단위로 작성

        private Dictionary<StageManager.StageType, View> views;




    } 
}

