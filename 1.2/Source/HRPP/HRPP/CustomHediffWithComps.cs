using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPPA
{
    class CustomHediffWithComps : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            this.severityInt -= 0.00002f;
            if (this.severityInt > 0.8)
            {
                this.pawn.health.AddHediff(HRPPA_HediffDefOf.HRPPA_downed);
            }
            else if(this.pawn.health.hediffSet.HasHediff(HRPPA_HediffDefOf.HRPPA_downed) && this.severityInt < 0.6)
            {
                this.pawn.health.RemoveHediff(this.pawn.health.hediffSet.GetFirstHediffOfDef(HRPPA_HediffDefOf.HRPPA_downed));
            }
            else if (this.severityInt > 1.4)
            {
                this.severityInt = 1;
            }
        }
    }
}
