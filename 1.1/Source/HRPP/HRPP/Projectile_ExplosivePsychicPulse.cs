using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPPA
{
    class Projectile_ExplosivePsychicPulse : Projectile_Explosive
    {
        protected override void Explode()
        {
            float explosionRadius = this.def.projectile.explosionRadius;
            IEnumerable<IntVec3> c = GenRadial.RadialCellsAround(this.Position, explosionRadius, true);
            foreach (IntVec3 intvec in c)
            {
                if (intvec.InBounds(this.Map))
                {
                    List<Thing> thingList = intvec.GetThingList(this.Map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        Pawn pawn = thingList[j] as Pawn;
                        if (pawn != null && pawn.RaceProps.intelligence >= Intelligence.Humanlike)
                        {
                            //This checks to see if the character has a heal differential, or hediff on them already.
                            var hediffOnPawn = pawn.health?.hediffSet?.GetFirstHediffOfDef(HRPPA_HediffDefOf.HRPPA_PsychicShock);
                            // Severity percentage to add calculation based on quality
                            float severity = 0.12f;
                            QualityCategory quality;
                            this.launcher.TryGetQuality(out quality);
                            if (quality == QualityCategory.Awful)
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
                            severity *= pawn.GetStatValue(StatDefOf.PsychicSensitivity);
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
                                Hediff hediff = HediffMaker.MakeHediff(HRPPA_HediffDefOf.HRPPA_PsychicShock, pawn);
                                hediff.Severity = severity;
                                pawn.health.AddHediff(hediff);
                            }

                            var rand2 = Rand.Value; // This is another random percentage between 0% and 100%
                            if (rand2 <= 0.05)
                            {
                                // Make the pawn insane (go berserk)
                                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "Hit by psychic pulse", true, false);
                            }
                        }
                    }
                }
            }
            base.Explode();
        }
    }
}
