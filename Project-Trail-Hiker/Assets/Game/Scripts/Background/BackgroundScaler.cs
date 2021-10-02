using UnityEngine;

namespace Game.Scripts.Background
{
    public class BackgroundScaler : MonoBehaviour
    {

        private Vector3 startScale;

        private enum ScaleType
        {
            x, y, all
        }

        [SerializeField] ScaleType scaleType = ScaleType.all;

        // Start is called before the first frame update
        void Start()
        {
            startScale = transform.localScale;

            FitToScreen();
        }

        private void FitToScreen()
        {
            float height = UnityEngine.Camera.main.orthographicSize * 2;
            float width = height * Screen.width / Screen.height;

            float newWidth = (scaleType == ScaleType.all || scaleType == ScaleType.x) ? width : startScale.x;
            float newHeight = (scaleType == ScaleType.all || scaleType == ScaleType.y) ? height : startScale.y;

            transform.localScale = new Vector3(newWidth, newHeight, startScale.z);
        }
    }
}
