using UnityEngine;


public class DamageSplash : MonoBehaviour
{
    public GameObject damageSplashPanel;


    public void Start()
    {
        damageSplashPanel.SetActive(false);
    }
    public void ShowDamage(int damage)
    {
        // Show the damage splash screen
        damageSplashPanel.SetActive(true);

        // Disable the splash screen after 1 second
        Invoke("HideDamage", 1f); // Adjust the delay as needed
    }

    private void HideDamage()
    {
        // Hide the damage splash screen
        damageSplashPanel.SetActive(false);
    }
}
