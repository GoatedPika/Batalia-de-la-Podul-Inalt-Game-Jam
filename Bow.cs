using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Animator CharacterAnimator;
    public Animator bowAnim;
    public SwapWeapon sw;

    public float reloadTime;
    public PlayerArrow arrowPrefab;
    public Transform spawnArrow;
    public PlayerArrow currentArrow;
    public bool isReloading;

    public float firePower;
    public float maxFirePower;
    public float firePowerSpeed;
    public float Basedmg;

    public LineRenderer lineRenderer;
    public int numPoints;
    public float timeIntervalInPoints;
    public LayerMask layers;
    public Transform StartLoc;
    public AudioSource audio;
    public AudioClip fireSound;
    public AudioClip pullSound;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (sw.ChangingWeapon)
        {
            bowAnim.SetBool("isPulling", false);
            bowAnim.Play("idle bow");
        }
        else
        {
            //sa traga sfoara
            if (Input.GetKey(KeyCode.Mouse1) && !bowAnim.GetBool("isPulling") && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bow Equip") && !CharacterAnimator.GetBool("shotBow"))
            {
                firePower = 0;
                bowAnim.SetBool("isPulling", true);
                CharacterAnimator.SetBool("isPullingBow", true);
                audio.clip = pullSound;
                audio.Play();

            }


            if (Input.GetKeyUp(KeyCode.Mouse1) && bowAnim.GetBool("isPulling"))
            {

                bowAnim.SetBool("isPulling", false);
                CharacterAnimator.SetBool("isPullingBow", false);

            }

            if (bowAnim.GetBool("isPulling") && firePower < maxFirePower)
            {
                firePower += Time.deltaTime * firePowerSpeed;
            }
            if (firePower > maxFirePower)
                firePower = maxFirePower;


            //cand trage cu sageata
            if (bowAnim.GetBool("isPulling") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
                audio.PlayOneShot(fireSound,1f);

                firePower = 0;


                bowAnim.SetBool("shot", true);
                CharacterAnimator.SetBool("shotBow", true);

                bowAnim.SetBool("isPulling", false);
                CharacterAnimator.SetBool("isPullingBow", false);
            }

            if (CharacterAnimator.GetBool("shotBow") && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("arc pull") && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("maintain aim"))
            {
                bowAnim.SetBool("shot", false);
                CharacterAnimator.SetBool("shotBow", false);
            }

            if (bowAnim.GetBool("isPulling") && currentArrow != null)
            {
                DrawTrajectory();
                lineRenderer.enabled = true;
            }
            else
                lineRenderer.enabled = false;




        }




    }

    public void Reload()
    {

        //sa reincarce arcul
        if (isReloading || currentArrow != null)
            return;

        isReloading = true;

        StartCoroutine(ReloadAfterTime());
    }

    public IEnumerator ReloadAfterTime()
    {
        yield return new WaitForSeconds(reloadTime);
        currentArrow = Instantiate(arrowPrefab, spawnArrow);
        currentArrow.transform.localPosition = Vector3.zero;

        isReloading = false;
    }

    public void Fire()
    {
        if (isReloading || currentArrow == null)
            return;

        var force = spawnArrow.TransformDirection(Vector3.left * firePower);
        currentArrow.Fly(force);
        currentArrow.dmg = Basedmg * firePower / maxFirePower;

        if (currentArrow.dmg < 0.5f * Basedmg)
            currentArrow.dmg = 0.5f * Basedmg;

        currentArrow = null;
        Reload();

    }

    public bool IsReady()
    {
        return (!isReloading && currentArrow != null);
    }

    void DrawTrajectory()
    {

        //sa vezi traiectoria sagetii
        lineRenderer.positionCount = numPoints;
        List<Vector3> points = new List<Vector3>();

        Vector3 startiPosition = StartLoc.position;

        var force = spawnArrow.TransformDirection(Vector3.left * firePower);
        Vector3 startingVelocity = force / currentArrow.rb.mass;

        for(float t = 0;t<numPoints;t += timeIntervalInPoints)
        {
            Vector3 newPoint = startiPosition + t * startingVelocity;
            newPoint.y = startiPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);

            if(Physics.OverlapSphere(newPoint,0.5f,layers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
        }

        lineRenderer.SetPositions(points.ToArray());
    }

   




}
