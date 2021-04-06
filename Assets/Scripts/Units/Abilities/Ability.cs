using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    protected Unit unit;
    [SerializeField] protected float currentStamina;

    [SerializeField] protected int castStaminaThreshold;
    protected int castRange;
    protected int areaOfEffect;
    protected float duration;
    private float basePower;
    protected float currentPower;
    protected bool canGenerateStamina = true;

    public float BasePower { get => basePower; set => basePower = value; }

    public void setUnit(Unit unit)
    {
        this.unit = unit;
        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }

    public void updateAbility(float incrementStamina)
    {
        if (canGenerateStamina)
        {
            currentStamina += incrementStamina;
            if (currentStamina >= castStaminaThreshold && !unit.getIsAbilityActivated())
            {
                currentStamina -= castStaminaThreshold;
                castAbility();
            }

            unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
        }
    }

    virtual public void castAbility()
    {
        updateCurrentPower();
        unit.ApplyAbilityGemsEffects();
    }

    private void updateCurrentPower()
    {
        currentPower = BasePower * unit.Power;
    }
    
    public void mageSynergy(int lvl)
    {
        if (lvl == 1)
        {
            currentStamina = castStaminaThreshold / 4;
        }
        if (lvl == 2)
        {
            currentStamina = castStaminaThreshold / 2;
        }
        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }

    protected IEnumerator ProjectileAnimation(Unit targetUnit, GameObject projectileGameObject)
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
        float baseAngle = projectileGameObject.transform.localRotation.eulerAngles.z;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle + baseAngle, Vector3.forward);

        //while there is still a target, the projectile is not arrived at the target position and the max number of refresh is not reached
        while (targetUnit != null && projectile.transform.position != targetUnit.transform.position && numberOfRefresh <= maxRefresh)
        {
            projectile.transform.position += distance * speed;
            numberOfRefresh++;
            yield return new WaitForSeconds(refreshRate);
        }

        Destroy(projectile);

        if (targetUnit != null)
            targetUnit.takeDamage(currentPower);
    }
}
