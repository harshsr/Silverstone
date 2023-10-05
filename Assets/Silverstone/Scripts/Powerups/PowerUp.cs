using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PowerUpType PowerUpType;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<IPowerUp>() != null)
        {
            other.gameObject.GetComponentInParent<IPowerUp>().UpdatePowerUp(PowerUpType);
            Destroy(gameObject);
        }
    }
}
