using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;
using RPG.VFX;
using Unity.VisualScripting;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private HealingEffectPool healingEffectPool;
    [SerializeField] private GameObject healingPrefab;
    [SerializeField] private TakeDamageEffectPool takeDamageEffectPool;
    //disappeared vfx
    [SerializeField] private GameObject disappearedVFX;
    [SerializeField] private GameObject summonVFX;
    private float vfxOffset = 0.6f;
    //hadling summon vfx


    private void Awake()
    {
        GlobalEventManager.OnHealingTakeEffect += GlobalEventManager_OnHealingTakeEffect;
        GlobalEventManager.OnPlayTakeDamageEffect += GlobalEventManager_OnPlayTakeDamageEffect;
        GlobalEventManager.OnGameObjectDisappeared += GlobalEventManager_OnGameObjectDisappeared;
        GlobalEventManager.OnBossSummonedEnemies += GlobalEventManager_OnBossSummonedEnemies;
    }

    private void GlobalEventManager_OnBossSummonedEnemies(object sender, Transform e)
    {
        var vfxObj = Instantiate(summonVFX);
        vfxObj.transform.position = e.position;
    }

    private void GlobalEventManager_OnGameObjectDisappeared(object sender, Transform objectTransform)
    {
        var spawnPos = new Vector3(objectTransform.position.x, objectTransform.position.y + vfxOffset, objectTransform.position.z);
        disappearedVFX.transform.position = spawnPos;
        StartCoroutine(HandleSpawnDisappearedVFX());
    }

    private void GlobalEventManager_OnPlayTakeDamageEffect(object sender, GameCharacterController controller)
    {
        Vector3 pos = controller.EffectPoint.position;
        OnSpawningTakingDmgEffect(pos);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnHealingTakeEffect -= GlobalEventManager_OnHealingTakeEffect;
        GlobalEventManager.OnPlayTakeDamageEffect -= GlobalEventManager_OnPlayTakeDamageEffect;
        GlobalEventManager.OnGameObjectDisappeared -= GlobalEventManager_OnGameObjectDisappeared;
        GlobalEventManager.OnBossSummonedEnemies -= GlobalEventManager_OnBossSummonedEnemies;


    }

    private void GlobalEventManager_OnHealingTakeEffect(object sender, System.EventArgs e)
    {
        Vector3 pos = PlayerController.Instance.EffectPoint.position;
        OnSpawningHealingEffect(pos);
    }

    public void OnSpawningHealingEffect(Vector3 position)
    {
        var healingVFX = healingEffectPool.Pool.Get();
        healingVFX.transform.position = position;
    }
    public void OnSpawningTakingDmgEffect(Vector3 position)
    {
        var takeDmgVFX = takeDamageEffectPool.Pool.Get();
        takeDmgVFX.transform.position = position;
    }
    public IEnumerator HandleSpawnDisappearedVFX()
    {
        disappearedVFX.SetActive(true);
        yield return new WaitForSeconds(1f);
        disappearedVFX.SetActive(false);
    }

}
