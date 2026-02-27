using System.Collections;
using UnityEngine;

public class AutoDrawSystem : MonoBehaviour
{
    [SerializeField] private float autoDrawInterval = 5f;

    private void Start()
    {
        StartCoroutine(AutoDrawCards());
    }

    private IEnumerator AutoDrawCards()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoDrawInterval);
            
            DrawCardsGA drawCardsGA = new(1);
            ActionSystem.Instance.Perform(drawCardsGA);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
