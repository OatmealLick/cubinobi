using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cubinobi
{
    public class Effects
    {
        private readonly Dictionary<HitEffect, float> EffectsAndDurations = new();

        public void Update()
        {
            var copiedKeys = EffectsAndDurations.Keys.ToList();
            copiedKeys.ForEach(he =>
            {
                EffectsAndDurations[he] -= Time.deltaTime;
                if (EffectsAndDurations[he] < 0)
                {
                    EffectsAndDurations.Remove(he);
                }
            });
        }

        public bool Has(HitEffect effect)
        {
            return EffectsAndDurations.ContainsKey(effect);
        }

        public void Add(HitEffect effect, float duration)
        {
            if (Has(effect))
            {
                EffectsAndDurations[effect] = Math.Max(EffectsAndDurations[effect], duration);
            }
            else
            {
                EffectsAndDurations.Add(effect, duration);
            }
        }
    }
}