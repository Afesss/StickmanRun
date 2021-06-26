using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject confetti;
    [SerializeField] private Collider _collider;
    public event System.Action OnFinished;
    [Inject] private PlayerSettings playerSettings;
    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;
        confetti.SetActive(true);

        
        StartCoroutine(WaitToFinish());
    }
    private IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(playerSettings.finishDelay);
        OnFinished?.Invoke();
    }
    public void ResetFinish()
    {
        confetti.SetActive(false);
        _collider.enabled = true;
    }
}
