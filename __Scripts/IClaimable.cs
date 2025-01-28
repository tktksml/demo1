using UnityEngine;


namespace FTK.GamePlayLib.MapLib.BuildingsLib
{
    public interface IClaimable
    {
        public Transform TransformCache { get; }


        public bool IsFullyClaimed { get; set; }
        public Team ClaimedTeam { get; set; }








        public void OnClaiming(float claimValue, Team claimTeam);
    }
}