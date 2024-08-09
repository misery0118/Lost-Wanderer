using UnityEngine;

public class KuroFollow : MonoBehaviour
{
    public GameObject Raijin; // Reference to the player
    public Transform BackTransform; // Reference to the player's Back transform
    public float TargetDistance;
    public float AllowedDistance = 5;
    public GameObject Kuro; // Reference to the NPC
    public float FollowSpeed;
    private bool isClimbing = false;

    void Update()
    {
        if (isClimbing)
        {
            AttachToBack();
        }
        else
        {
            FollowPlayer();
        }
    }

    public void SetClimbingState(bool climbing)
    {
        isClimbing = climbing;
        Debug.Log("SetClimbingState called with climbing: " + climbing);

        if (climbing)
        {
            AttachToBack();
        }
        else
        {
            DetachFromBack();
        }
    }

    private void AttachToBack()
    {
        Debug.Log("Attaching Kuro to Back");
        
        if (Kuro == null || BackTransform == null)
        {
            Debug.LogError("Kuro or BackTransform is not assigned.");
            return;
        }

        Kuro.transform.position = BackTransform.position;
        Kuro.transform.rotation = BackTransform.rotation;
        Kuro.transform.SetParent(BackTransform);
    }

    private void DetachFromBack()
    {
        Debug.Log("Detaching Kuro from Back");

        if (Kuro == null)
        {
            Debug.LogError("Kuro is not assigned.");
            return;
        }

        Kuro.transform.SetParent(null);
    }

    private void FollowPlayer()
    {
        if (Raijin == null)
        {
            Debug.LogError("Raijin is not assigned.");
            return;
        }

        transform.LookAt(Raijin.transform);
        TargetDistance = Vector3.Distance(Raijin.transform.position, transform.position);

        if (TargetDistance > AllowedDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, Raijin.transform.position, FollowSpeed * Time.deltaTime);
        }
    }
}