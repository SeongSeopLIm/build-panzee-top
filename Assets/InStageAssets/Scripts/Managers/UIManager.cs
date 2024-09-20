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
    { // ��ȭ ���� �����ϰ� ������������ �ۼ�

        private Dictionary<StageManager.StageType, View> views;




    } 
}

