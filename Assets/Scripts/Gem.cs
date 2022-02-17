
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] public int coinValue;
    [SerializeField] public ParticleSystem explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoinValue() {

        coinValue += 10;
    }
}
