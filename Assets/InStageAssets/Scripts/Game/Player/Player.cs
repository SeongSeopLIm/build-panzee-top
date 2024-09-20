using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WAK
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        public Camera PlayerCamera => playerCamera;


    }
}
