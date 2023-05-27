using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;

namespace RPG.Terrain
{
    public class TreeManager : MonoBehaviour
    {
        [SerializeField] private string folderName = "TreeMaterial";
        private Material[] materialArr;
        private int count = 0;
        private void OnEnable()
        {
            GlobalEventManager.OnTreeAppeared += GlobalEventManager_OnTreeAppeared;
        }
        private void Start()
        {
            SettingMaterialArr();
        }
        private void OnDisable()
        {
            GlobalEventManager.OnTreeAppeared -= GlobalEventManager_OnTreeAppeared;
        }

        private void GlobalEventManager_OnTreeAppeared(object sender, TreeVisual treeVisual)
        {
            var randomMaterial = GetRandomMaterial();
            treeVisual.SetLeafMaterial(randomMaterial);
            count++;
        }
        private void SettingMaterialArr()
        {
            var allMaterials = Resources.LoadAll<Material>(folderName);
            materialArr = allMaterials;
        }

        private Material GetRandomMaterial()
        {
            int randomIndex = Random.Range(0, materialArr.Length);
            return materialArr[randomIndex];
        }
    }

}
