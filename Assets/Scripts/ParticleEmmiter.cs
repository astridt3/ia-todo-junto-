using UnityEngine;

public class ParticleEmmiter : MonoBehaviour
{
    private ParticleSystem system;

    [SerializeField] Material material;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        gameObject.transform.Rotate(-90, 0, 0);
        gameObject.GetComponent<ParticleSystemRenderer>().material = material;
        var mainModule = system.main;
        mainModule.startColor = Color.lightSkyBlue;
        mainModule.startSize = 0.5f;
    }

    public void DoEmit()
    {
        system.maxParticles = 100;
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = Color.red;
        emitParams.startSize = 0.2f;
        system.Emit(emitParams, 100);
        system.Play();
        system.maxParticles = 0;
    }
}
