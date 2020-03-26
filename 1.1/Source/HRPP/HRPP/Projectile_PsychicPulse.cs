using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPPA
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
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn && hitPawn != null)
            {
                if(hitPawn.RaceProps != null && hitPawn.RaceProps.intelligence == Intelligence.Animal && hitPawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.Manhunter && hitPawn.Faction != Faction.OfPlayer)
                {
                    System.Random random = new System.Random();
                    int rn = random.Next(0, 101);
                    if (rn > 50)
                    {
                        List<Pawn> sameAnimalRaces = (from p in hitPawn.Map.mapPawns.AllPawnsSpawned
                                                      where p.kindDef == hitPawn.kindDef
                                                      select p).ToList<Pawn>();
                        if(sameAnimalRaces.Count > 0)
                        {
                            foreach (Pawn item in sameAnimalRaces)
                            {
                                item.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                            }
                            Find.LetterStack.ReceiveLetter("LetterLabelAnimalInsanityMultiple".Translate(hitPawn.kindDef.GetLabelPlural(-1)), "AnimalInsanityMultiple".Translate(hitPawn.kindDef.GetLabelPlural(-1)), LetterDefOf.ThreatBig, hitPawn);
                        }
                        else
                        {
                            hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                            Find.LetterStack.ReceiveLetter("LetterLabelAnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")), "AnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")), LetterDefOf.ThreatBig, hitPawn);
                        }
                    }
                    else if (rn > 25)
                    {
                        hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                        Find.LetterStack.ReceiveLetter("LetterLabelAnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")), "AnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")), LetterDefOf.ThreatBig, hitPawn);
                    }
                }
                MoteMaker.MakeAttachedOverlay(hitPawn, ThingDefOf.Mote_PsycastPsychicEffect, Vector3.zero, 1f, -1f);
                MoteMaker.MakeConnectingLine(this.launcher.DrawPos, hitPawn.DrawPos, ThingDefOf.Mote_PsycastPsychicLine, this.launcher.Map, 1f);
                //This checks to see if the character has a heal differential, or hediff on them already.
                var hediffOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                // Severity percentage to add calculation based on quality
                float severity = Def.baseline;
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
                severity *= hitPawn.GetStatValue(StatDefOf.PsychicSensitivity);
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

                var rand2 = Rand.Value; // This is another random percentage between 0% and 100%
                if (rand2 <= 0.05)
                {
                    // Make the pawn insane (go berserk)
                    hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "Hit by psychic pulse", true, false);
                }

            }
        }
    }
}
