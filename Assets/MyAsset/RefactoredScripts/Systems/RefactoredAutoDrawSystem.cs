using System.Collections;
using UnityEngine;
using CardDefense.Core.Events;

namespace CardDefense.Systems
{
    public class RefactoredAutoDrawSystem : MonoBehaviour
    {
        [Header("Auto Draw Settings")]
        [SerializeField] private float autoDrawInterval = 5f;
        [SerializeField] private int drawAmount = 1;

        private void Start()
        {
            StartCoroutine(AutoDrawCards());
        }

        private IEnumerator AutoDrawCards()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoDrawInterval);
                EventBus.Publish(new TriggerDrawCardEvent { Amount = drawAmount });
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
