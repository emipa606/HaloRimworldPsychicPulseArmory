using Verse;

namespace HRPPA
{
    internal class CustomHediffWithComps : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            severityInt -= 0.00002f;
            if (severityInt > 0.8)
            {
                pawn.health.AddHediff(HRPPA_HediffDefOf.HRPPA_downed);
            }
            else if (pawn.health.hediffSet.HasHediff(HRPPA_HediffDefOf.HRPPA_downed) && severityInt < 0.6)
            {
                pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HRPPA_HediffDefOf.HRPPA_downed));
            }
            else if (severityInt > 1.4)
            {
                severityInt = 1;
            }
        }
    }
}