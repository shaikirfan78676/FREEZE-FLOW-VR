using UnityEngine;
using UnityEngine.XR;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 3f;
    public float damage = 40f;
    public Camera playerCamera;

    private InputDevice rightHand;

    void Start()
    {
        // Get right hand controller
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!rightHand.isValid)
            rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool triggerPressed;

        // Detect trigger button press
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
        {
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            attackRange))
        {
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                //Debug.Log("Enemy Hit!");
                enemy.TakeDamage(damage);
                AudioManager.Instance?.PlaySFX(AudioManager.Instance.punchSound);
            }
        }
    }
}