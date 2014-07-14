using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public delegate void DamageHandler(UnitBody target, Damager dmg);

    public class Damager
    {
        #region Definition
        public UnitBody Attacker { get; set; }
        public UnitBody Target { get; set; }

        private float currdelay;
        public float CurrDelay { get; set; }

        public float Value { get; set; }
        #endregion
        #region Constructor
        public Damager(UnitBody attacker, UnitBody target, Ability ability)
        {
            Attacker = attacker;
            Attacker.Tick += OnTick;
            Target = target;


            currdelay = ability.Delay;
            Value = ability.Use();
        }
        #endregion
        #region Methods
        private void OnTick(float dt)
        {
            currdelay -= dt;
            if (currdelay <= 0)
                Use();
        }

        private bool used = false;
        public bool Used { get { return used; } }

        //public event DamageHandler Use;

        private void Use()
        {
            used = true;
            Target.TakeAttack(Value);
            SoaringText.Create(Target.transform.position, Value.ToString(), Color.red, new Vector3(0,5,0));
            Attacker.Tick -= OnTick;
        }
        #endregion
    }
