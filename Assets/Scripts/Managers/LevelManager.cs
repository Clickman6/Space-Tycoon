using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Managers {
    public class LevelManager : Singleton<LevelManager> {
        public float Prestige = 1;

        [Header("Main Settings")]
        [SerializeField] private BigFloat _money = 0;
        [SerializeField] private CurrencyInfo _currency;
        [SerializeField] private int _startMoney;

        [Header("People")]
        private BigFloat _people = 1;

        [Header("Person Income")]
        [SerializeField] private float _personIncome = 1;
        public float IncrementForPersonIncome;

        [Header("Life Quality")]
        [SerializeField] private float _lifeQuality = 1;
        public float IncrementForLifeQuality;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _moneyLabel;
        [SerializeField] private TextMeshProUGUI _peopleLabel;
        [SerializeField] private TextMeshProUGUI _incomeLabel;

        #region Propeties

        public static CurrencyInfo Currency => Instance._currency;

        public static BigFloat Money {
            get => Instance._money;
            set {
                Instance._money = value;
                Instance.UpdateMoneyLabel();
            }
        }

        public static BigFloat People {
            get => Instance._people.Floor();
            set {
                Instance._people = value;
                Instance.UpdatePeopleLabel();
                Instance.UpdateIncomeLabel();
            }
        }

        public static float PersonIncome {
            get => Instance._personIncome;
            set {
                Instance._personIncome = value;
                Instance.UpdateIncomeLabel();
            }
        }

        public static float LifeQuality {
            get => Instance._lifeQuality;
            set {
                Instance._lifeQuality = value;
                Instance.UpdateIncomeLabel();
            }
        }

        #endregion

        protected override void Awake() {
            base.Awake();

            Prestige = PlayerPrefs.GetFloat("Prestige", Prestige);
            PlayerPrefs.DeleteKey("Prestige");
        }

        private void Start() {
            _money = _startMoney;

            UpdateMoneyLabel();
            UpdatePeopleLabel();
            UpdateIncomeLabel();

            AgentManager.Instance.InstantiatePeople();

            StartCoroutine(IncrementMoney());
        }

        private IEnumerator IncrementMoney() {
            while (true) {
                Money += IncomePerSecond();

                yield return new WaitForSeconds(1f);
            }
        }

        public static void DecrementMoney(BigFloat money) {
            Money -= money;
        }

        public static void IncrementPersonIncome() {
            PersonIncome = GetNextPersonIncome();
        }

        public static void IncrementPeople(int level) {
            People += GetPeopleIncrement(level);
            AgentManager.Instance.InstantiatePeople();
        }

        public static void IncrementLifeQuality() {
            LifeQuality = GetNextLifeQuality();
        }

        private static BigFloat IncomePerSecond() {
            BigFloat tmp = new BigFloat(GetPrestigeCoefficient() * (1 + Mathf.Log(LifeQuality)) * PersonIncome);

            return BigFloat.Multiply(People, tmp);
        }

        #region UI

        private void UpdateMoneyLabel() {
            _moneyLabel.text = Currency.ConvertToString(Money);
        }

        private void UpdatePeopleLabel() {
            _peopleLabel.text = Currency.ConvertToString(People);
        }

        private void UpdateIncomeLabel() {
            _incomeLabel.text = Currency.ConvertToString(BigFloat.Multiply(IncomePerSecond(), 60));
        }

        #endregion

        public static BigFloat GetPeopleIncrement(int level) {
            return BigFloat.Round(BigFloat.Sqrt(BigFloat.Sqrt(level)) * 10);
        }

        public static float GetNextPersonIncome() {
            return PersonIncome + Instance.IncrementForPersonIncome;
        }

        public static float GetNextLifeQuality() {
            return LifeQuality + Instance.IncrementForLifeQuality;
        }

        public static BigFloat GetPrestigeCoefficient() {
            return BigFloat.Pow(1000, (int)Instance.Prestige - 1);
        }
    }
}
