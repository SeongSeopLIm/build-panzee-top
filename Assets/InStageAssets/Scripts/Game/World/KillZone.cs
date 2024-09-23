using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAK.Managers;

namespace WAK.Game
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class KillZone : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"On Enter killzone : {collision.gameObject.name}");
            if(collision.gameObject.TryGetComponent<WakHead>(out var wakHead))
            {
                StageManager.Instance.SwitchStage(StageManager.StageType.Result);
            }
        }
         
    }


}