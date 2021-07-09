using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using Random = System.Random;

namespace HRPPA
{
    public class Projectile_PsychicPulse : Projectile
    {
        public ThingDef_PsychicPulse Def => def as ThingDef_PsychicPulse;

        protected override void Impact(Thing hitThing)
        {
            var map = Map;
            base.Impact(hitThing);
            var battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing,
                equipmentDef, def, targetCoverDef);
            Find.BattleLog.Add(battleLogEntry_RangedImpact);
            if (hitThing != null)
            {
                FleckMaker.AttachedOverlay(hitThing, FleckDefOf.PsycastAreaEffect, Vector3.zero);
                if (hitThing is Pawn hitPawn)
                {
                    if (hitPawn.RaceProps != null && hitPawn.RaceProps.intelligence == Intelligence.Animal &&
                        hitPawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.Manhunter &&
                        hitPawn.Faction != Faction.OfPlayer)
                    {
                        var random = new Random();
                        var rn = random.Next(0, 101);
                        if (rn > 50)
                        {
                            var sameAnimalRaces = (from p in hitPawn.Map.mapPawns.AllPawnsSpawned
                                where p.kindDef == hitPawn.kindDef
                                select p).ToList();
                            if (sameAnimalRaces.Count > 0)
                            {
                                foreach (var item in sameAnimalRaces)
                                {
                                    item.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                                }

                                Find.LetterStack.ReceiveLetter(
                                    "LetterLabelAnimalInsanityMultiple".Translate(hitPawn.kindDef.GetLabelPlural()),
                                    "AnimalInsanityMultiple".Translate(hitPawn.kindDef.GetLabelPlural()),
                                    LetterDefOf.ThreatBig, hitPawn);
                            }
                            else
                            {
                                hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                                Find.LetterStack.ReceiveLetter(
                                    "LetterLabelAnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")),
                                    "AnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")),
                                    LetterDefOf.ThreatBig, hitPawn);
                            }
                        }
                        else if (rn > 25)
                        {
                            hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                            Find.LetterStack.ReceiveLetter(
                                "LetterLabelAnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")),
                                "AnimalInsanitySingle".Translate(hitPawn.Label, hitPawn.Named("ANIMAL")),
                                LetterDefOf.ThreatBig, hitPawn);
                        }
                    }

                    //This checks to see if the character has a heal differential, or hediff on them already.
                    var hediffOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                    // Severity percentage to add calculation based on quality
                    var severity = Def.baseline;
                    launcher.TryGetQuality(out var quality);
                    if (quality == QualityCategory.Awful)
                    {
                        severity *= 0.5f;
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
                        var hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn);
                        hediff.Severity = severity;
                        hitPawn.health.AddHediff(hediff);
                    }

                    var rand2 = Rand.Value; // This is another random percentage between 0% and 100%
                    if (rand2 <= 0.05)
                    {
                        // Make the pawn insane (go berserk)
                        hitPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk,
                            "Hit by psychic pulse", true);
                    }
                }

                var dinfo = new DamageInfo(def.projectile.damageDef, DamageAmount, ArmorPenetration,
                    ExactRotation.eulerAngles.y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown,
                    intendedTarget.Thing);
                hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
                if (hitThing is Pawn {stances: { }} pawn && pawn.BodySize <= def.projectile.StoppingPower + 0.001f)
                {
                    pawn.stances.StaggerFor(95);
                }

                if (def.projectile.extraDamages == null)
                {
                    return;
                }

                using var enumerator = def.projectile.extraDamages.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var extraDamage = enumerator.Current;
                    if (extraDamage != null && !Rand.Chance(extraDamage.chance))
                    {
                        continue;
                    }

                    if (extraDamage == null)
                    {
                        continue;
                    }

                    var dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount,
                        extraDamage.AdjustedArmorPenetration(), ExactRotation.eulerAngles.y, launcher, null,
                        equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing);
                    hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
                }

                return;
            }

            SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(Position, map));
            if (Position.GetTerrain(map).takeSplashes)
            {
                FleckMaker.WaterSplash(ExactPosition, map, Mathf.Sqrt(DamageAmount) * 1f, 4f);
                return;
            }

            FleckMaker.Static(ExactPosition, map, FleckDefOf.PsycastAreaEffect);
            FleckMaker.Static(ExactPosition, map, FleckDefOf.ShotHit_Dirt);
        }
    }
}