using UnityEngine;

namespace Game.Scripts.Background
{
    public class TextureAnimator : MonoBehaviour
    {
        private Material mat;
        private Vector2 offset;

        [Range(0.1f, 100.0f)]
        public float speed = 5f;

        [Range(0.1f, 100.0f)]
        [SerializeField]
        float speedController = 10f;

        // Start is called before the first frame update
        void Start()
        {
            mat = gameObject.GetComponent<Renderer>().material;
            offset = mat.GetTextureOffset("_MainTex");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            offset.x = offset.x + (speed * Time.fixedDeltaTime / speedController);
            mat.SetTextureOffset("_MainTex", offset);
        }
    }
}