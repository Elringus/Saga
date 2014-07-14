using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

    public delegate void TickHandler(float dt);
    public class UnitBody : MonoBehaviour
    {
        #region Character

        public float MaxXP;
        public float Armor;
        public float MaxMoveSpeed;
        public int UseAbilityIndex;

        public float HPRegen;

        private float currHP;
        public float CurrHP
        {
            get
            {
                return currHP;
            }
        }

        private void ChangeHP(float value)
        {
            if (value > 0 && IsAlive && currHP + value < MaxXP)
                currHP += value;

            if (currHP + value <= 0)
            {
                currHP = 0;
                ToDead();
            }
        }

        private bool isalive = true;
        public bool IsAlive { get { return IsAlive; } }

        #region Abilitys
        private List<Ability> abilitys = new List<Ability>();
        public List<Ability> Abilitys { get { return abilitys; } }

        private Ability currAbility
        {
            get
            {
                if (UseAbilityIndex < abilitys.Count)
                    return abilitys[UseAbilityIndex];
                else
                    return abilitys[0];
            }
        }

        #endregion
        #endregion
        #region Events

        public event TickHandler Tick;

        #endregion
        #region Methods
        #region Attack

        private List<Damager> dmgs = new List<Damager>();
        public void Attack(UnitBody target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= currAbility.MaxDistance && currAbility.Active)
                dmgs.Add(new Damager(this, target, currAbility));
        }

        //private void MakeAttack(UnitBody target, Damager dmg)
        //{
        //   target.TakeAttack(dmg.Value);
        //}
        public void TakeAttack(float dmgvalue)
        {
            ChangeHP(dmgvalue);
            dmgs.RemoveAll(delegate(Damager d) { return d.Used; });
        }

        #endregion
        #region Life

        private void ToDead()
        {
            isalive = false;
        }

        #endregion
        #endregion
        #region Unity

        protected virtual void Update()
        {
            float dt = Time.deltaTime;
            ChangeHP(HPRegen * dt);

            if (Tick != null)
                Tick(dt);
        }

        protected virtual void Awake()
        {
            AbilityIntial();
        }

        private void AbilityIntial()
        {
            abilitys.Add(new Ability(this) { AType = AbilityType.MWeapon, MaxDistance = 3, Name = "Melee Atack", Value = 10, Delay = 0.3f, Cooldown = 2 });
            abilitys.Add(new Ability(this) { AType = AbilityType.Magic, MaxDistance = 8, Name = "Kaka Ball", Value = 5, Delay = 0.1f, Cooldown = 1 });
            abilitys.Add(new Ability(this) { AType = AbilityType.MWeapon, MaxDistance = 3, Name = "Killer Attack", Value = 25, Delay = 3f, Cooldown = 8 });
            abilitys.Add(new Ability(this) { AType = AbilityType.Magic, MaxDistance = 20, Name = "Star", Value = 20, Delay = 0.2f, Cooldown = 5 });
        }

        #endregion
    }
