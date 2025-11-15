using UnityEngine;

namespace Utils
{
    public static class FPSMonitor
    {
        private static float _fps;
        private static float _timer;
        private static int _frameCount;

        public static float CurrentFPS => _fps;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            GameObject go = new GameObject("FPSMonitor");
            go.AddComponent<FPSCounterUpdater>();
            Object.DontDestroyOnLoad(go);
        }

        private class FPSCounterUpdater : MonoBehaviour
        {
            private void Update()
            {
                _frameCount++;
                _timer += Time.unscaledDeltaTime;

                if (_timer >= 0.5f)
                {
                    _fps = _frameCount / _timer;
                    _frameCount = 0;
                    _timer = 0f;
                }
            }
        }
    }

}