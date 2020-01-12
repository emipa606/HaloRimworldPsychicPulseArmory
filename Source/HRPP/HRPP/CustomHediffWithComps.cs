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
                this.pawn.health.AddHediff(HediffDefOf.HRPP_downed);
            }
            else if(this.pawn.health.hediffSet.hediffs.Where((Hediff h) => h.def.defName == "HRPP_downed") is Hediff hediff && hediff != null)
            {
                this.pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
