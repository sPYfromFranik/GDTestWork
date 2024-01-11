using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image fillImage;
    [SerializeField] Player player;
    [SerializeField] Sprite inRangeSprite;
    [SerializeField] Sprite outRangeSprite;

    private void Update()
    {
        if (player.superAttackTimer > 0)
            timerText.text = player.superAttackTimer.ToString("F1");
        else
            timerText.text = "";
        slider.value = player.superAttackTimer;
        if (player.EnemyInRange())
            fillImage.sprite = inRangeSprite;
        else
            fillImage.sprite = outRangeSprite;
    }
}
