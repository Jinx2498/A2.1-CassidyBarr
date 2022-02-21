#region Derived from Mat Buckland's Programming Game AI by Example
// Please see Buckland's book for the original C++ code and examples.
#endregion

using GameBrains.Entities.V1;

namespace GameBrains.WestWorld.Entities
{
    public class MinerEntity : Entity
    {
        // The amount of gold a miner must have before he feels comfortable.
        public int comfortLevel = 5;
        
        // The amount of nuggets a miner can carry.
        public int maximumNuggets = 3;
        
        // Above this value a miner is thirsty.
        public int thirstLevel = 5;
        
        // Above this value a miner is sleepy.
        public int tirednessThreshold = 10;
        
        // Gets or sets the level of thirst.
        public int Thirst { get; set; }
        
        // Gets a value indicating whether the miner's pockets are full of gold.
        public bool ArePocketsFull => GoldCarried >= maximumNuggets;
        
        // Gets or sets the fatigue level.
        public int Fatigue { get; set; }
        
        // Gets or sets the amount of gold carried.
        public int GoldCarried { get; set; }
        
        // Gets a value indicating whether the miner is tired.
        public bool IsFatigued => Fatigue >= tirednessThreshold;
        
        // Gets a value indicating whether the miner is thirsty.
        public bool IsThirsty => Thirst >= thirstLevel;
        
        // Gets or sets the miner's location.
        public Locations Location { get; set; }
        
        // Gets or sets the amount of gold in the bank.
        public int MoneyInBank { get; set; }

        public override void Awake()
        {
            base.Awake();

            Location = Locations.Shack;
            GoldCarried = 0;
            MoneyInBank = 0;
            Thirst = 0;
            Fatigue = 0;
        }
        
        // Add to the amount of gold carried.
        public void AddToGoldCarried(int amount)
        {
            GoldCarried += amount;
            if (GoldCarried < 0)
            {
                GoldCarried = 0;
            }
        }
        
        // Add to the gold in the bank.
        public void AddToMoneyInBank(int amount)
        {
            MoneyInBank += amount;
            if (MoneyInBank < 0)
            {
                MoneyInBank = 0;
            }
        }
        
        // Buy and drink whiskey. Quenches this but costs 2 gold.
        public void BuyAndDrinkAWhiskey()
        {
            Thirst = 0;
            MoneyInBank -= 2;
        }
        
        // Decrease the fatigue level.
        public void DecreaseFatigue()
        {
            Fatigue -= 1;
        }

        // Increase the fatigue level.
        public void IncreaseFatigue()
        {
            Fatigue += 1;
        }
        
        // Increase the thirst level.
        public void IncreaseThirst()
        {
            Thirst += 1;
        }
    }
}