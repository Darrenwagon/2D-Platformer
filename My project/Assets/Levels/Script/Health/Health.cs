using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float current_Health { get; private set;}
    private Animator anim;
    private bool dead;

   private void Awake() 
   {
     current_Health = startingHealth;
     anim = GetComponent<Animator>();
   }

    public void TakeDamage(float _damage)
    {
        current_Health = Mathf.Clamp(current_Health - _damage, 0, startingHealth);

        if (current_Health > 0)
        {
           anim.SetTrigger("hurt");
        }
        else 
        {
            if(!dead)
            {
            anim.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
            dead =true;
            }
        }
    }

    public void update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }
}
