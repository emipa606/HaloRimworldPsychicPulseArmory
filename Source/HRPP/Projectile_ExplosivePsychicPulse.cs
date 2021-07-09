using RimWorld;
using Verse;

namespace HRPPA
{
    internal class Projectile_ExplosivePsychicPulse : Projectile_Explosive
    {
        protected override void Explode()
        {
            var explosionRadius = def.projectile.explosionRadius;
            var c = GenRadial.RadialCellsAround(Position, explosionRadius, true);
            FleckMaker.Static(Position.ToVector3(), Map, FleckDefOf.PsycastAreaEffect, 3f);
            foreach (var intvec in c)
            {
                if (!intvec.InBounds(Map))
                {
                    continue;
                }

                var thingList = intvec.GetThingList(Map);
                foreach (var thing in thingList)
                {
                    if (thing is not Pawn pawn || pawn.RaceProps.intelligence < Intelligence.Humanlike)
                    {
                        continue;
                    }

                    //This checks to see if the character has a heal differential, or hediff on them already.
                    var hediffOnPawn =
                        pawn.health?.hediffSet?.GetFirstHediffOfDef(HRPPA_HediffDefOf.HRPPA_PsychicShock);
                    // Severity percentage to add calculation based on quality
                    var severity = 0.12f;
                    launcher.TryGetQuality(out var quality);
                    switch (quality)
                    {
                        case QualityCategory.Awful:
                            severity *= 0.5f;
                            break;
                        case QualityCategory.Poor:
                            severity -= severity * 0.25f;
                            break;
                        case QualityCategory.Good:
                            severity += severity * 0.25f;
                            break;
                        case QualityCategory.Excellent:
                            severity += severity * 0.5f;
                            break;
                        case QualityCategory.Masterwork:
                            severity += severity * 0.75f;
                            break;
                        case QualityCategory.Legendary:
                            severity += severity;
                            break;
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
                        var hediff = HediffMaker.MakeHediff(HRPPA_HediffDefOf.HRPPA_PsychicShock, pawn);
                        hediff.Severity = severity;
                        pawn.health.AddHediff(hediff);
                    }

                    var rand2 = Rand.Value; // This is another random percentage between 0% and 100%
                    if (rand2 <= 0.05)
                    {
                        // Make the pawn insane (go berserk)
                        pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk,
                            "Hit by psychic pulse", true);
                    }
                }
            }

            base.Explode();
        }
    }
}