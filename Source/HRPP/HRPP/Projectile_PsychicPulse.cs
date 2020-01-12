using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPP
{
    public class Projectile_PsychicPulse : Bullet
    {
        public ThingDef_PsychicPulse Def
        {
            get
            {
                return this.def as ThingDef_PsychicPulse;
            }
        }

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                var rand = Rand.Value; // This is a random percentage between 0% and 100%
                if (rand <= Def.baseline) // If the percentage falls under the chance, success!
                {
                    //This checks to see if the character has a heal differential, or hediff on them already.
                    var hediffOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                    // Severity percentage to add calculation based on quality
                    float severity = Def.baseline;
                    QualityCategory quality;
                    this.launcher.TryGetQuality(out quality);
                    if(quality == QualityCategory.Awful)
                    {
                        severity = severity * 0.5f;
                    }
                    else if (quality == QualityCategory.Poor)
                    {
                        severity -= severity * 0.25f;
                    }
                    else if (quality == QualityCategory.Good)
                    {
                        severity += severity * 0.25f;
                    }
                    else if (quality == QualityCategory.Excellent)
                    {
                        severity += severity * 0.5f;
                    }
                    else if (quality == QualityCategory.Masterwork)
                    {
                        severity += severity * 0.75f;
                    }
                    else if (quality == QualityCategory.Legendary)
                    {
                        severity += severity;
                    }
                    // End
                    // Severity adjustment base on pawn traits
                    
                    // End
                    if (hediffOnPawn != null)
                    {
                        //If they already have the hediff
                        hediffOnPawn.Severity += severity;
                    }
                    else
                    {
                        //These three lines create a new health differential or Hediff,
                        //put them on the character, and increase its severity by a random amount.
                        Hediff hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn);
                        hediff.Severity = severity;
                        hitPawn.health.AddHediff(hediff);
                    }
                }
                var rand2 = Rand.Value; // This is another random percentage between 0% and 100%
            }
        }
    }
}
