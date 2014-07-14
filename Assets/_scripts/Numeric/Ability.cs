using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public enum AbilityType  {MWeapon,RWeapon, Magic}
    public class Ability
    {
        #region Definition
        public string Name { get; set; }
        public float Value { get; set; }

        public float MaxDistance { get; set; }

        public AbilityType AType { get; set; }

        public float Delay { get; set; }

        public bool AvtoDelay { get; set; }

        public float Cooldown { get; set; }

        private float currCooldown=0;
        #endregion
        #region Constructor
        public Ability(UnitBody carrier)
        {
            carrier.Tick += OnTick;
        }
        #endregion
        #region Methods

        public bool Active { get { return currCooldown == 0; } }

        private void OnTick(float dt)
        {
            if (currCooldown > 0)
                currCooldown -= dt;
            else
                currCooldown = 0;
        }
        public float Use()
        {
            currCooldown = Cooldown;
            return Value;
        }
        #endregion

    }
