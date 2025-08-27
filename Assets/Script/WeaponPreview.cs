using UnityEngine;
using UnityEngine.UI;

public class WeaponPreview : MonoBehaviour
{
    [HideInInspector] public int currentIndex;
    [HideInInspector] public PlayerShootingController playerShootingController;
    public Image imagePreview;

    public void SelectWeapon()
    {
        playerShootingController.SwitchToWeapon(currentIndex);
    }
}
