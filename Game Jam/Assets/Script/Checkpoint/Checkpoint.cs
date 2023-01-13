using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public TowerContext TowerSide = TowerContext.South;
    public float CamHeight;
    public Animator animator;
    public AudioClip _ac;

    private AudioSource _as;

    private void Start()
    {
        _as = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isTriggered");
            Debug.Log(gameObject.name);
            GameManager.Inst.Player.CurrentCheckpoint = this;
            
            _as.PlayOneShot(_ac);
        }
        
        
    }
}
