﻿namespace Assets.Scripts.Monsters
{
    public class Klorslug : Monster
    {
        public override int AttackRating
        {
            get { return 7; }
        }

        public override int DefenseRating
        {
            get { return 3; }
        }

        public override int MovementRating
        {
            get { return 2; }
        }
    }
}
