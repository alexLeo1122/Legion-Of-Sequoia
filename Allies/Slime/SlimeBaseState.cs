using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Allies 
{
    public abstract class SlimeBaseState 
    {
        protected SlimeController controller;
        public SlimeBaseState(SlimeController controller)
        {
            this.controller = controller;
        }
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

    }

}


