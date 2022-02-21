#region Derived from Mat Buckland's Programming Game AI by Example
// Please see Buckland's book for the original C++ code and examples.
#endregion

using GameBrains.Entities.V1;

namespace GameBrains.WestWorld.Entities
{
    public class MinersWifeEntity : Entity
    {
        // Gets or sets a value indicating whether the wife is cooking.
        public bool IsCooking { get; set; }

        // Gets or sets the miner's wife's location.
        public Locations Location { get; set; }

        public override void Awake()
        {
            base.Awake();
            
            Location = Locations.Shack;
        }
    }
}