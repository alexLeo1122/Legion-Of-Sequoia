using RPG.Ultilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Allies
{
    public class SlimeStateAttribute : Attribute
    {
        public SlimeStateEnum stateEnum;

        public SlimeStateAttribute(SlimeStateEnum stateEnum)
        {
            this.stateEnum = stateEnum;
        }
    }
}

