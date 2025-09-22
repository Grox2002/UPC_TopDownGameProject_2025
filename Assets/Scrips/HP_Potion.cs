using UnityEngine;

public class HP_Potion : MonoBehaviour
{
    public int healAmount = 25; // cantidad de vida que restaura

    public void Use(GameObject target)
    {
        P_Health stats = target.GetComponent<P_Health>();
        if (stats != null)
        {
            stats.Heal(healAmount);
            Debug.Log("Poción de vida usada: +" + healAmount + " HP");
            Destroy(gameObject); // se consume la poción
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<HP_Potion>().Use(other.gameObject);
        }
    }


}
