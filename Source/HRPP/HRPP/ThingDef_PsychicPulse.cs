using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPP
{
    public class ThingDef_PsychicPulse : ThingDef
    {
        public float baseline;
        public float insanityChance = 0.05f;
        public HediffDef HediffToAdd;
    }
}
