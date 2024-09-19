using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using WAK.Game; 

namespace WAK.Managers
{

    public class GameManager : MonoSingleton<GameManager>
    {

        [SerializeField] private GamePlaySettings gamePlaySettings;

        public void Set(GamePlaySettings gamePlaySettings)
        {
            this.gamePlaySettings = gamePlaySettings;
        }

        public void Clear()
        {

        }

        public void Play()
        {

        }


        public void SpawnAnimal(Vector2 screenPos)
        {
            var objectParams = new ObjectManager.ObjectParams()
            {
                dataKey = 0
            };


            var wakHeadImpl = ObjectManager.Instance.Spawn<WakHeadImpl>(objectParams);


        }
         
    }
}

