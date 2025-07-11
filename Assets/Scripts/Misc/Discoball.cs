
using UnityEngine;

public class Discoball : MonoBehaviour, IHitable
{

    private Flash _flash;
    private DiscoBallManager _discoBallManager;


    void Awake()
    {
        _flash = GetComponent<Flash>();
        _discoBallManager = FindFirstObjectByType<DiscoBallManager>();
    }



   

    public void TakeHit()
    {
        Debug.Log("Inicio la hora loca");
        _discoBallManager.DiscoBallParty();
        _flash.StartFlash();
    }
}


//  Health health = other.gameObject.GetComponent<Health>();
//         health?.TakeDamage(_damageAmount);

//         KnockBack knockBack = other.gameObject.GetComponent<KnockBack>();
//         knockBack?.GetKnockedBack(PlayerController.Instance.transform.position, _knockbackThrust);
//         Flash flash = other.gameObject.GetComponent<Flash>();
//         flash?.StartFlash();