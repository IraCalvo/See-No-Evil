using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPeak : MonoBehaviour
{
    public AudioSource soundEffect;
    public float activationDistance;
    public float coneAngle;
    bool soundFXPlayed = false;
    public Animator anim;
    public GameObject slothHead;

    public void Update()
    {
        //CheckIfPlayerPeaked();
        CastConeRays(transform.position, transform.forward, coneAngle, activationDistance);
    }

    public void CheckIfPlayerPeaked()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * activationDistance, Color.red);

        if (Physics.Raycast(ray, out hit, activationDistance))
        {
            if (hit.collider.CompareTag("Player") && PlayerController.instance.eyesOpen)
            {
                Debug.Log("This is being called");
                if (!soundFXPlayed && soundEffect.isPlaying == false)
                {
                    soundEffect.Play();
                    soundFXPlayed = true;
                    anim.SetTrigger("peakedAt");
                }
            }
        }
    }

    void CastConeRays(Vector3 origin, Vector3 direction, float angle, float length)
    {
        float halfAngledRad = angle * Mathf.Rad2Deg * 0.5f;

        Vector3 forwardDir = direction.normalized;
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * forwardDir;
        Vector3 leftDir = Quaternion.Euler(0, -angle * 0.5f, 0) * forwardDir;

        RaycastHit coneHit;

        for(int i = 0; i < angle; i++)
        {
            Vector3 dir = Quaternion.Euler(0, angle + i, 0) * direction.normalized;
            if (Physics.Raycast(origin, dir, out coneHit, length)) 
            {
                Debug.DrawRay(origin, dir * length, Color.green);
                if (coneHit.collider.CompareTag("Player") && PlayerController.instance.eyesOpen)
                {
                    Debug.DrawRay(origin, dir * length, Color.blue);
                    if (!soundFXPlayed && soundEffect.isPlaying == false)
                    {
                        soundEffect.Play();
                        soundFXPlayed = true;
                        anim.SetTrigger("peakedAt");
                    }
                }
            }

        }

        //if (Physics.Raycast(origin, forwardDir, out coneHit, length))
        //{
        //    Debug.DrawLine(origin, forwardDir * length, Color.green);
        //    if(coneHit.collider.CompareTag("Player") && PlayerController.instance.eyesOpen)
        //    {
        //        Debug.Log("Detecting player middle");
        //        if (!soundFXPlayed && soundEffect.isPlaying == false)
        //        {
        //            soundEffect.Play();
        //            soundFXPlayed = true;
        //            anim.SetTrigger("peakedAt");
        //        }
        //    }
        //}

        //else 
        //{
        //    Debug.DrawLine(origin, forwardDir * length, Color.blue);
        //}

        //if (Physics.Raycast(origin, rightDir, out coneHit, length))
        //{
        //    Debug.DrawLine(origin, rightDir * length, Color.green);
        //    if (coneHit.collider.CompareTag("Player") && PlayerController.instance.eyesOpen)
        //    {
        //        Debug.Log("Detecting player right");
        //        if (!soundFXPlayed && soundEffect.isPlaying == false)
        //        {
        //            soundEffect.Play();
        //            soundFXPlayed = true;
        //            anim.SetTrigger("peakedAt");
        //        }
        //    }
        //}
        //else 
        //{
        //    Debug.DrawLine(origin, rightDir * length, Color.blue);
        //}

        //if (Physics.Raycast(origin, leftDir, out coneHit, activationDistance))
        //{
        //    Debug.DrawLine(origin, leftDir * length, Color.green);
        //    if (coneHit.collider.CompareTag("Player") && PlayerController.instance.eyesOpen)
        //    {
        //        Debug.Log("Detecting player left");
        //        if (!soundFXPlayed && soundEffect.isPlaying == false)
        //        {
        //            soundEffect.Play();
        //            soundFXPlayed = true;
        //            anim.SetTrigger("peakedAt");
        //        }
        //    }
        //}
        //else 
        //{
        //    Debug.DrawLine(origin, leftDir * length, Color.blue);
        //}
    }

    public void animEventAnimationDone()
    {
        slothHead.SetActive(false);
    }
}
