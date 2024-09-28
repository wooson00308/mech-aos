using System.Collections;
using System.Collections.Generic;
using Quantum;
using Quantum.Mech;
using UnityEngine;


namespace Quantum.Mech
{
    public unsafe class RibbonContainer : QuantumSceneViewComponent<CustomViewContext>
    {
        [SerializeField] private RibbonUI ribbonPrefab; // RibbonUI 프리팹
        [SerializeField] private float spacing = 100.0f; // 각 RibbonUI 사이의 간격
        [SerializeField] private float animationDuration = 0.5f; // 애니메이션 지속 시간
        [SerializeField] private AnimationCurve moveCurve; // 이동 애니메이션 커브
        [SerializeField] private List<Sprite> textures;

        private Dictionary<string, RibbonUI> ribbonDictionary = new Dictionary<string, RibbonUI>(); // Ribbon 저장 구조
        private List<RibbonUI> ribbonList = new List<RibbonUI>(); // RibbonUI 리스트 (정렬을 위해)
        private Queue<IEnumerator> animationQueue = new Queue<IEnumerator>(); // 애니메이션 작업 큐
        private bool isAnimating = false; // 애니메이션 진행 여부


        public void Start()
        {
            QuantumEvent.Subscribe<EventOnMechanicTakeDamage>(this, OnMechanicTakeDamage);
            QuantumEvent.Subscribe<EventOnMechanicDeath>(this, OnMechanicDeath);
        }

        private void OnMechanicDeath(EventOnMechanicDeath e)
        {
            if (e.Killer != ViewContext.LocalEntityRef) return;
            ReceiveRibbon("Killer", textures[0]);
        }

        private void OnMechanicTakeDamage(EventOnMechanicTakeDamage e)
        {
            Debug.Log($"OnMechanicTakeDamage -> Mechanic : {e.Source}");
            Debug.Log($"OnMechanicTakeDamage -> EntityRef : {ViewContext.LocalEntityRef}");
            if (e.Source != ViewContext.LocalEntityRef) return;
            ReceiveRibbon("Attacker", textures[1]);
        }

        private void ReceiveRibbon(string ribbonName, Sprite sprite)
        {
            if (ribbonDictionary.TryGetValue(ribbonName, out var value))
            {
                // 이미 있는 약장이면 해당 RibbonUI의 ReceiveServiceRibbon 호출

                value.ReceiveServiceRibbon(ribbonName, sprite);
                if (ribbonList.Contains(value))
                {
                    ribbonList.Remove(value);
                    ribbonList.Add(value);
                }
            }
            else
            {
                // 약장이 없다면 새로운 RibbonUI 생성
                CreateNewRibbon(ribbonName, sprite);
            }

            animationQueue.Enqueue(AnimateRibbons());
            if (!isAnimating)
            {
                StartCoroutine(ProcessAnimationQueue());
            }
        }

        private void CreateNewRibbon(string ribbonName, Sprite sprite)
        {
            RibbonUI newRibbon = Instantiate(ribbonPrefab, transform);
            newRibbon.ReceiveServiceRibbon(ribbonName, sprite);
            newRibbon.OnClose += DestroyRibbonUI;
            ribbonDictionary[ribbonName] = newRibbon;
            ribbonList.Add(newRibbon);
        }

        private IEnumerator ProcessAnimationQueue()
        {
            while (animationQueue.Count > 0)
            {
                isAnimating = true;
                yield return StartCoroutine(animationQueue.Dequeue()); // 큐에서 작업을 꺼내서 실행
            }

            isAnimating = false;
        }

        private IEnumerator AnimateRibbons()
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / animationDuration);
                float curveValue = moveCurve.Evaluate(t);

                for (int i = 0; i < ribbonList.Count; i++)
                {
                    var rectTransform = ribbonList[i].GetComponent<RectTransform>();
                    Vector2 targetPosition =
                        new Vector2(0, +(ribbonList.Count - 1 - i) * (ribbonList[i].Size.y + spacing));
                    var currentPosition = rectTransform.anchoredPosition;
                    var newPosition = Vector2.Lerp(currentPosition, targetPosition, curveValue);
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosition.y);
                }

                yield return null; // 프레임마다 대기
            }

            for (int i = 0; i < ribbonList.Count; i++)
            {
                var rectTransform = ribbonList[i].GetComponent<RectTransform>();
                Vector2 targetPosition = new Vector2(0, +(ribbonList.Count - 1 - i) * (ribbonList[i].Size.y + spacing));
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetPosition.y);
            }
        }

        private void DestroyRibbonUI(string key)
        {
            var ribbonUI = ribbonDictionary[key];
            ribbonList.Remove(ribbonUI);
            ribbonDictionary.Remove(key);
            Destroy(ribbonUI.gameObject);
        }
    }
}
