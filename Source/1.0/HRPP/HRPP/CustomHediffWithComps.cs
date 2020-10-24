using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using UnityEngine;

namespace HRPP
{
    class CustomHediffWithComps : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            this.severityInt -= 0.00002f;
            if (this.severityInt > 0.8)
            {
                this.pawn.health.AddHediff(HRPP_HediffDefOf.HRPP_downed);
            }
            else if(this.pawn.health.hediffSet.HasHediff(HRPP_HediffDefOf.HRPP_downed) && this.severityInt < 0.6)
            {
                this.pawn.health.RemoveHediff(this.pawn.health.hediffSet.GetFirstHediffOfDef(HRPP_HediffDefOf.HRPP_downed));
            }
            else if (this.severityInt > 1.4)
            {
                this.severityInt = 1;
            }
        }
    }
}
