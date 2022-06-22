using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.current_Health / 10;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.current_Health / 10;
    }
}