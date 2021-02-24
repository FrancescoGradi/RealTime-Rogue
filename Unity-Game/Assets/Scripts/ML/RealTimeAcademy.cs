using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy
{
  public override void AcademyReset() {

      FindObjectOfType<GameManager>().ResetTrainingScene();
  }
}
