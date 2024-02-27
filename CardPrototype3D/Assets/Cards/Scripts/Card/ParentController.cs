using Cards;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    [SerializeField] private Transform _parentOne;
    [SerializeField] private Transform _parentTwo;

    public void SetNewParent(Card card)
    {
        if (card.Payment != CardPaymentType.Cheaply) return;
        
        switch (card.gameObject.transform.parent.gameObject.layer)
        {
            case 6:
                card.gameObject.transform.SetParent(_parentOne);
                break;
            case 7:
                card.gameObject.transform.SetParent(_parentTwo);
                break;
        }
    }
}