/*  Ice Hockey Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Ice Hockey Objective
 * 
 */

public class IceHockeyObjective : Objective {
#region IceHockeyObjective Methods
    override protected void SetUI() {
        calligrapher.IceHockeyInit(e_color);
    }

    override protected void ResetUI() {
        calligrapher.ScoreReset(e_color);
    }

    // Update UI and check for completion
    public void UpdatePuckScore() {
        i_score++;
        calligrapher.UpdateScoreUI(e_color, i_score);
        if (i_score >= Constants.ObjectiveStats.C_HockeyMaxScore) {
            b_isComplete = true;
        }
    }
#endregion

#region Unity Overrides
    void OnEnable() {
        maestro.PlayBeginHockey();
    }
#endregion
}
