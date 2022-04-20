using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

namespace Building {
    public class Administration : Building {
        protected override void ActionOnUpdate() {
            base.ActionOnUpdate();
            LevelManager.IncrementPersonIncome();
        }
    }
}
