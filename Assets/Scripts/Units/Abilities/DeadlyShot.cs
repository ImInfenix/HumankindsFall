using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyShot : Ability
{
    [SerializeField] private GameObject projectileGameObject;

    private void Awake()
    {
        castStaminaThreshold = 400;
        BasePower = 30;

        

        soundEffect = Resources.Load("SoundEffects/bow-released") as AudioClip;

        projectileGameObject = Resources.Load("Ability Prefabs/DeadlyShot") as GameObject;
    }
    override public void castAbility()
    {
        base.castAbility();

        GameObject[] listEnemyUnits = GameObject.FindGameObjectsWithTag(unit.getTargetTag());

        if (listEnemyUnits.Length > 0)
        {
            Unit targetUnit = listEnemyUnits[0].GetComponent<Unit>();
            float minLifeProv = targetUnit.CurrentLife;


            foreach (GameObject unit in listEnemyUnits)
            {
                if(unit.GetComponent<Unit>().CurrentLife < minLifeProv)
                {
                    minLifeProv = unit.GetComponent<Unit>().CurrentLife;
                    targetUnit = unit.GetComponent<Unit>();
                }
            }

            playSound();
            StartCoroutine(ProjectileAnimation(targetUnit));
        }
    }

    IEnumerator ProjectileAnimation(Unit targetUnit)
    {
        Vector3 startPosition = transform.position;
        GameObject projectile = Instantiate(projectileGameObject, startPosition, Quaternion.identity, transform);

        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.06f;

        //set the maximum number of refresh of the projectile animation
        float maxRefresh = 1 / speed;

        //time to wait between every movement
        float refreshRate = 0.01f;

        int numberOfRefresh = 0;

        //get the distance between start position and target position
        Vector3 distance = targetUnit.transform.position - projectile.transform.position;

        //set the correct angle
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //while there is still a target, the projectile is not arrived at the target position and the max number of refresh is not reached
        while (targetUnit != null && projectile.transform.position != targetUnit.transform.position && numberOfRefresh <= maxRefresh)
        {
            projectile.transform.position += distance * speed;
            numberOfRefresh++;
            yield return new WaitForSeconds(refreshRate);
        }

        Destroy(projectile);

        if(targetUnit != null)
            targetUnit.takeDamage(currentPower);
    }
}
