using UnityEngine;

namespace Lobby
{
    public class CanvasOn : MonoBehaviour
    {
        public GameObject Canvas;

        private void Start()
        {
            Canvas.SetActive(true);
        }
    }
}