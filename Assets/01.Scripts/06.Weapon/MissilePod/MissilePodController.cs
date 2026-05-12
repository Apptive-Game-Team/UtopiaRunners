using _01.Scripts._07.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Scripts._06.Weapon.MissilePod
{
    public class MissilePodController : WeaponController
    {
        private int _jumpAndSlideCount;
        public int missileCount;
        
        protected override void Start()
        {
            base.Start();
            SetJumpAndSlideCount();
        }

        private void SetJumpAndSlideCount()
        {
            PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsInactive.Include,FindObjectsSortMode.None);

            foreach (PlayerController player in players)
            {
                player.OnJumpDetected += () =>
                {
                    _jumpAndSlideCount++;
                    if (_jumpAndSlideCount == 10)
                    {
                        _jumpAndSlideCount = 0;
                        missileCount++;
                    }
                };

                player.OnSlideDetected += () =>
                {
                    _jumpAndSlideCount++;
                    if (_jumpAndSlideCount == 10)
                    {
                        _jumpAndSlideCount = 0;
                        missileCount++;
                    }
                };
            }
        }
    }
}
