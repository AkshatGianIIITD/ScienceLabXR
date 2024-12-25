using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;
    public GameObject chargeParticle;
    private ChargedParticle chargedParticleComponent; 

    void Start()
    {
        chargedParticleComponent = chargeParticle.GetComponent<ChargedParticle>();
    }

    void Update()
    {
        chargedParticleComponent.charge = slider.value;
    }
}
