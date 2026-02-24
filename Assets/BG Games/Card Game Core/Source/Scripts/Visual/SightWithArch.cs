using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BG_Games.Card_Game_Core.Visual
{
    class SightWithArch:AimVisual
    {
        [SerializeField] private Transform _sight;
        [SerializeField] private SpriteRenderer _archElementPrefab;
        [Header("Arch Params")]
        [SerializeField] private float _calculationPointsDensity = 10; 
        [SerializeField] private float _minGap = 0.1f;
        [SerializeField] private int _targetOffset = 2;
        [SerializeField] private float _curveness = 3f;
        [SerializeField][Range(0,1)] private float _curveControl = 0.5f;
        [Space]
        [SerializeField] private bool _dynamicCurveness = false;
        [SerializeField] private float _curvenessIncrease = 1;

        private ObjectPool<Transform> _elementsPool;
        private List<Transform> _archElements = new List<Transform>();

        private Vector3 startPos;

        private void Awake()
        {
            _elementsPool = new ObjectPool<Transform>(() =>
            {
                return Instantiate(_archElementPrefab, transform).transform;
            }, element =>
            {
                element.gameObject.SetActive(true);
            }, element =>
            {
                element.gameObject.SetActive(false);
            });
        }

        public override void ShowAim()
        {
            _sight.gameObject.SetActive(true);
        }

        public override void HideAim()
        {
            ReleaseLeftover(0);
            _sight.gameObject.SetActive(false);
        }

        public override void SetStartPosition(Vector3 position)
        {
            startPos = position;
            startPos.z = 0;
        }

        public override void AnimateAim(Vector3 pointerScreenPosition)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(pointerScreenPosition);

            target.z = _sight.position.z;
            _sight.transform.position = target;

            target.z = 0;
            
            Vector3 toTarget = target - startPos;
            float distance = toTarget.magnitude;

            int elementsCount = (int)(distance * _calculationPointsDensity);
            float percentGap = 1f / elementsCount;

            float curveness = !_dynamicCurveness ? _curveness : distance * _curvenessIncrease;

            Vector3 up = Vector3.Cross(Vector3.up, toTarget);
            Vector3 normal = Vector3.Cross(toTarget, up);
            Vector3 bezierControl = Vector3.Lerp(startPos,target,_curveControl) + (normal.normalized * curveness);


            Vector3 prevArchPoint = startPos;
            Vector3 prevCurvePoint = startPos;

            int archIndex = 0;
            for (int i = 1; i < elementsCount; i++)
            {
                Vector3 pos = GetBezierPoint(startPos, target, percentGap * i, bezierControl);

                if ((pos - prevArchPoint).magnitude > _minGap)
                {
                    SetUpElement(archIndex, pos, pos - prevCurvePoint);

                    archIndex++;
                    prevArchPoint = pos;
                }

                prevCurvePoint = pos;
            }
            
            ReleaseLeftover((archIndex+1) - _targetOffset);
        }

        private void SetUpElement(int index, Vector3 pos, Vector3 direction)
        {
            if (index < _archElements.Count)
            {
                _archElements[index].position = pos;
                _archElements[index].rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
            else
            {
                Transform archElement = _elementsPool.Get();
                _archElements.Add(archElement);

                archElement.position = pos;
                archElement.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }

        }

        private void ReleaseLeftover(int max)
        {
            if (max > _archElements.Count || max < 0)
            {
                return;
            }

            for (int i = max; i < _archElements.Count; i++)
            {
                _elementsPool.Release(_archElements[i]);
            }
            _archElements.RemoveRange(max, _archElements.Count - max);
        }

        public Vector3 GetBezierPoint(Vector3 startPoint, Vector3 endPoint, float t, Vector3 control)
        {
            return Mathf.Pow(1 - t, 2) * startPoint +
                   2 * (1 - t) * t * control +
                   Mathf.Pow(t, 2) * endPoint;
        }
    }
}
